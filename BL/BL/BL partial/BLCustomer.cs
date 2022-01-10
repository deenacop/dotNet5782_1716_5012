using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DalApi;


namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        public void AddCustomer(Customer customer)
        {
            if (CheckNumOfDigits(customer.Id) != 9)//בדיקה
                throw new WrongIDException("Bad custumer ID");
            if (customer.Location.Latitude < 31 || customer.Location.Latitude > 32
             || customer.Location.Longitude < 35 || customer.Location.Longitude > 36)//Checking that the location is in the allowed range (Jerusalem area)
                throw new UnlogicalLocationException("The location is unlogical");
            try
            {
                DO.Customer customerDO = new();
                object obj = customerDO;//boxing and unBoxing
                customer.CopyPropertiesTo(obj);
                customerDO = (DO.Customer)obj;
                //needs to update by hand the location
                customerDO.Longitude = customer.Location.Longitude;
                customerDO.Latitude = customer.Location.Latitude;
                dal.Add(customerDO);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            try
            {
                CustomerToList customer1 = GetListCustomer().FirstOrDefault(i => i.Id == customer.Id);
                if (customer1 == null)
                    throw new ItemNotExistException("The customer does not exit");
                DO.Customer customerDO = new();
                object obj = customerDO;//boxing and unBoxing
                customer.CopyPropertiesTo(obj);
                customerDO = (DO.Customer)obj;
                customerDO.Latitude = customer.Location.Latitude;
                customerDO.Longitude = customer.Location.Longitude;
                dal.Remove(customerDO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Name == null || customer.Name == "")
                throw new WrongInputException("Missing drone model");
            if (customer.PhoneNumber == null || customer.PhoneNumber == "")
                throw new WrongInputException("Missing phone number");
            CustomerToList Listcustomer = GetListCustomer().FirstOrDefault(i => i.Id == customer.Id);
            try
            {
                customer.CopyPropertiesTo(Listcustomer);
                dal.UpdateCustomer(customer.Id, customer.Name, customer.PhoneNumber);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public Customer GetCustomer(int ID)
        {
            DO.Customer customerDO = new();
            Customer customerBO = new();
            try
            {
                customerDO = dal.GetCustomer(ID);//ask the wanted customer
                customerDO.CopyPropertiesTo(customerBO);//convert
                customerBO.Location = new()//needs to be initialized by hand
                {
                    Longitude = customerDO.Longitude,
                    Latitude = customerDO.Latitude
                };
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //set the collections of parcels - parcels sent from the customer / parcels sent by the customer
            customerBO.FromCustomer = (from item in dal.GetListParcel(i => i.Sender == ID)
                                       select HelpMethod(item, customerBO));
            customerBO.ToCustomer = (from item in dal.GetListParcel(i => i.Targetid == ID)
                                     select HelpMethod(item, customerBO));
            return customerBO;
        }

        public IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null)
        {
            IEnumerable<DO.Customer> customerDO = dal.GetListCustomer(item => !item.IsRemoved);
            List<CustomerToList> customerToLists = new();
            foreach (var (currentCustomer, customerList) in
                from DO.Customer currentCustomer in customerDO
                let customerList = new CustomerToList()
                select (currentCustomer, customerList))
            {
                currentCustomer.CopyPropertiesTo(customerList);
                customerList.NumberParcelSentAndDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered != null).Count();
                customerList.NumberParcelSentAndNOTDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered == null).Count();
                customerList.NumberOfParcelReceived = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.Delivered != null).Count();
                customerList.NumberOfParcelOnTheWayToCustomer = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered == null).Count();
                customerToLists.Add(customerList);
            }
            //foreach (DO.Customer currentCustomer in customerDO)
            //{
            //    CustomerToList customerList = new();
            //    currentCustomer.CopyPropertiesTo(customerList);
            //    //Brings all the parcels that were send by the current customer and were delivered
            //    IEnumerable<DO.Parcel> parcelsSendAndDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered != null);
            //    customerList.NumberParcelSentAndDelivered = parcelsSendAndDelivered.Count();
            //    //brings all the parcels that were send by the current customer and werent delivered
            //    IEnumerable<DO.Parcel> parcelsSendAndNOTDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered == null && i.PickUp != null);
            //    customerList.NumberParcelSentAndNOTDelivered = parcelsSendAndNOTDelivered.Count();
            //    //brings all the parcels that were received by the current customer and were delivered
            //    IEnumerable<DO.Parcel> parcelsReceived = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.Delivered != null);
            //    customerList.NumberOfParcelReceived = parcelsReceived.Count();
            //    //brings all the parcels that were received by the current customer and werent delivered
            //    IEnumerable<DO.Parcel> parcelsOnTheWay = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.PickUp != null && i.Delivered == null);
            //    customerList.NumberOfParcelOnTheWayToCustomer = parcelsOnTheWay.Count();
            //    customerToLists.Add(customerList);
            //}
            return customerToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}