using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        /// <summary>
        /// Add a  single drone 
        /// </summary>
        /// <param name="drone">The new drone that we asked to add</param>
        /// <param name="stationID">A station ID for a initial charge</param>
        public void AddDrone(Drone drone, int stationID)
        {
            if (ChackingNumOfDigits(drone.DroneID) != 3)
                throw new WrongIDException("worng ID");
            T wantedStation = FindBaseStation(stationID);
            if (wantedStation.StationID == 0)
                throw new AlreadyExistedItemException("The station for charging the drone, does not exist");
            drone.MyCurrentLocation = wantedStation.StationLocation;
            drone.Battery = rand.Next(20, 41);
            drone.DroneStatus = @enum.DroneStatus.Maintenance;
            try
            {
                IDAL.DO.Drone droneDO = new IDAL.DO.Drone();
                drone.CopyPropertiesTo(droneDO);
                dal.Add(droneDO);
            }
            catch (Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }

        public void SendDroneToCharge(int ID)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);//finds the drone with the wanted ID
            if (index == -1 || DroneListBL[index].DroneStatus != @enum.DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone does not exist or is not available");
          
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay(i=>i.NumOfAvailableChargeSlots>0).ToList();//Receive the drone list with available slots from the data layer.

            double minDistance = 0;
         
            //finds the closest station from the sender
            foreach (IDAL.DO.Station currentStation in StationListDL)
            {
                Location currrentStationLocation = new Location
                {
                    Latitude = currentStation.Latitude,
                    Longitude = currentStation.Longitude
                };
                double distance = DistanceCalculation(DroneListBL[index].MyCurrentLocation, currrentStationLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currrentStationLocation = currentStation.StationLocation;
                }
            }

        }
    }
}
