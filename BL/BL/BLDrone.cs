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
        /// Adds a drone to the list of drones in the IDAL
        /// </summary>
        /// <param name="drone">The new drone that we asked to add</param>
        /// <param name="stationID">A station ID for a initial charge</param>
        public void AddDrone(Drone drone, int stationID)
        {
            if (ChackingNumOfDigits(drone.DroneID) != 3)
                throw new WrongIDException("worng ID");
            IDAL.DO.Station wantedStation = new();
            try
            {
                wantedStation = dal.StationDisplay(stationID);
                drone.MyCurrentLocation.Longitude = wantedStation.Longitude;
                drone.MyCurrentLocation.Latitude = wantedStation.Latitude;
                drone.Battery = rand.Next(20, 41);
                drone.DroneStatus = @enum.DroneStatus.Maintenance;
                IDAL.DO.Drone droneDO = new();
                object obj = droneDO;
                drone.CopyPropertiesTo(obj);
                droneDO = (IDAL.DO.Drone)obj;
                dal.Add(droneDO);//calls the function from DALOBJECT
            }
            catch (ItemNotExistException ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            catch (AlreadyExistedItemException ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }

        /// <summary>
        /// Update the drone model
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="model">new midel</param>
        public void UpdateDroneModel(int ID, string model)
        {
            try
            {
                dal.UpdateDroneModel(ID, model);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }


        /// <summary>
        /// The function send a specific drone to the closest available station
        /// </summary>
        /// <param name="ID">drone ID</param>
        public void SendDroneToCharge(int ID)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);//finds the drone with the wanted ID
            if (index < 0 || DroneListBL[index].DroneStatus != @enum.DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone does not exist or is not available");

            List<BaseStation> BaseStationListBL = new();
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay(i => i.NumOfAvailableChargingSlots > 0).ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesTo(BaseStationListBL);//convret from IDAT to IBL
            if (BaseStationListBL == null)
                throw new ItemNotExistException("There is no stations with available slots");
            //finds the closest station from the drone
            double distance = MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item2;
            if (distance * vacant > DroneListBL[index].Battery)
                throw new NotEnoughBatteryException("The drone cant go to a baseStation");
            try
            {
                dal.SendingDroneToChargingBaseStation(
                    ID, MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item3);//calls the function from DALOBJECT
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
            if (index < 0)//NOT FOUND
                throw new ItemNotExistException("The drone does not exist");
            if (DroneListBL[index].Battery + (int)(minuteInCharge * droneLoadingRate) <= 50)//the drone is half charged and can be used
                throw new NotEnoughBatteryException("The drone needs to be charged");

            List<BaseStation> BaseStationListBL = null;
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay().ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesTo(BaseStationListBL);//convret from IDAT to IBL
            //if drone *can* be released
            try
            {
                dal.ReleasingDroneFromChargingBaseStation(
                    ID, BaseStationListBL.Find(item => item.StationLocation == DroneListBL[index].MyCurrentLocation).StationID);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            DroneToList tmp = DroneListBL[index];
            tmp.Battery += (int)(minuteInCharge * droneLoadingRate);
            if (tmp.Battery > 100)
                tmp.Battery = 100;
            tmp.DroneStatus = @enum.DroneStatus.Available;
            DroneListBL[index] = tmp;
        }


        /// <summary>
        /// Assings a drone to a parcel
        /// </summary>
        /// <param name="ID">The drone to assign</param>
        public void AssignParcelToDrone(int ID)
        {
            Drone drone = DisplayDrone(ID);
            if (drone.DroneStatus != @enum.DroneStatus.Available)
                throw new WorngStatusException("The drone is not available");
            List<IDAL.DO.Parcel> parcels = dal.ListParcelDisplay().ToList();
            IDAL.DO.Parcel bestParcel = parcels.First();// we assume that the best parcel for the drone is the first parcel in the list
            foreach (IDAL.DO.Parcel currentParcel in parcels)
            {
                //checks if the parcel could be assigned
                if (!legalParcel(bestParcel, drone) || !batteryCheckingForDroneAndParcel(bestParcel, drone) || (bestParcel.Scheduled == DateTime.MinValue && bestParcel.Requested != DateTime.MinValue))
                {
                    bestParcel = currentParcel;
                    break;
                }
                //checks if the parcel could be assigned
                if (legalParcel(currentParcel, drone) && batteryCheckingForDroneAndParcel(currentParcel, drone) && (bestParcel.Scheduled == DateTime.MinValue && bestParcel.Requested != DateTime.MinValue))
                {
                    if (
                    (currentParcel.Priority > bestParcel.Priority) ||/*1: checking if the priority is bigger*/
                    ((currentParcel.Priority == bestParcel.Priority) && (currentParcel.Weight > bestParcel.Weight)) ||/*2: priority is the same and the weight is heavier*/
                    ((currentParcel.Priority == bestParcel.Priority) &&/*3: checking if the weight and the priority is the same and the distance is smaller*/
                    (currentParcel.Weight == bestParcel.Weight) &&
                    (DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(bestParcel.Sender).CustomerLocation) > DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(currentParcel.Sender).CustomerLocation)))/*checks if the distance is smaller and and the priority and weight are the same*/
                    )
                    {
                        bestParcel = currentParcel;
                    }
                }
            }
            //if there isnt a legal parcel to assign
            if (!legalParcel(bestParcel, drone) || !batteryCheckingForDroneAndParcel(bestParcel, drone) || !(bestParcel.Scheduled == DateTime.MinValue && bestParcel.Requested != DateTime.MinValue))
                throw new ItemNotExistException("There is no parcel to assign with the drone");
            drone.DroneStatus = @enum.DroneStatus.Delivery;
            dal.AssignParcelToDrone(bestParcel.ParcelID, drone.DroneID);
        }

        /// <summary>
        /// Update a drone to collect a parcel
        /// </summary>
        /// <param name="ID">drone ID</param>
        public void CollectionOfParcelByDrone(int ID)
        {
            Drone drone = DisplayDrone(ID);
            if (drone.DroneStatus != @enum.DroneStatus.Delivery)
                throw new WorngStatusException("The drone is not in delivery mode");
            Parcel parcel = ParcelDisplay(drone.MyParcel.ParcelID);
            if (parcel.PickUp != DateTime.MinValue)
                throw new WorngStatusException("The parcel has allready been picked up ");
            double distance = DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(parcel.SenderCustomer.CustomerID).CustomerLocation);
            drone.Battery -= (int)distance * (int)vacant;
            drone.MyCurrentLocation = CustomerDisplay(parcel.SenderCustomer.CustomerID).CustomerLocation;
            dal.CollectionOfParcelByDrone(parcel.ParcelID, drone.DroneID);
        }


        /// <summary>
        /// Delivers a parcel to the receiver
        /// </summary>
        /// <param name="ID">drone ID</param>
        public void DeliveryParcelByDrone(int ID)
        {
            Drone drone = DisplayDrone(ID);
            Parcel parcel = ParcelDisplay(drone.MyParcel.ParcelID);
            if (drone.DroneStatus == @enum.DroneStatus.Delivery && parcel.PickUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
            {
                double distance = DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(parcel.TargetidCustomer.CustomerID).CustomerLocation);
                switch ((int)drone.Weight)
                {
                    case (int)@enum.WeightCategories.Light:
                        drone.Battery -= (int)distance * (int)carriesLightWeight;
                        break;
                    case (int)@enum.WeightCategories.Midium:
                        drone.Battery -= (int)distance * (int)carriesMediumWeight;
                        break;
                    case (int)@enum.WeightCategories.Heavy:
                        drone.Battery -= (int)distance * (int)carriesHeavyWeight;
                        break;
                }
                drone.MyCurrentLocation = CustomerDisplay(parcel.TargetidCustomer.CustomerID).CustomerLocation;
                dal.DeliveryParcelToCustomer(drone.DroneID);
            }
            else throw new WorngStatusException("The parcel couldnt be delivered");
        }

        /// <summary>
        /// Display one BL drone
        /// </summary>
        /// <param name="droneID">drone ID</param>
        /// <returns>The wanted drone</returns>
        public Drone DisplayDrone(int droneID)
        {
            Drone droneBO = new Drone();
            try
            {
                IDAL.DO.Drone droneDO = dal.DroneDisplay(droneID);
                droneDO.CopyPropertiesTo(droneBO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            if (!DroneListBL.Exists(item => item.DroneID == droneID))
                throw new ItemNotExistException("Drone not found");

            DroneToList tmp = DroneListBL.Find(item => item.DroneID == droneID);
            droneBO.MyParcel = new();
            droneBO.DroneStatus = tmp.DroneStatus;
            droneBO.Battery = tmp.Battery;
            droneBO.MyCurrentLocation = tmp.MyCurrentLocation;

            if (droneBO.DroneStatus == @enum.DroneStatus.Delivery)//if not in a delivery mode= doesnt have any parcel
            {
                List<IDAL.DO.Parcel> parcels = dal.ListParcelDisplay().ToList();
                IDAL.DO.Parcel parcel = parcels.Find(i => i.MyDroneID == droneID);
                droneBO.MyParcel.SenderCustomer = new();
                droneBO.MyParcel.ReceiverCustomer = new();
                parcel.CopyPropertiesTo(droneBO.MyParcel);

                if (parcel.PickUp == DateTime.MinValue)//is not already picked
                    droneBO.MyParcel.Status = false;//waiting
                else droneBO.MyParcel.Status = true;//on the way

                IDAL.DO.Customer senderCustomer = dal.CustomerDisplay(parcel.Sender);
                IDAL.DO.Customer receiverCustomer = dal.CustomerDisplay(parcel.Targetid);
                senderCustomer.CopyPropertiesTo(droneBO.MyParcel.SenderCustomer);
                receiverCustomer.CopyPropertiesTo(droneBO.MyParcel.ReceiverCustomer);

                droneBO.MyParcel.Collection = new() { Longitude = senderCustomer.Longitude, Latitude = senderCustomer.Latitude };
                droneBO.MyParcel.Delivery = new() { Longitude = receiverCustomer.Longitude, Latitude = receiverCustomer.Latitude };
                droneBO.MyParcel.TransportDistance = DistanceCalculation(droneBO.MyParcel.Collection, droneBO.MyParcel.Delivery);
            }
            return droneBO;
        }


        /// <summary>
        /// Displays the list of drones
        /// </summary>
        /// <returns>The list of drones</returns>
        public IEnumerable<DroneToList> ListDroneDisplay()
        {
            return DroneListBL;
        }
    }
}