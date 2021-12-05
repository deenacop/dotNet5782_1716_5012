﻿using System;
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
            if (customer.CustomerLocation.Latitude < 31 || customer.CustomerLocation.Latitude > 32
             || customer.CustomerLocation.Longitude < 35 || customer.CustomerLocation.Longitude > 36)//בדיקה
                throw new UnlogicalLocationException("The location is not logical");
            try
            {
                IDAL.DO.Customer customerDO = new();
                object obj = customerDO;
                customer.CopyPropertiesTo(obj);
                customerDO = (IDAL.DO.Customer)obj;
                //needs to update by hand the location
                customerDO.Longitude = customer.CustomerLocation.Longitude;
                customerDO.Latitude = customer.CustomerLocation.Latitude;
                dal.Add(customerDO);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Name == null || customer.Name == "")
               throw new WrongInputException("Missing drone model");
            if (customer.PhoneNumber == null || customer.PhoneNumber == "")
                throw new WrongInputException("Missing drone model");
            CustomerToList Listcustomer = GetListCustomer().FirstOrDefault(i => i.Id == customer.Id);
            customer.CopyPropertiesTo(Listcustomer);
            try
            {
                dal.UpdateCustomer(customer.Id, customer.Name, customer.PhoneNumber);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        //public void UpdateDrone(Drone drone)
        //{
        //    if (drone.Model == null || drone.Model == "")
        //        throw new WrongInputException("Missing drone model");

        //    DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
        //    if (listDrone == null)
        //        throw new ItemNotExistException("Drone does not exist");
        //    drone.CopyPropertiesTo(listDrone);

        //    object obj = new IDAL.DO.Drone();
        //    drone.CopyPropertiesTo(obj);
        //    try
        //    {
        //        dal.UpdateDrone((IDAL.DO.Drone)obj);//calls the function from DALOBJECT
        //    }
        //    catch (IDAL.DO.ItemNotExistException ex)
        //    {
        //        throw new ItemNotExistException("Drone does not exist", ex);
        //    }

        public Customer GetCustomer(int ID)
        {
            IDAL.DO.Customer customerDO = new();
            Customer customerBO = new();
            try
            {
                customerDO = dal.CustomerDisplay(ID);
                customerDO.CopyPropertiesTo(customerBO);
                customerBO.CustomerLocation = new()
                {
                    Longitude = customerDO.Longitude,
                    Latitude = customerDO.Latitude
                };
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }

            IEnumerable<IDAL.DO.Parcel> SenderParcels = dal.ListParcelDisplay(i => i.Sender == ID).ToList();//the list of the parcels that the customer send
            IEnumerable<IDAL.DO.Parcel> ReceiverParcels = dal.ListParcelDisplay(i => i.Targetid == ID).ToList();//the list of the parcels that the customer received
            ParcelByCustomer sendParcel = new();

            foreach (IDAL.DO.Parcel currentParcel in SenderParcels)
            {
                currentParcel.CopyPropertiesTo(sendParcel);
                sendParcel.SecondSideOfParcelCustomer = new();
                sendParcel.SecondSideOfParcelCustomer.CustomerID = currentParcel.Targetid;//second side is targetid
                sendParcel.SecondSideOfParcelCustomer.Name = dal.CustomerDisplay(currentParcel.Targetid).Name;
                if (currentParcel.Scheduled == null)//not schedule yet
                    sendParcel.ParcelStatus = ParcelStatus.Defined;
                else if (currentParcel.PickUp == null)//scheduled but has not been picked up
                    sendParcel.ParcelStatus = ParcelStatus.Associated;
                else if (currentParcel.Delivered == null) //scheduled and picked up  but has not been delivered
                    sendParcel.ParcelStatus = ParcelStatus.PickedUp;
                else sendParcel.ParcelStatus = ParcelStatus.Delivered;
                //add the parcel to the list
                customerBO.FromCustomer = new();
                customerBO.FromCustomer.Add(sendParcel);
            }
            ParcelByCustomer receiveParcel = new();

            foreach (IDAL.DO.Parcel currentParcel in ReceiverParcels)
            {
                currentParcel.CopyPropertiesTo(receiveParcel);
                receiveParcel.SecondSideOfParcelCustomer = new();
                receiveParcel.SecondSideOfParcelCustomer.CustomerID = currentParcel.Sender;//second side is targetid
                receiveParcel.SecondSideOfParcelCustomer.Name = dal.CustomerDisplay(currentParcel.Sender).Name;
                receiveParcel.ParcelStatus = ParcelStatus.Delivered;//the status id delivered cause its by the targetid..
                //add the parcel to the list
                customerBO.TOCustomer = new();
                customerBO.TOCustomer.Add(receiveParcel);
            }

            return customerBO;
        }

        public IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null)
        {
            IEnumerable<IDAL.DO.Customer> customerDO = dal.ListCustomerDisplay();
            List<CustomerToList> customerToLists = new();
            foreach (IDAL.DO.Customer currentCustomer in customerDO)
            {
                CustomerToList tmpCustomerToList = new();
                currentCustomer.CopyPropertiesTo(tmpCustomerToList);
                //brings all the parcels that were send by the current customer and were delivered
                IEnumerable<IDAL.DO.Parcel> parcelsSendAndDelivered = dal.ListParcelDisplay(i => i.Sender == currentCustomer.Id && i.Delivered != null);
                tmpCustomerToList.NumberParcelSentAndDelivered = parcelsSendAndDelivered.Count();
                //brings all the parcels that were send by the current customer and werent delivered
                IEnumerable<IDAL.DO.Parcel> parcelsSendAndNOTDelivered = dal.ListParcelDisplay(i => i.Sender == currentCustomer.Id && i.Delivered == null && i.PickUp != null);
                tmpCustomerToList.NumberParcelSentAndNOTDelivered = parcelsSendAndNOTDelivered.Count();
                //brings all the parcels that were received by the current customer and were delivered
                IEnumerable<IDAL.DO.Parcel> parcelsReceived = dal.ListParcelDisplay(i => i.Targetid == currentCustomer.Id && i.Delivered != null);
                tmpCustomerToList.NumberOfParcelReceived = parcelsReceived.Count();
                //brings all the parcels that were received by the current customer and werent delivered
                IEnumerable<IDAL.DO.Parcel> parcelsOnTheWay = dal.ListParcelDisplay(i => i.Targetid == currentCustomer.Id && i.PickUp != null && i.Delivered == null);
                tmpCustomerToList.NumberOfParcelOnTheWayToCustomer = parcelsOnTheWay.Count();
                customerToLists.Add(tmpCustomerToList);
            }
            return customerToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}