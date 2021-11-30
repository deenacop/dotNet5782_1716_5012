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
            try
            {
                dal.CustomerDisplay(parcel.SenderCustomer.CustomerID);
                dal.CustomerDisplay(parcel.TargetidCustomer.CustomerID);
            }
            catch (Exception)
            {
                throw new WrongIDException("Wrong ID");
            }
            if (parcel.Weight < WeightCategories.Light || parcel.Weight > WeightCategories.Heavy)
                throw new WrongInputException("Wrong input");
            if (parcel.Priority < Priorities.Normal || parcel.Priority > Priorities.Urgent)
                throw new WrongInputException("Wrong input");
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = null;
            parcel.PickUp = null;
            parcel.Delivered = null;
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
            if (parcelDO.Scheduled != null && parcelDO.Delivered==null)//if the parel is assigned
            {
                DroneToList drone = DroneListBL.Find(i => i.DroneID == parcelDO.MyDroneID);
                parcelBO.MyDrone = new();
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }

        public IEnumerable<ParcelToList> ListParcelDisplay(Predicate<ParcelToList> predicate = null)
        {
            IEnumerable<IDAL.DO.Parcel> parcelsDO = dal.ListParcelDisplay();
            List<ParcelToList> listParcelToList = new();
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                ParcelToList tmpParcelBO = new();
                Parcel tmp = ParcelDisplay(currentParcel.ParcelID);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.SenderCustomer.Name;
                tmpParcelBO.NameOfTargetaed = tmp.TargetidCustomer.Name;
                if (tmp.Scheduled == null)//not schedule yet
                    tmpParcelBO.ParcelStatus = ParcelStatus.Defined;
                else if (tmp.PickUp == null)//scheduled but has not been picked up
                    tmpParcelBO.ParcelStatus = ParcelStatus.Associated;
                else if (tmp.Delivered == null) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.ParcelStatus = ParcelStatus.PickedUp;
                else tmpParcelBO.ParcelStatus = ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}

  