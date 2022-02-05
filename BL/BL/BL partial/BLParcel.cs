using BO;
using System;
using System.Collections.Generic;
using DalApi;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            lock (dal)
            {
                try
                {
                    parcel.SenderCustomer.Name = dal.GetCustomer(parcel.SenderCustomer.Id).Name;
                    parcel.TargetidCustomer.Name = dal.GetCustomer(parcel.TargetidCustomer.Id).Name;
                }
                catch (ItemNotExistException)
                {
                    throw new ItemNotExistException("customer does not exist");
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
                    int id = dal.Add(tmpParcel);//the function returns the parcels id to update also the BL list of parcels
                    parcel.Id = id;
                }
                catch (ItemAlreadyExistsException ex)
                {
                    throw new ItemAlreadyExistsException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(Parcel parcel)
        {
            lock (dal)
            {
                try
                {
                    DO.Parcel parcelDO = new();
                    object obj = parcelDO;//boxing and unBoxing
                    parcel.CopyPropertiesTo(obj);
                    parcelDO = (DO.Parcel)obj;
                    if (parcel.Scheduled == null)//if the parcel was associated you cant remove him
                        dal.RemoveParcel(parcelDO.Id);
                    else
                        throw new ItemCouldNotBeRemoved("The parcel has been associated already!");
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
                catch (ItemCouldNotBeRemoved ex)
                {
                    throw new ItemCouldNotBeRemoved(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int ID)
        {
            lock (dal)
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
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
                ///In case there is a drone:
                if (parcelDO.Scheduled != null && parcelDO.Delivered == null)//if the parel is assigned and there is drone to update
                {
                    DroneToList drone = DronesBL.Find(i => i.Id == parcelDO.MyDroneID);
                    parcelBO.MyDrone = new();
                    drone.CopyPropertiesTo(parcelBO.MyDrone);
                }
                return parcelBO;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetListParcel(Predicate<ParcelToList> predicate = null)
        {
            lock (dal)
            {
                IEnumerable<ParcelToList> listParcelToList = from p in dal.GetListParcel(i => !i.IsRemoved)
                                                             select p.CopyPropertiesTo(new ParcelToList
                                                             {
                                                                 Status = p.Scheduled == null ?//not schedule yet
                                                                          ParcelStatus.Defined :
                                                                          (p.PickUp == null ?//scheduled but has not been picked up
                                                                           ParcelStatus.Associated :
                                                                           (p.Delivered == null ? //scheduled and picked up  but has not been delivered
                                                                           ParcelStatus.PickedUp :
                                                                           ParcelStatus.Delivered)),
                                                                 NameOfTargetid = dal.GetCustomer(p.Targetid).Name,
                                                                 NameOfSender = dal.GetCustomer(p.Sender).Name
                                                             });
                return listParcelToList.Where(i => predicate == null ? true : predicate(i));
            }
        }
    }
}

