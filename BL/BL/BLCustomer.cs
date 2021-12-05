using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using BL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddCustomer(Customer customer)
        {
            if (CheckNumOfDigits(customer.Id) != 9)//בדיקה
                throw new WrongIDException("Worng ID");
            if (customer.Location.Latitude < 31 || customer.Location.Latitude > 32
             || customer.Location.Longitude < 35 || customer.Location.Longitude > 36)//בדיקה
                throw new UnlogicalLocationException("The location is not logical");
            try
            {
                IDAL.DO.Customer customerDO = new();
                object obj = customerDO;
                customer.CopyPropertiesTo(obj);
                customerDO = (IDAL.DO.Customer)obj;
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

        public void UpdateCustomer(Customer customer )
        {
            try
            {
                object obj = new IDAL.DO.Customer();
                customer.CopyPropertiesTo(obj);
                dal.UpdateCustomer((IDAL.DO.Customer)obj);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public Customer GetCustomer(int ID)
        {
            IDAL.DO.Customer customerDO = new();
            Customer customerBO = new();
            try
            {
                customerDO = dal.GetCustomer(ID);
                customerDO.CopyPropertiesTo(customerBO);
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

            IEnumerable<IDAL.DO.Parcel> SenderParcels = dal.GetListParcel(i => i.Sender == ID).ToList();//the list of the parcels that the customer send
            IEnumerable<IDAL.DO.Parcel> ReceiverParcels = dal.GetListParcel(i => i.Targetid == ID).ToList();//the list of the parcels that the customer received
            ParcelByCustomer sendParcel = new();

            foreach (IDAL.DO.Parcel currentParcel in SenderParcels)
            {
                currentParcel.CopyPropertiesTo(sendParcel);
                sendParcel.SecondSideOfParcelCustomer = new();
                sendParcel.SecondSideOfParcelCustomer.Id = currentParcel.Targetid;//second side is targetid
                sendParcel.SecondSideOfParcelCustomer.Name = dal.GetCustomer(currentParcel.Targetid).Name;
                if (currentParcel.Scheduled == null)//not schedule yet
                    sendParcel.Status = ParcelStatus.Defined;
                else if (currentParcel.PickUp == null)//scheduled but has not been picked up
                    sendParcel.Status = ParcelStatus.Associated;
                else if (currentParcel.Delivered == null) //scheduled and picked up  but has not been delivered
                    sendParcel.Status = ParcelStatus.PickedUp;
                else sendParcel.Status = ParcelStatus.Delivered;
                //add the parcel to the list
                customerBO.FromCustomer = new();
                customerBO.FromCustomer.Add(sendParcel);
            }
            ParcelByCustomer receiveParcel = new();

            foreach (IDAL.DO.Parcel currentParcel in ReceiverParcels)
            {
                currentParcel.CopyPropertiesTo(receiveParcel);
                receiveParcel.SecondSideOfParcelCustomer = new();
                receiveParcel.SecondSideOfParcelCustomer.Id = currentParcel.Sender;//second side is targetid
                receiveParcel.SecondSideOfParcelCustomer.Name = dal.GetCustomer(currentParcel.Sender).Name;
                receiveParcel.Status =ParcelStatus.Delivered;//the status id delivered cause its by the targetid..
                //add the parcel to the list
                customerBO.ToCustomer = new();
                customerBO.ToCustomer.Add(receiveParcel);
            }

            return customerBO;
        }

        public IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null)
        {
            IEnumerable<IDAL.DO.Customer> customerDO = dal.GetListCustomer();
            List<CustomerToList> customerToLists = new();
            foreach (IDAL.DO.Customer currentCustomer in customerDO)
            {
                CustomerToList tmpCustomerToList = new();
                currentCustomer.CopyPropertiesTo(tmpCustomerToList);
                //brings all the parcels that were send by the current customer and were delivered
                IEnumerable<IDAL.DO.Parcel> parcelsSendAndDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered != null);
                tmpCustomerToList.NumberParcelSentAndDelivered = parcelsSendAndDelivered.Count();
                //brings all the parcels that were send by the current customer and werent delivered
                IEnumerable<IDAL.DO.Parcel> parcelsSendAndNOTDelivered = dal.GetListParcel(i => i.Sender == currentCustomer.Id && i.Delivered == null && i.PickUp!= null);
                tmpCustomerToList.NumberParcelSentAndNOTDelivered = parcelsSendAndNOTDelivered.Count();
                //brings all the parcels that were received by the current customer and were delivered
                IEnumerable<IDAL.DO.Parcel> parcelsReceived = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.Delivered != null);
                tmpCustomerToList.NumberOfParcelReceived = parcelsReceived.Count();
                //brings all the parcels that were received by the current customer and werent delivered
                IEnumerable<IDAL.DO.Parcel> parcelsOnTheWay = dal.GetListParcel(i => i.Targetid == currentCustomer.Id && i.PickUp != null && i.Delivered == null);
                tmpCustomerToList.NumberOfParcelOnTheWayToCustomer = parcelsOnTheWay.Count();
                customerToLists.Add(tmpCustomerToList);
            }
            return customerToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}