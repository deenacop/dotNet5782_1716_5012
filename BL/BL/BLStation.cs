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
        public void AddBaseStation(BaseStation station)
        {
            if (CheckNumOfDigits(station.Id) != 4)
                throw new WrongIDException("Wrong ID");
            if (station.Location.Latitude <= 31 || station.Location.Latitude >= 32
                || station.Location.Longitude <= 35 || station.Location.Longitude >= 36)
                throw new UnlogicalLocationException("the location is not logical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new NegetiveException("Cant be negative");
            station.DronesInCharging.Clear();
            try
            {
                IDAL.DO.Station tmpStation = new();
                object obj = tmpStation;
                station.CopyPropertiesTo(obj);
                tmpStation = (IDAL.DO.Station)obj;
                tmpStation.Longitude = station.Location.Longitude;
                tmpStation.Latitude = station.Location.Latitude;
                dal.Add(tmpStation);
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }

        public void UpdateStation(BaseStation baseStation)
        {
            if (baseStation.Name == null || baseStation.Name == "")
                throw new WrongInputException("Missing drone model");

            BaseStationToList stationList = new();
            baseStation.CopyPropertiesTo(stationList);
            object obj = new IDAL.DO.Station();
            baseStation.CopyPropertiesTo(obj);
            IDAL.DO.Station tmp = (IDAL.DO.Station)obj;
            tmp.Latitude = baseStation.Location.Latitude;
            tmp.Longitude = baseStation.Location.Longitude;
            obj = tmp;

            try
            {
                dal.UpdateStation((IDAL.DO.Station)obj);//calls the function from DALOBJECT
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
                IDAL.DO.Station station = dal.GetStation(stationID);
                BaseStation baseStation = new();
                station.CopyPropertiesTo(baseStation);//we got the station details from DAL
                baseStation.Location = new()
                {
                    Longitude = station.Longitude,
                    Latitude = station.Latitude 
                };
                List<DroneInCharging> DroneChargingBL = new();
                IEnumerable<IDAL.DO.DroneCharge> DroneChargeingListDL = dal.GetListDroneCharge(i=>i.BaseStationID==stationID);//Receive the drone list from the data layer.
                DroneChargeingListDL.CopyPropertiesToIEnumerable(DroneChargingBL);//convret from IDAT to IBL

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
            IEnumerable<IDAL.DO.Station> stations = dal.GetListStation();
            List<BaseStationToList> stationToLists = new();
            foreach (IDAL.DO.Station currentStation in stations)
            {
                BaseStation tmp = new();
                BaseStationToList tmpToLst = new();
                tmp = GetBaseStation(currentStation.Id);
                tmp.CopyPropertiesTo(tmpToLst);
                tmpToLst.NumOfBusyChargingSlots = tmp.DronesInCharging.FindAll(i => i.FinishedRecharging == null).Count;
                stationToLists.Add(tmpToLst);
            }
            return stationToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}