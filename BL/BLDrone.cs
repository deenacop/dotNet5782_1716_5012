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
            catch (IDAL.DO.AlreadyExistedItemException ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }
        /// <summary>
        /// The function updates the model of a specific drone
        /// </summary>
        /// <param name="ID">Drone ID</param>
        /// <param name="model">Drone model</param>
        public void UpdateDroneName(int ID, string model)
        {
            try
            {
                dal.UpdateDroneName( ID,  model);//sends to IDAL
            }
            catch(ItemNotExistException ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }
    }
}
