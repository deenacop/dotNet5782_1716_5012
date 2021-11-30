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
            if (ChackingNumOfDigits(station.StationID) != 4)
                throw new WrongIDException("Wrong ID");
            if (station.StationLocation.Latitude <= 31 || station.StationLocation.Latitude >= 32
                || station.StationLocation.Longitude <= 35 || station.StationLocation.Longitude >= 36)
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
                tmpStation.Longitude = station.StationLocation.Longitude;
                tmpStation.Latitude = station.StationLocation.Latitude;
                dal.Add(tmpStation);
            }
            catch (Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }


        public BaseStation BaseStationDisplay(int StationID)
        {
            try
            {
                IDAL.DO.Station station = dal.StationDisplay(StationID);
                BaseStation baseStation = new();
                station.CopyPropertiesTo(baseStation);//we got the station details from DAL
                baseStation.StationLocation = new()
                {
                    Longitude = station.Longitude,
                    Latitude = station.Latitude
                };
                List<DroneInCharging> DroneChargingBL = new();
                IEnumerable<IDAL.DO.DroneCharge> DroneChargeingListDL = dal.ListOfDroneCharge();//Receive the drone list from the data layer.
                DroneChargeingListDL.CopyPropertiesToIEnumerable(DroneChargingBL);//convret from IDAT to IBL

                foreach (DroneInCharging currentDronCharge in DroneChargingBL)//running on all the drone charge of BL
                {
                    foreach (DroneToList currentDrone in DroneListBL)//running on all the drones
                    {
                        if (currentDronCharge.DroneID == currentDrone.DroneID && currentDronCharge.FinishedRecharging == null)
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


        public void UpdateStation(int ID, string name, int numOfSlots)
        {
            try
            {
                dal.UpdateStation(ID, name, numOfSlots);//calls the function from the dalObject
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public IEnumerable<BaseStationToList> ListBaseStationDisplay(Predicate<BaseStationToList> predicate = null)
        {
            IEnumerable<IDAL.DO.Station> stations = dal.ListStationDisplay();
            List<BaseStationToList> stationToLists = new();
            foreach (IDAL.DO.Station currentStation in stations)
            {
                BaseStation tmp = new();
                BaseStationToList tmpToLst = new();
                tmp = BaseStationDisplay(currentStation.StationID);
                tmp.CopyPropertiesTo(tmpToLst);
                tmpToLst.NumOfBusyChargingSlots = tmp.DronesInCharging.FindAll(i => i.FinishedRecharging == null).Count;
                stationToLists.Add(tmpToLst);
            }
            return stationToLists.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}