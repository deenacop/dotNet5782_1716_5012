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
            catch(Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }


        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        public BaseStation BaseStationDisplay(int StationID)
        {
            IDAL.DO.Station station = dal.StationDisplay(StationID);
            BaseStation baseStation = new BaseStation();
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
                    if(currentDronCharge.DroneID== currentDrone.DroneID)
                    {
                        currentDronCharge.Battery = currentDrone.Battery;
                        break;
                    }
                }
            }
            baseStation.DronesInCharging = DroneChargingBL;
            return baseStation;
        }

        public void UpdateStation(int ID, string name=null,int? numOfSlots=null)
        {
            try
            {
                dal.UpdateStation(ID, name, numOfSlots);
            }
            catch(Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }






    }
}