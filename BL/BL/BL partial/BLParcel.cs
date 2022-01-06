using BO;
using System;
using System.Collections.Generic;
using DalApi;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        public void AddParcel(Parcel parcel)
        {
            try
            {
                parcel.SenderCustomer.Name = dal.GetCustomer(parcel.SenderCustomer.Id).Name;
                parcel.TargetidCustomer.Name=dal.GetCustomer(parcel.TargetidCustomer.Id).Name;
            }
            catch (Exception)
            {
                throw new WrongIDException("Bad custumers ID");
            }
            if (parcel.Weight < WeightCategories.Light || parcel.Weight > WeightCategories.Heavy)
                throw new WrongInputException("Wrong input");
            if (parcel.Priority < Priorities.Normal || parcel.Priority > Priorities.Urgent)
                throw new WrongInputException("Wrong input");
            parcel.Requested = DateTime.Now;
            parcel.MyDrone = new();
            
            try
            {
                DO.Parcel tmpParcel = new();
                object obj = tmpParcel;//Boxing and unBoxing
                parcel.CopyPropertiesTo(obj);
                tmpParcel = (DO.Parcel)obj;
                tmpParcel.Sender = parcel.SenderCustomer.Id;
                tmpParcel.Targetid = parcel.TargetidCustomer.Id;
                int id=dal.Add(tmpParcel);
                parcel.Id = id;
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public void RemoveParcel(Parcel parcel)
        {
            try
            {
                DO.Parcel parcelDO = new();
                object obj = parcelDO;//boxing and unBoxing
                parcel.CopyPropertiesTo(obj);
                parcelDO = (DO.Parcel)obj;
                if(parcel.Scheduled==null)
                    dal.Remove(parcelDO);
                else
                    throw new ItemCouldNotBeRemoved("The parcel has been associated already!");
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public Parcel GetParcel(int ID)
        {
            DO.Parcel parcelDO = new();
            Parcel parcelBO = new();
            parcelBO.SenderCustomer = new();
            parcelBO.TargetidCustomer = new();
            try
            {
                parcelDO = dal.GetParcel(ID);
                parcelDO.CopyPropertiesTo(parcelBO);
                //set the properties that needed to be set by hand
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
            ///In case there is a drone:
            if (parcelDO.Scheduled != null && parcelDO.Delivered==null)//if the parel is assigned and there is drone to update
            {
                DroneToList drone = DroneListBL.Find(i => i.Id == parcelDO.MyDroneID);
                parcelBO.MyDrone = new();
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }

        public IEnumerable<ParcelToList> GetListParcel(Predicate<ParcelToList> predicate = null)
        {
            IEnumerable<DO.Parcel> parcelsDO = dal.GetListParcel(i=>!i.IsRemoved);
            List<ParcelToList> listParcelToList = new();
            foreach (DO.Parcel currentParcel in parcelsDO)
            {
                ParcelToList tmpParcelBO = new();
                Parcel tmp = GetParcel(currentParcel.Id);
                tmp.CopyPropertiesTo(tmpParcelBO);
                //Set the properties that needed to be set by hand
                tmpParcelBO.NameOfSender = tmp.SenderCustomer.Name;
                tmpParcelBO.NameOfTargetid = tmp.TargetidCustomer.Name;
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

  