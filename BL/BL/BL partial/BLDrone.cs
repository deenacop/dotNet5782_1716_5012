using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DalApi;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        public void AddDrone(Drone drone, int stationID)
        {
            Random rand = new(DateTime.Now.Millisecond);//Random variable for use along the function
            if (CheckNumOfDigits(drone.Id) != 3)
                throw new WrongInputException("Bad Drone ID");
            if (drone.Model == null || drone.Model == "")
                throw new WrongInputException("Missing drone model");
            try
            {
                DroneToList listDrone = new();
                DO.Station wantedStation = dal.GetStation(stationID);
                
                drone.Location = new() { Longitude = wantedStation.Longitude, Latitude = wantedStation.Latitude };
                drone.Battery = rand.Next(20, 41);
                drone.Status = DroneStatus.Maintenance;//By adding a drone it is initialized to a maintenance mode
                listDrone.Location = new() { Longitude = wantedStation.Longitude, Latitude = wantedStation.Latitude };

                drone.CopyPropertiesTo(listDrone);
                object obj = new DO.Drone();//Boxing and unBoxing
                drone.CopyPropertiesTo(obj);
                dal.Add((DO.Drone)obj); //calls the function from DALOBJECT
                DroneListBL.Add(listDrone);
                //listDrone.CopyPropertiesTo(drone);
                dal.SendingDroneToChargingBaseStation(listDrone.Id, stationID);
            }
            catch (ItemNotExistException ex)
            {
                throw new ItemNotExistException("Station does not exist", ex);
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw new ItemAlreadyExistsException("Trying to add an existing drone", ex);
            }
        }

        public void RemoveDrone(Drone drone)
        {
            try
            {
                DO.Drone droneDO = new();
                object obj = droneDO;//boxing and unBoxing
                drone.CopyPropertiesTo(obj);
                droneDO = (DO.Drone)obj;
                dal.Remove(droneDO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }

        }
        public void UpdateDrone(Drone drone)
        {
            if (drone.Model == null || drone.Model == "")
                throw new WrongInputException("Missing drone model");

            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            if(drone.Status==DroneStatus.Delivery)//
                throw new WorngStatusException ("Drone cant be update while delivery");
            drone.CopyPropertiesTo(listDrone);
            object obj = new DO.Drone();//Boxing and unBoxing
            drone.CopyPropertiesTo(obj);
            try
            {
                dal.UpdateDrone((DO.Drone)obj);//calls the function from DALOBJECT
            }
            catch (DO.ItemNotExistException ex)
            {
                throw new ItemNotExistException("Drone does not exist", ex);
            }
        }


        public void SendDroneToCharge(Drone drone)
        {
            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Dronen {drone.Id} does not exist");
            if (listDrone.Status != DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone is not available");
            List<BaseStation> BaseStationListBL = new();
            IEnumerable<DO.Station> StationListDL = dal.GetListStation(i => i.NumOfAvailableChargingSlots > 0);//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesToIEnumerable(BaseStationListBL);//convret from DalApi to BL
            //Set the locations:
            IEnumerable<int> counter = Enumerable.Range(0, StationListDL.Count());
            foreach (int j in counter)
            {
                BaseStationListBL.ElementAt(j).Location = new()
                {
                    Longitude = StationListDL.ElementAt(j).Longitude,
                    Latitude = StationListDL.ElementAt(j).Latitude
                };
            }
            if (BaseStationListBL.Count() <= 0)
                throw new ItemNotExistException("There is no stations with available slots");
            //Finds the closest station from the drone
            double distance = MinDistanceLocation(BaseStationListBL, listDrone.Location).Item2;
            if (distance * vacant > listDrone.Battery)
                throw new NotEnoughBatteryException("The drone can not reach the station - there is not enough battery");
            try
            {
                dal.SendingDroneToChargingBaseStation(
                    listDrone.Id, MinDistanceLocation(BaseStationListBL, listDrone.Location).Item3);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //if the drone *can* go to a charging Drone station:
            listDrone.Battery -= (int)(distance * vacant);
            listDrone.Status = DroneStatus.Maintenance;
            listDrone.Location = MinDistanceLocation(BaseStationListBL, listDrone.Location).Item1;
            int index = DroneListBL.FindIndex(item => item.Id == listDrone.Id);//finds the drone with the wanted ID
            DroneListBL[index] = listDrone;
        }

        public void ReleasingDroneFromBaseStation(Drone drone)
        {
            //Finds the wanted station:
            int baseStationId = dal.GetListStation().First(i => i.Latitude == drone.Location.Latitude && i.Longitude == drone.Location.Longitude).Id;
            DO.DroneCharge droneCharge = dal.GetDroneCharge(drone.Id, baseStationId).Item1;
            //Calculate the time the drone was in charge
            TimeSpan diff = DateTime.Now - (DateTime)droneCharge.EnterToChargBase;
            int minuteInCharge = (int)diff.TotalSeconds / 60;
            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            if (listDrone.Battery + (int)(minuteInCharge * droneLoadingRate) <= 30)//The minimal claim has not ended and the drone is not available
                throw new NotEnoughBatteryException("The drone needs to be charged");
            try
            {//Finds the station that has the wanted location
                DO.Station station = dal.GetListStation(item => item.Latitude == listDrone.Location.Latitude
                     && item.Longitude == listDrone.Location.Longitude).First();
                //update
                dal.ReleasingDroneFromChargingBaseStation(listDrone.Id, station.Id);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }

            //update the drone
            listDrone.Battery += (int)(minuteInCharge * droneLoadingRate);
            if (listDrone.Battery > 100)//cant be more than 100
                listDrone.Battery = 100;
            listDrone.Status = DroneStatus.Available;
            int index = DroneListBL.FindIndex(item => item.Id == listDrone.Id);
            DroneListBL[index] = listDrone;
        }

        public void AssignParcelToDrone(Drone drone)
        {
            int index = DroneListBL.FindIndex(i => i.Id == drone.Id);//find index of the wanted drone
                                                                     // Drone drone = GetDrone(ID);//drone from IDAL.DO
            if (drone.Status != DroneStatus.Available)
                throw new WorngStatusException("The drone is not available");
            //Brings the list of parcels sorted by order of urgency
            IEnumerable<DO.Parcel> parcels = dal.GetListParcel(i => i.MyDroneID == 0 && (int)i.Weight <= (int)drone.Weight).
                OrderByDescending(currentParcel => currentParcel.Priority).ThenByDescending(currentParcel =>
                 currentParcel.Weight).ThenByDescending(currentParcel =>
                     DistanceCalculation(drone.Location, GetCustomer(currentParcel.Sender).Location)).ToList();
            foreach (DO.Parcel currentParcel in parcels)//Tests whether the drone can perform the shipping route (battery test)
            {
                if (BatteryCheckingForDroneAndParcel(currentParcel, drone))
                {
                    DroneListBL[index].Status = DroneStatus.Delivery;
                    dal.AssignParcelToDrone(currentParcel.Id, drone.Id);
                    return;
                }
            }
            throw new ItemNotExistException("There is no parcel to assign with the drone");
        }

        public void CollectionParcelByDrone(Drone drone)
        {
            int index = DroneListBL.FindIndex(i => i.Id == drone.Id);//find the index of the dron
                                                                     // Drone drone = GetDrone(drone.Id);//drone from IDAL.DO
            if (drone.Status != DroneStatus.Delivery)
                throw new WorngStatusException("The drone is not in delivery mode");
            Parcel parcel = GetParcel(drone.Parcel.Id);//the wanted parcel
            if (parcel.PickUp != null)
                throw new WorngStatusException("The parcel has allready been picked up ");
            double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.SenderCustomer.Id).Location);
            DroneListBL[index].Battery -= (int)(distance * vacant);//update the battery
            DroneListBL[index].Location = GetCustomer(parcel.SenderCustomer.Id).Location;
            dal.CollectionOfParcelByDrone(parcel.Id, drone.Id);//send to the function
        }

        public void DeliveryParcelByDrone(Drone drone)
        {
            int index = DroneListBL.FindIndex(i => i.Id == drone.Id);
            // Drone drone = GetDrone(ID);
            Parcel parcel = GetParcel(drone.Parcel.Id);
            if (drone.Status == DroneStatus.Delivery && parcel.PickUp != null && parcel.Delivered == null)
            {
                double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.TargetidCustomer.Id).Location);
                switch ((int)drone.Weight)
                {
                    case (int)WeightCategories.Light:
                        DroneListBL[index].Battery -= (int)(distance * carriesLightWeight);
                        break;
                    case (int)WeightCategories.Medium:
                        DroneListBL[index].Battery -= (int)(distance * carriesMediumWeight);
                        break;
                    case (int)WeightCategories.Heavy:
                        DroneListBL[index].Battery -= (int)(distance * carriesHeavyWeight);
                        break;
                }
                DroneListBL[index].Location = GetCustomer(parcel.TargetidCustomer.Id).Location;
                DroneListBL[index].Status = DroneStatus.Available;
                dal.DeliveryParcelToCustomer(parcel.Id);
            }
            else throw new WorngStatusException("The parcel couldnt be delivered");
        }

        public DroneInCharging GetDroneInCharge(int id, int stationId)
        {

            DroneInCharging droneInChargingBO = new();
            try
            {
                DO.DroneCharge droneCharge = dal.GetDroneCharge(id, stationId).Item1;

                droneCharge.CopyPropertiesTo(droneInChargingBO);
                droneInChargingBO.Battery=DroneListBL.Find(i => i.Id == id).Battery;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }

            //if(droneInChargingBO.FinishedRecharging!=null)
                return droneInChargingBO;
            //throw new ItemNotExistException("drone does not exist");
        }

        public Drone GetDrone(int id)
        {
            Drone droneBO = new();
            try
            {
                DO.Drone droneDO = dal.GetDrone(id);
                droneDO.CopyPropertiesTo(droneBO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            DroneToList tmp = DroneListBL.Find(item => item.Id == id);
            droneBO.Parcel = new();
            droneBO.Status = tmp.Status;
            droneBO.Battery = tmp.Battery;
            droneBO.Location = tmp.Location;

            if (droneBO.Status == DroneStatus.Delivery)//if not in a delivery mode= doesnt have any parcel
            {
                DO.Parcel parcel = dal.GetListParcel(i => i.MyDroneID == id).First();
                droneBO.Parcel.SenderCustomer = new();
                droneBO.Parcel.ReceiverCustomer = new();
                parcel.CopyPropertiesTo(droneBO.Parcel);

                if (parcel.PickUp == null)//is not already picked
                    droneBO.Parcel.Status = false;//waiting
                else droneBO.Parcel.Status = true;//on the way

                DO.Customer senderCustomer = dal.GetCustomer(parcel.Sender);
                DO.Customer receiverCustomer = dal.GetCustomer(parcel.Targetid);
                senderCustomer.CopyPropertiesTo(droneBO.Parcel.SenderCustomer);
                receiverCustomer.CopyPropertiesTo(droneBO.Parcel.ReceiverCustomer);

                droneBO.Parcel.Collection = new() { Longitude = senderCustomer.Longitude, Latitude = senderCustomer.Latitude };
                droneBO.Parcel.Delivery = new() { Longitude = receiverCustomer.Longitude, Latitude = receiverCustomer.Latitude };
                droneBO.Parcel.TransportDistance = (float)DistanceCalculation(droneBO.Parcel.Collection, droneBO.Parcel.Delivery);
            }
            return droneBO;
        }

      
        public IEnumerable<DroneInCharging> GetDroneInChargingList(Predicate<DroneInCharging> predicate = null)
        {
            List<DroneInCharging> droneChargeBO=new();
            foreach (DO.DroneCharge droneCharge in dal.GetListDroneCharge())
            {
                droneChargeBO.Add(GetDroneInCharge(droneCharge.Id, droneCharge.BaseStationID));
            }
            return droneChargeBO;
        }
       
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DroneListBL.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}