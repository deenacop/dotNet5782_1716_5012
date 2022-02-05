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
        public void AddBaseStation(BaseStation station)
        {
            if (CheckNumOfDigits(station.Id) != 4)
                throw new WrongIDException("Bad station ID");
            if (station.Location.Latitude <= 31 || station.Location.Latitude >= 32
                || station.Location.Longitude <= 35 || station.Location.Longitude >= 36)//Checking that the location is in the allowed range (Jerusalem area)
                throw new UnlogicalLocationException("the location is unlogical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new NegetiveException("There may not be a number of negative charging positions");
            station.DronesInCharging = new List<DroneInCharging>();
            lock (dal)
            {
                try
                {
                    DO.Station tmpStation = new();
                    object obj = tmpStation;//Boxing and unBoxing
                    station.CopyPropertiesTo(obj);

                    tmpStation = (DO.Station)obj;
                    tmpStation.Longitude = station.Location.Longitude;
                    tmpStation.Latitude = station.Location.Latitude;
                    if (!dal.Add(tmpStation))//calls the function from DALOBJECT
                        throw new AskRecoverExeption($"The station has been deleted. Are you sure you want to recover? ");
                }
                catch (ItemAlreadyExistsException ex)
                {
                    throw new ItemAlreadyExistsException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StationRecover(BaseStation station)
        {
            lock (dal)
            {
                try
                {
                    BaseStation deletedStation = GetBaseStation(station.Id);
                    station.DronesInCharging = deletedStation.DronesInCharging;
                    DO.Station stationlDO = new();
                    object obj = stationlDO;//boxing and unBoxing
                    station.CopyPropertiesTo(obj);
                    stationlDO = (DO.Station)obj;
                    stationlDO.Longitude = station.Location.Longitude;
                    stationlDO.Latitude = station.Location.Latitude;
                    dal.UpdateStation(stationlDO); //calls the function from DALOBJECT
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(BaseStation baseStation)
        {
            lock (dal)
            {
                try
                {
                    DO.Station stationlDO = new();
                    object obj = stationlDO;//boxing and unBoxing
                    baseStation.CopyPropertiesTo(obj);
                    stationlDO = (DO.Station)obj;
                    //needs to be initialize by hand
                    stationlDO.Latitude = baseStation.Location.Latitude;
                    stationlDO.Longitude = baseStation.Location.Longitude;
                    BaseStationToList stationList = new();
                    baseStation.CopyPropertiesTo(stationList);
                    BaseStation tempS = GetBaseStation(stationList.Id);
                    stationList.NumOfBusyChargingSlots = tempS.DronesInCharging.Count();
                    if (stationList.NumOfBusyChargingSlots != 0)
                        throw new ItemCouldNotBeRemoved("The station could not be removed");
                    dal.RemoveStation(stationlDO.Id);//calls the function from DALOBJECT
                }
                catch (ItemCouldNotBeRemoved ex)
                {
                    throw new ItemCouldNotBeRemoved(ex.Message);
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(BaseStation baseStation)
        {
            if (baseStation.Name == null || baseStation.Name == "")
                throw new WrongInputException("Missing station name");
            BaseStationToList stationList = new();
            baseStation.CopyPropertiesTo(stationList);
            object obj = new DO.Station();//Boxing and unBoxing
            baseStation.CopyPropertiesTo(obj);
            DO.Station tmp = (DO.Station)obj;
            //needs to be initialize by hand
            tmp.Latitude = baseStation.Location.Latitude;
            tmp.Longitude = baseStation.Location.Longitude;
            obj = tmp;
            lock (dal)
            {
                try
                {
                    dal.UpdateStation((DO.Station)obj);//calls the function from DALOBJECT
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int stationID)
        {
            lock (dal)
            {
                try
                {
                    DO.Station station = dal.GetStation(stationID);
                    BaseStation baseStation = new();
                    station.CopyPropertiesTo(baseStation);
                    //Set the location by hand
                    baseStation.Location = new()
                    {
                        Longitude = station.Longitude,
                        Latitude = station.Latitude
                    };
                    baseStation.DronesInCharging = from item in dal.GetListDroneCharge(s => s.BaseStationID == stationID)
                                                   let tempD = DronesBL.FirstOrDefault(i => i.Id == item.Id)
                                                   select item.CopyPropertiesTo(new DroneInCharging()
                                                   {
                                                       Battery = tempD.Battery
                                                   });
                    return baseStation;
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null)
        {
            lock (dal)
            {
                List<BaseStationToList> stationToLists = new();
                stationToLists.AddRange(from s in dal.GetListStation(i => !i.IsRemoved)
                                        let tempS = GetBaseStation(s.Id)
                                        select tempS.CopyPropertiesTo(new BaseStationToList()
                                        {
                                            NumOfBusyChargingSlots = tempS.DronesInCharging.Count()//the amount of drones
                                                                                                   //in the station
                                        })
                                        );
                return stationToLists.FindAll(i => predicate == null ? true : predicate(i));
            }
        }
    }
}