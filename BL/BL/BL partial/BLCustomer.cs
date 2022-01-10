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
                customerDO = dal.GetCustomer(ID);//ask th wanted customer
                customerDO.CopyPropertiesTo(customerBO);//convert
                customerBO.Location = new()
                {
                    Longitude = customerDO.Longitude,
                    Latitude = customerDO.Latitude
                };
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //Set the lists of parcels:
            ParcelByCustomer sendParcel = new();
            IEnumerable<DO.Parcel> SenderParcels = dal.GetListParcel(i => i.Sender == ID);//Returns the list of the parcels that the customer send
            IEnumerable<DO.Parcel> ReceiverParcels = dal.GetListParcel(i => i.Targetid == ID);//Returns the list of the parcels that the customer received
            foreach (DO.Parcel currentParcel in SenderParcels)
            {
                currentParcel.CopyPropertiesTo(sendParcel);
                //set the "SecondSideOfParcelCustomer" property in the parcel:
                sendParcel.SecondSideOfParcelCustomer = new();
                sendParcel.SecondSideOfParcelCustomer.Id = currentParcel.Targetid;//second side is targetid
                sendParcel.SecondSideOfParcelCustomer.Name = dal.GetCustomer(currentParcel.Targetid).Name;
                //set the status of the parcel:
                if (currentParcel.Scheduled == null)//not schedule yet
                    sendParcel.Status = ParcelStatus.Defined;
                else if (currentParcel.PickUp == null)//scheduled but has not been picked up
                    sendParcel.Status = ParcelStatus.Associated;
                else if (currentParcel.Delivered == null) //scheduled and picked up  but has not been delivered
                    sendParcel.Status = ParcelStatus.PickedUp;
                else sendParcel.Status = ParcelStatus.Delivered;
                //add the parcel to the list
                customerBO.FromCustomer = new List<ParcelByCustomer>(); ;
                customerBO.FromCustomer.ToList().Add(sendParcel);
            }

            ParcelByCustomer receiveParcel = new();
            foreach (DO.Parcel currentParcel in ReceiverParcels)
            {
                currentParcel.CopyPropertiesTo(receiveParcel);
                //set the "SecondSideOfParcelCustomer" property in the parcel:
                receiveParcel.SecondSideOfParcelCustomer = new();
                receiveParcel.SecondSideOfParcelCustomer.Id = currentParcel.Sender;//second side is targetid
                receiveParcel.SecondSideOfParcelCustomer.Name = dal.GetCustomer(currentParcel.Sender).Name;
                receiveParcel.Status =ParcelStatus.Delivered;//the status is delivered (cause its by the targetid..)
                //add the parcel to the list
                customerBO.ToCustomer = new List<ParcelByCustomer>();
                customerBO.ToCustomer.ToList().Add(receiveParcel);
            }
            return customerBO;
        }

        public IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null)
        {
            IEnumerable<DO.Customer> customerDO = dal.GetListCustomer(item=>!item.IsRemoved);
            List<CustomerToList> customerToLists = new();
            foreach (DO.Customer currentCustomer in customerDO)
            {
                CustomerToList customerList = new();
                currentCustomer.CopyPropertiesTo(customerList);
                //Brings all the parcels that were send by the current customer and were delivered
                IEnumerable<DO.Parcel> parcelsSendAndDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered != null);
                customerList.NumberParcelSentAndDelivered = parcelsSendAndDelivered.Count();
                //brings all the parcels that were send by the current customer and werent delivered
                IEnumerable<DO.Parcel> parcelsSendAndNOTDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered == null && i.PickUp!= null);
                customerList.NumberParcelSentAndNOTDelivered = parcelsSendAndNOTDelivered.Count();
                //brings all the parcels that were received by the current customer and were delivered
                IEnumerable<DO.Parcel> parcelsReceived = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.Delivered != null);
                customerList.NumberOfParcelReceived = parcelsReceived.Count();
                //brings all the parcels that were received by the current customer and werent delivered
                IEnumerable<DO.Parcel> parcelsOnTheWay = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.PickUp != null && i.Delivered == null);
                customerList.NumberOfParcelOnTheWayToCustomer = parcelsOnTheWay.Count();
                customerToLists.Add(customerList);
            }
            return customerToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}