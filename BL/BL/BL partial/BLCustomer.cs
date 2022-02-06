using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            if (CheckNumOfDigits( customer.Id )!= 9)//Input check
                throw new WrongIDException("Bad custumer ID");
            if (customer.Location.Latitude < 31 || customer.Location.Latitude > 32
             || customer.Location.Longitude < 35 || customer.Location.Longitude > 36)//Checking that the location is in the allowed range (Jerusalem area)
                throw new UnlogicalLocationException("The location is unlogical");
            customer.FromCustomer = new List<ParcelByCustomer>();
            customer.ToCustomer = new List<ParcelByCustomer>();
            lock (dal)
            {
                try
                {
                    DO.Customer customerDO = new();
                    object obj = customerDO;//boxing and unBoxing
                    customer.CopyPropertiesTo(obj);
                    customerDO = (DO.Customer)obj;
                    //needs to update by hand the location
                    customerDO.Longitude = customer.Location.Longitude;
                    customerDO.Latitude = customer.Location.Latitude;
                    if (!dal.Add(customerDO))//calls the function from DALOBJECT
                        throw new AskRecoverExeption($"The customer has been deleted. Are you sure you want to recover? ");
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CustonerRecover(Customer customer)
        {//a fanction that recover a customer
            lock (dal)
            {
                try
                {
                    dal.UpdateCustomer(customer.Id, customer.Name, customer.PhoneNumber
                        , customer.Location.Longitude, customer.Location.Latitude); //calls the function from DALOBJECT
                }
                catch(ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(Customer customer)
        {
            lock (dal)
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
                    dal.RemoveCustomer(customerDO.Id);
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            if (customer.Name == null || customer.Name == "")
                throw new WrongInputException("Missing customer name");
            if (customer.PhoneNumber == null || customer.PhoneNumber == "")
                throw new WrongInputException("Missing phone number");
            CustomerToList Listcustomer = GetListCustomer().FirstOrDefault(i => i.Id == customer.Id);
            lock (dal)
            {
                try
                {
                    customer.CopyPropertiesTo(Listcustomer);
                    dal.UpdateCustomer(customer.Id, customer.Name, customer.PhoneNumber);//calls the function from DALOBJECT
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int ID)
        {
            lock (dal)
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
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
                //set the collections of parcels - parcels sent from the customer / parcels sent by the customer
                customerBO.FromCustomer = (from item in dal.GetListParcel(i => i.Sender == ID)
                                           select HelpMethodToSetParcelInCustomer(item, customerBO));
                customerBO.ToCustomer = (from item in dal.GetListParcel(i => i.Targetid == ID)
                                         select HelpMethodToSetParcelInCustomer(item, customerBO));
                return customerBO;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null)
        {
            lock (dal)
            {
                IEnumerable<CustomerToList> customerSBO = from c in dal.GetListCustomer(c => !c.IsRemoved)
                                                          select c.CopyPropertiesTo(new CustomerToList()
                                                          {
                                                              NumberParcelSentAndDelivered = dal.GetListParcel(i => i.Sender == c.Id && i.Delivered != null).Count(),
                                                              NumberParcelSentAndNOTDelivered = dal.GetListParcel(i => i.Sender == c.Id && i.Delivered == null).Count(),
                                                              NumberOfParcelReceived = dal.GetListParcel(i => i.Targetid == c.Id && i.Delivered != null).Count(),
                                                              NumberOfParcelOnTheWayToCustomer = dal.GetListParcel(i => i.Targetid == c.Id && i.Delivered == null).Count()
                                                          });
                return customerSBO.Where(i => predicate == null ? true : predicate(i));
            }
        }
    }
}