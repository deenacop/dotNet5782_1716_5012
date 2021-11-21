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
        /// <summary>
        /// Adds a station to the list of stations in the IDAL
        /// </summary>
        /// <param name="station">The wanted station</param>
        public void AddBaseStation(BaseStation station)
        {
            if (ChackingNumOfDigits(station.StationID) != 4)
                throw new WrongIDException("Wrong ID");
            if (station.StationLocation.Latitude < 31 || station.StationLocation.Latitude > 32
                || station.StationLocation.Longitude < 35 || station.StationLocation.Longitude > 36)
                throw new UnlogicalLocationException("the location is not logical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new ArgumentOutOfRangeException("cant be negative");
            station.DronesInCharging.Clear();
            try
            {
                IDAL.DO.Station tmpStation = new();
                station.CopyPropertiesTo(tmpStation);
                dal.Add(tmpStation);
            }
            catch (Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }


        /// <summary>
        /// Display the base station
        /// </summary>
        /// <param name="StationID">The ID of the wanted station</param>
        /// <returns>Returns the wanted base station</returns>
        public BaseStation BaseStationDisplay(int StationID)
        {
            IDAL.DO.Station station = new();
            try
            {
                station = dal.StationDisplay(StationID);
            }
            catch(Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            BaseStation baseStation = new ();
            station.CopyPropertiesTo(baseStation);//we got the station details from DAL
            baseStation.StationLocation.Longitude = station.Longitude;
            baseStation.StationLocation.Latitude = station.Latitude;//set the location

            List<DroneInCharging> DroneChargingBL = null;
            List<IDAL.DO.DroneCharge> DroneChargeingListDL = dal.ListOfDroneCharge().ToList();//Receive the drone list from the data layer.
            DroneChargeingListDL.CopyPropertiesTo(DroneChargingBL);//convret from IDAT to IBL

            foreach (DroneInCharging currentDronCharge in DroneChargingBL)//running on all the drone charge of BL
            {
                foreach (DroneToList currentDrone in DroneListBL)//running on all the drones
                {
                    if (currentDronCharge.DroneID == currentDrone.DroneID && currentDronCharge.FinishedRecharging == DateTime.MinValue)
                    {
                        currentDronCharge.Battery = currentDrone.Battery;
                        break;
                    }
                }
            }
            baseStation.DronesInCharging = DroneChargingBL;
            return baseStation;
        }


        /// <summary>
        /// Updates the station 
        /// </summary>
        /// <param name="ID">The ID of the wanted station</param>
        /// <param name="name">The name of the wanted station</param>
        /// <param name="numOfSlots">The numOfSlots of the wanted station</param>
        public void UpdateStation(int ID, string name = null, int? numOfSlots = null)
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

        /// <summary>
        /// Displays the list of BL base station
        /// </summary>
        /// <returns>The list of BL base ststion</returns>
        public IEnumerable<BaseStationToList> ListBaseStationlDisplay()
        {
            List<IDAL.DO.Station> stations = dal.ListStationDisplay().ToList();
            List<BaseStationToList> stationToLists = new();
            foreach(IDAL.DO.Station currentStation in stations)
            {
                BaseStationToList tmpToLst = new();
                BaseStation tmp = BaseStationDisplay(currentStation.StationID);
                tmp.CopyPropertiesTo(tmpToLst);
                tmpToLst.NumOfBusyChargingSlots = tmp.DronesInCharging.FindAll(i=>i.FinishedRecharging==DateTime.MinValue).Count;
                stationToLists.Add(tmpToLst);
            }
            return stationToLists;
        }


        /// <summary>
        /// Displays the list of BL base station with available slots
        /// </summary>
        /// <returns>The list of BL base ststion with available slots</returns>
        public IEnumerable<BaseStationToList> ListOfAvailableSlotsBaseStationlDisplay()
        {
            List<IDAL.DO.Station> stations = dal.ListStationDisplay(i=>i.NumOfAvailableChargingSlots>0).ToList();
            List<BaseStationToList> stationToLists = new();
            foreach (IDAL.DO.Station currentStation in stations)
            {
                BaseStationToList tmpToLst = new();
                BaseStation tmp = BaseStationDisplay(currentStation.StationID);
                tmp.CopyPropertiesTo(tmpToLst);
                tmpToLst.NumOfBusyChargingSlots = tmp.DronesInCharging.FindAll(i => i.FinishedRecharging == DateTime.MinValue).Count;
                stationToLists.Add(tmpToLst);
            }
            return stationToLists;
        }




    }
}