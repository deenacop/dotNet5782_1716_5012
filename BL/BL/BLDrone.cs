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
            BaseStation wantedStation = FindBaseStation(stationID);
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
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);
            if (index == -1 || DroneListBL[index].DroneStatus != @enum.DroneStatus.Available)
                throw new ItemNotExistException("Drone does not exist or is not available");

            List<BaseStation> BaseStationListBL = null;
            IEnumerable<IDAL.DO.Station> StationListDL = dal.ListStationDisplay();//Receive the drone list from the data layer.
            BaseStationListBL.CopyPropertiesTo(StationListDL);//convret from IDAT to IBL

            double minDistance = 0;
            Location closestStation = null;
            //finds the closest station from the sender
            foreach (BaseStation currentStation in BaseStationListBL)
            {
                double distance = DistanceCalculation(DroneListBL[index].MyCurrentLocation, currentStation.StationLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestStation = currentStation.StationLocation;
                }
            }

        }
    }
}
