using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddParcel(Parcel parcel)
        {
            if (ChackingNumOfDigits(parcel.SenderCustomer.CustomerID) != 9 || dal.ListCustomerDisplay(i => i.CustomerID == parcel.SenderCustomer.CustomerID) == null)
                throw new WrongIDException("Wrong ID");
            if (ChackingNumOfDigits(parcel.TargetidCustomer.CustomerID) != 9 || dal.ListCustomerDisplay(i => i.CustomerID == parcel.TargetidCustomer.CustomerID) == null)
                throw new WrongIDException("Wrong ID");
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.PickUp = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            parcel.MyDrone = new();
            try
            {
                IDAL.DO.Parcel tmpParcel = new();
                object obj = tmpParcel;
                parcel.CopyPropertiesTo(obj);
                tmpParcel = (IDAL.DO.Parcel)obj;
                tmpParcel.Sender = parcel.SenderCustomer.CustomerID;
                tmpParcel.Targetid = parcel.TargetidCustomer.CustomerID;

                dal.Add(tmpParcel);
            }
            catch (Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }

        public Parcel ParcelDisplay(int ID)
        {
            IDAL.DO.Parcel parcelDO = new();
            Parcel parcelBO = new();
            parcelBO.SenderCustomer = new();
            parcelBO.TargetidCustomer = new();
            try
            {
                parcelDO = dal.ParcelDisplay(ID);
                parcelDO.CopyPropertiesTo(parcelBO);
                parcelBO.SenderCustomer.CustomerID = dal.CustomerDisplay(parcelDO.Sender).CustomerID;
                parcelBO.MyDrone = new();
                parcelBO.SenderCustomer.Name = dal.CustomerDisplay(parcelDO.Sender).Name;
                parcelBO.TargetidCustomer.CustomerID = dal.CustomerDisplay(parcelDO.Targetid).CustomerID;
                parcelBO.TargetidCustomer.Name = dal.CustomerDisplay(parcelDO.Targetid).Name;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            ///האם קיים הdrone?
            if (parcelDO.Scheduled != DateTime.MinValue)//if the parel is assigned
            {
                DroneToList drone = DroneListBL.Find(i => i.DroneID == parcelDO.MyDroneID);
                parcelBO.MyDrone = new();
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }

        public IEnumerable<ParcelToList> ListParcelDisplay()
        {
            List<IDAL.DO.Parcel> parcelsDO = new();
            try
            {
                parcelsDO = dal.ListParcelDisplay().ToList();
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }           
            List<ParcelToList> listParcelToList = new();
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                ParcelToList tmpParcelBO = new();
                Parcel tmp = ParcelDisplay(currentParcel.ParcelID);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.SenderCustomer.Name;
                tmpParcelBO.NameOfTargetaed = tmp.TargetidCustomer.Name;
                if (tmp.Scheduled == DateTime.MinValue)//not schedule yet
                    tmpParcelBO.ParcelStatus = ParcelStatus.Defined;
                else if (tmp.PickUp == DateTime.MinValue)//scheduled but has not been picked up
                    tmpParcelBO.ParcelStatus = ParcelStatus.Associated;
                else if (tmp.Delivered == DateTime.MinValue) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.ParcelStatus = ParcelStatus.PickedUp;
                else tmpParcelBO.ParcelStatus = ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList;
        }

        public IEnumerable<ParcelToList> ListOfUnassignedParcelDisplay()
        {
            List<IDAL.DO.Parcel> parcelsDO = new();
            try
            {
                parcelsDO = dal.ListParcelDisplay(i => i.MyDroneID == 0).ToList();
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            Parcel tmp = new();

            List<ParcelToList> listParcelToList = new();
            List<IDAL.DO.Customer> CustomerListDL = dal.ListCustomerDisplay().ToList();//Receive the customer list from the data layer.
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                ParcelToList tmpParcelBO = new();
                tmp = ParcelDisplay(currentParcel.ParcelID);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.SenderCustomer.Name;
                tmpParcelBO.NameOfTargetaed = tmp.TargetidCustomer.Name;
                if (tmp.Scheduled == DateTime.MinValue)//not schedule yet
                    tmpParcelBO.ParcelStatus = ParcelStatus.Defined;
                else if (tmp.PickUp == DateTime.MinValue)//scheduled but has not been picked up
                    tmpParcelBO.ParcelStatus = ParcelStatus.Associated;
                else if (tmp.Delivered == DateTime.MinValue) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.ParcelStatus = ParcelStatus.PickedUp;
                else tmpParcelBO.ParcelStatus = ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList;
        }
    }
}