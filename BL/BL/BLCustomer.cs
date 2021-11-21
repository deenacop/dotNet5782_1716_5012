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
        /// <summary>
        /// Adds a customer to the list of customers in the IDAL
        /// </summary>
        /// <param name="customer">the wanted customer</param>
        public void AddCustomer(Customer customer)
        {
            if (ChackingNumOfDigits(customer.CustomerID) != 9)
                throw new WrongIDException("Worng ID");
            if (customer.CustomerLocation.Latitude < 31 || customer.CustomerLocation.Latitude > 32
             || customer.CustomerLocation.Longitude < 35 || customer.CustomerLocation.Longitude > 36)
                throw new UnlogicalLocationException("The location is not logical");
            try
            {
                IDAL.DO.Customer customerDO = new IDAL.DO.Customer();
                customer.CopyPropertiesTo(customerDO);
                dal.Add(customerDO);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }

        /// <summary>
        /// The function updates the customer name or phone number, by the users request
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <param name="name">customer name</param>
        /// <param name="phone">customer phone</param>
        public void UpdateCustomer(int ID, string name = null, string phone = null)
        {
            try
            {
                dal.UpdateCustomer(ID, name, phone);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        /// <summary>
        /// Display one customer
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <returns>The wanted customer</returns>
        public Customer CustomerDisplay(int ID)
        {
            IDAL.DO.Customer customerDO = new();
            try
            {
                customerDO = dal.CustomerDisplay(ID);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            Customer customerBO = new();
            customerDO.CopyPropertiesTo(customerBO);
            customerBO.CustomerLocation.Longitude = customerDO.Longitude;
            customerBO.CustomerLocation.Latitude = customerDO.Latitude;

            List<IDAL.DO.Parcel> SenderParcels = dal.ListParcelDisplay(i => i.Sender == ID).ToList();//the list of the parcels that the customer send
            List<IDAL.DO.Parcel> ReceiverParcels = dal.ListParcelDisplay(i => i.Targetid == ID).ToList();//the list of the parcels that the customer received

            foreach (IDAL.DO.Parcel currentParcel in SenderParcels)
            {
                ParcelByCustomer parcel = new();
                currentParcel.CopyPropertiesTo(parcel);

                parcel.SecondSideOfParcelCustomer.CustomerID = currentParcel.Targetid;
                parcel.SecondSideOfParcelCustomer.Name = dal.CustomerDisplay(currentParcel.Targetid).Name;
                if (currentParcel.Scheduled == DateTime.MinValue)//not schedule yet
                    parcel.ParcelStatus = @enum.ParcelStatus.Defined;
                else if (currentParcel.PickUp == DateTime.MinValue)//scheduled but has not been picked up
                    parcel.ParcelStatus = @enum.ParcelStatus.Associated;
                else if (currentParcel.Delivered == DateTime.MinValue) //scheduled and picked up  but has not been delivered
                    parcel.ParcelStatus = @enum.ParcelStatus.PickedUp;
                else parcel.ParcelStatus = @enum.ParcelStatus.Delivered;
                //add the parcel to the list
                customerBO.FromCustomer.Add(parcel);
            }

            foreach (IDAL.DO.Parcel currentParcel in ReceiverParcels)
            {
                ParcelByCustomer parcel = new();
                currentParcel.CopyPropertiesTo(parcel);

                parcel.SecondSideOfParcelCustomer.CustomerID = currentParcel.Sender;
                parcel.SecondSideOfParcelCustomer.Name = dal.CustomerDisplay(currentParcel.Sender).Name;
                parcel.ParcelStatus = @enum.ParcelStatus.Delivered;//the status id delivered cause its by the targetid..
                //add the parcel to the list
                customerBO.TOCustomer.Add(parcel);
            }

            return customerBO;
        }
        /// <summary>
        /// Displays the list of the customerToList
        /// </summary>
        /// <returns>The list of the customer</returns>
        public IEnumerable<CustomerToList> ListCustomerDsplay()
        {
            List<IDAL.DO.Customer> customerDO = new();
            try
            {
                customerDO = dal.ListCustomerDisplay().ToList();
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            CustomerToList tmpCustomerToList = new();
            List<CustomerToList> customerToLists = new();
            foreach (IDAL.DO.Customer currentCustomer in customerDO)
            {
                currentCustomer.CopyPropertiesTo(tmpCustomerToList);
                //brings all the parcels that were send by the current customer and were delivered
                List<IDAL.DO.Parcel> parcelsSendAndDelivered = dal.ListParcelDisplay(i => i.Sender == currentCustomer.CustomerID && i.Delivered != DateTime.MinValue).ToList();
                tmpCustomerToList.NumberParcelSentAndDelivered = parcelsSendAndDelivered.Count;
                //brings all the parcels that were send by the current customer and werent delivered
                List<IDAL.DO.Parcel> parcelsSendAndNOTDelivered = dal.ListParcelDisplay(i => i.Sender == currentCustomer.CustomerID && i.Delivered == DateTime.MinValue).ToList();
                tmpCustomerToList.NumberParcelSentAndDelivered = parcelsSendAndNOTDelivered.Count;
                //brings all the parcels that were received by the current customer and were delivered
                List<IDAL.DO.Parcel> parcelsReceived = dal.ListParcelDisplay(i => i.Targetid == currentCustomer.CustomerID && i.Delivered != DateTime.MinValue).ToList();
                tmpCustomerToList.NumberOfParcelReceived = parcelsReceived.Count;
                //brings all the parcels that were received by the current customer and werent delivered
                List<IDAL.DO.Parcel> parcelsOnTheWay = dal.ListParcelDisplay(i => i.Targetid == currentCustomer.CustomerID && i.PickUp != DateTime.MinValue).ToList();
                tmpCustomerToList.NumberOfParcelOnTheWayToCustomer = parcelsOnTheWay.Count;
                customerToLists.Add(tmpCustomerToList);
            }
            return customerToLists;
        }
    }
}
