using BO;
using System;
using System.Collections.Generic;
using DalApi;


namespace BL
{
    internal partial class BL: BlApi.IBL
    {
        public void AddBaseStation(BaseStation station)
        {
            if (CheckNumOfDigits(station.Id) != 4)
                throw new WrongIDException("Bad station ID");
            if (station.Location.Latitude <= 31 || station.Location.Latitude >= 32
                || station.Location.Longitude <= 35 || station.Location.Longitude >= 36)//Checking that the location is in the allowed range (Jerusalem area)
                throw new UnlogicalLocationException("the location is unlogical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new NegetiveException("There may not be a number of negative charging positions");
            try
            {
                DO.Station tmpStation = new();
                object obj = tmpStation;//Boxing and unBoxing
                station.CopyPropertiesTo(obj);
                tmpStation = (DO.Station)obj;
                tmpStation.Longitude = station.Location.Longitude;
                tmpStation.Latitude = station.Location.Latitude;
                dal.Add(tmpStation);
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public void RemoveStation(BaseStation baseStation)
        {
            try
            {
                DO.Station stationlDO = new();
                object obj = stationlDO;//boxing and unBoxing
                baseStation.CopyPropertiesTo(obj);
                stationlDO = (DO.Station)obj;
                stationlDO.Latitude = baseStation.Location.Latitude;
                stationlDO.Longitude = baseStation.Location.Longitude;
                dal.Remove(stationlDO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public void UpdateStation(BaseStation baseStation)
        {
            if (baseStation.Name == null || baseStation.Name == "")
                throw new WrongInputException("Missing station model");
            BaseStationToList stationList = new();
            baseStation.CopyPropertiesTo(stationList);
            object obj = new DO.Station();//Boxing and unBoxing
            baseStation.CopyPropertiesTo(obj);
            DO.Station tmp = (DO.Station)obj;
            tmp.Latitude = baseStation.Location.Latitude;
            tmp.Longitude = baseStation.Location.Longitude;
            obj = tmp;

            try
            {
                dal.UpdateStation((DO.Station)obj);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public BaseStation GetBaseStation(int stationID)
        {
            try
            {
                DO.Station station = dal.GetStation(stationID);
                BaseStation baseStation = new();
                station.CopyPropertiesTo(baseStation);
                //Set the location
                baseStation.Location = new()
                {
                    Longitude = station.Longitude,
                    Latitude = station.Latitude 
                };
                //baseStation.DronesInCharging = from item in dal.GetListDroneCharge(i => i.BaseStationID == stationID).
                //                               let temp = DroneListBL.Find(i => i.Id == item.Id);
                List<DroneInCharging> DroneChargingBL = new();
                IEnumerable<DO.DroneCharge> DroneChargeingListDL = dal.GetListDroneCharge(i=>i.BaseStationID==stationID);//Receive the drone list from the data layer.
                DroneChargeingListDL.CopyPropertiesToIEnumerable(DroneChargingBL);//convret from DalApi to BL

                foreach (DroneInCharging currentDronCharge in DroneChargingBL)//running on all the drone charge of BL
                {
                    foreach (DroneToList currentDrone in DroneListBL)//running on all the drones
                    {
                        if (currentDronCharge.Id == currentDrone.Id && currentDronCharge.FinishedRecharging == null)
                        {
                            currentDronCharge.Battery = currentDrone.Battery;
                            break;
                        }
                    }
                }
                baseStation.DronesInCharging = DroneChargingBL;
                return baseStation;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null)
        {
            IEnumerable<DO.Station> stations = dal.GetListStation(i=>!i.IsRemoved);
            List<BaseStationToList> stationToLists = new();
            foreach (DO.Station currentStation in stations)
            {
                BaseStation tmp = new();
                BaseStationToList tmpToLst = new();
                tmp = GetBaseStation(currentStation.Id);
                tmp.CopyPropertiesTo(tmpToLst);
                //find the amount of *busy* slots
                tmpToLst.NumOfBusyChargingSlots = tmp.DronesInCharging.FindAll(i => i.FinishedRecharging == null).Count;//if "FinishedRecharging" is NULL this means that the drone's charge hasnt ended and the position is still occupied 
                stationToLists.Add(tmpToLst);
            }
            return stationToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}