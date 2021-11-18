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
        /// <summary>
        /// The function send a specific drone to the closest available station
        /// </summary>
        /// <param name="ID">drone ID</param>
        public void SendDroneToCharge(int ID)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);//finds the drone with the wanted ID
            if (index == -1 || DroneListBL[index].DroneStatus != @enum.DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone does not exist or is not available");

            List<BaseStation> BaseStationListBL = null;
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay(i => i.NumOfAvailableChargeSlots > 0).ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesTo(BaseStationListBL);//convret from IDAT to IBL
            if (BaseStationListBL == null)
                throw new ItemNotExistException("There is no stations with available slots");
            //finds the closest station from the drone
            double distance = MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item2;
            double[] ElectricityUse = dal.ChargingDrone();//brings the the amount of battery that is use when the drone is available
            double vacant = ElectricityUse[0];
            if (distance * vacant > DroneListBL[index].Battery)
                throw new NotEnoughBatteryException("The drone cant go to a baseStation");
            try
            {
                dal.SendingDroneToChargingBaseStation(ID, MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item3);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //if the drone *can* go to a charging station:
            DroneToList tmp = DroneListBL[index];
            tmp.Battery -= (int)(distance * vacant);
            tmp.DroneStatus = @enum.DroneStatus.Maintenance;
            tmp.MyCurrentLocation = MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item1;
            DroneListBL[index] = tmp;
        }

        /// <summary>
        /// This function is relaesing a drone
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="minuteInCharge">the amount of time(by minute) that the drone was in charge</param>
        public void ReleasingDroneFromBaseStation(int ID, int minuteInCharge)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);
            if (index == -1)//NOT FOUND
                throw new ItemNotExistException("The drone does not exist");
            if (DroneListBL[index].Battery <= 100)
                throw new NotEnoughBatteryException("The drone needs to be charged");

            List<BaseStation> BaseStationListBL = null;
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay().ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesTo(BaseStationListBL);//convret from IDAT to IBL
            //if drone *can* be released
            try
            {
                dal.ReleasingDroneFromChargingBaseStation(ID,  BaseStationListBL.Find(item=>item.StationLocation== DroneListBL[index].MyCurrentLocation).StationID);
            }
            catch(Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            double[] ElectricityUse = dal.ChargingDrone();//*צריך לבדוק מה הוא מעתיק
            double droneChargingRate = ElectricityUse[4];//Rate of charge per minute
            DroneToList tmp = DroneListBL[index];
            tmp.Battery += (int)(minuteInCharge * droneChargingRate);
            tmp.DroneStatus = @enum.DroneStatus.Available;
            DroneListBL[index] = tmp;
        }

        public void AssignParcelToDrone(int ID)
        {
            if (DroneListBL.Exists(item => item.DroneID == ID))//NOT FOUND
                throw new ItemNotExistException("The drone does not exist");
            DroneToList currentDrone= DroneListBL.Find(item => item.DroneID == ID);
            if(currentDrone.DroneStatus!=@enum.DroneStatus.Available)
                throw new NotAvailableException("The drone is not available");
            List<Parcel> ParcelListBL = null;
            //Receive the parcel list of parcels that are assign to drone (from the data layer).
            List<IDAL.DO.Parcel> ParcelListDL = dal.ListParcelDisplay(i => i.MyDroneID != 0).ToList();
            ParcelListDL.CopyPropertiesTo(ParcelListBL);//convret from IDAT to IBL
        }
    }
}
