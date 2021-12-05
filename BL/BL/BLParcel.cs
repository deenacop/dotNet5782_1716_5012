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
                dal.GetCustomer(parcel.SenderCustomer.Id);
                dal.GetCustomer(parcel.TargetidCustomer.Id);
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
                tmpParcel.Sender = parcel.SenderCustomer.Id;
                tmpParcel.Targetid = parcel.TargetidCustomer.Id;

                dal.Add(tmpParcel);
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public Parcel GetParcel(int ID)
        {
            IDAL.DO.Parcel parcelDO = new();
            Parcel parcelBO = new();
            parcelBO.SenderCustomer = new();
            parcelBO.TargetidCustomer = new();
            try
            {
                parcelDO = dal.GetParcel(ID);
                parcelDO.CopyPropertiesTo(parcelBO);
                parcelBO.SenderCustomer.Id = dal.GetCustomer(parcelDO.Sender).Id;
                parcelBO.MyDrone = new();
                parcelBO.SenderCustomer.Name = dal.GetCustomer(parcelDO.Sender).Name;
                parcelBO.TargetidCustomer.Id = dal.GetCustomer(parcelDO.Targetid).Id;
                parcelBO.TargetidCustomer.Name = dal.GetCustomer(parcelDO.Targetid).Name;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            ///האם קיים הdrone?
            if (parcelDO.Scheduled != null && parcelDO.Delivered==null)//if the parel is assigned
            {
                DroneToList drone = DroneListBL.Find(i => i.Id == parcelDO.MyDroneID);
                parcelBO.MyDrone = new();
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }

        public IEnumerable<ParcelToList> GetListParcel(Predicate<ParcelToList> predicate = null)
        {
            IEnumerable<IDAL.DO.Parcel> parcelsDO = dal.GetListParcel();
            List<ParcelToList> listParcelToList = new();
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                ParcelToList tmpParcelBO = new();
                Parcel tmp = GetParcel(currentParcel.Id);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.SenderCustomer.Name;
                tmpParcelBO.NameOfTargetaed = tmp.TargetidCustomer.Name;
                if (tmp.Scheduled == null)//not schedule yet
                    tmpParcelBO.Status = ParcelStatus.Defined;
                else if (tmp.PickUp == null)//scheduled but has not been picked up
                    tmpParcelBO.Status = ParcelStatus.Associated;
                else if (tmp.Delivered == null) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.Status = ParcelStatus.PickedUp;
                else tmpParcelBO.Status = ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}

  