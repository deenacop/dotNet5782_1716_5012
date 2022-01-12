﻿using System;
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
                //we add a drone but the type is DroneToList
                DroneToList listDrone = new();
                DO.Station wantedStation = dal.GetStation(stationID);
                //needs to be initialize by hand
                drone.Location = new()
                {
                    Longitude = wantedStation.Longitude,
                    Latitude = wantedStation.Latitude
                };
                drone.Battery = rand.Next(20, 41);
                drone.Status = DroneStatus.Maintenance;//By adding a drone it is initialized to a maintenance mode
                //needs to be initialize by hand
                listDrone.Location = new();
                listDrone.Location = drone.Location;
                drone.CopyPropertiesTo(listDrone);
                object obj = new DO.Drone();//Boxing and unBoxing
                drone.CopyPropertiesTo(obj);
                dal.Add((DO.Drone)obj); //calls the function from DALOBJECT
                DronesBL.Add(listDrone);//Should be added to the list maintained in BL  
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
                DroneToList listDrone = DronesBL.First(d => d.Id == drone.Id);
                DO.Drone droneDO = new();
                object obj = droneDO;//boxing and unBoxing
                drone.IsRemoved = true;//remove
                drone.CopyPropertiesTo(listDrone);//updates also the list
                drone.CopyPropertiesTo(obj);
                droneDO = (DO.Drone)obj;
                if (drone.Status == DroneStatus.Available)
                    dal.Remove(droneDO);
                if (drone.Status == DroneStatus.Maintenance)
                {
                    //Finds the wanted station:
                    int baseStationId = dal.GetListStation().First(i => i.Latitude == drone.Location.Latitude && i.Longitude == drone.Location.Longitude).Id;
                    DO.DroneCharge droneCharge = dal.GetDroneCharge(drone.Id, baseStationId);
                    //Calculate the time the drone was in charge
                    TimeSpan diff = DateTime.Now - (DateTime)droneCharge.EnterToChargBase;
                    int minuteInCharge = (int)diff.TotalSeconds / 60;
                    DroneToList droneList = DronesBL.FirstOrDefault(d => d.Id == drone.Id);
                    if (droneList == null)
                        throw new ItemNotExistException("Drone does not exist");
                    try
                    {//Finds the station that has the wanted location
                        DO.Station station = dal.GetListStation(item => item.Latitude == droneList.Location.Latitude
                             && item.Longitude == droneList.Location.Longitude).First();
                        //update
                        dal.ReleasingDroneFromChargingBaseStation(droneList.Id, station.Id);
                    }
                    catch (Exception ex)
                    {
                        throw new ItemNotExistException(ex.Message);
                    }
                    //update the drone
                    droneList.Battery += (int)(minuteInCharge * droneLoadingRate);
                    if (droneList.Battery > 100)//cant be more than 100
                        droneList.Battery = 100;
                    droneList.Status = DroneStatus.Available;
                    int index = DronesBL.FindIndex(item => item.Id == droneList.Id);
                    DronesBL[index] = droneList;
                    dal.Remove(droneDO);
                }
                if (drone.Status == DroneStatus.Delivery)
                    throw new ItemCouldNotBeRemoved("The drone is in delivery mode and could not be removed!");
            }
            catch (ItemNotExistException ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            catch (InvalidOperationException)
            {
                throw new ItemCouldNotBeRemoved("The drone is in delivery mode and could not be removed!");
            }

        }
        public void UpdateDrone(Drone drone)
        {
            if (drone.Model == null || drone.Model == "")
                throw new WrongInputException("Missing drone model");
            DroneToList listDrone = DronesBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            if (drone.Status == DroneStatus.Delivery)
                throw new WorngStatusException("Drone cant be update while delivery");
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
            DroneToList listDrone = DronesBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone.Status != DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone is not available");
            List<BaseStation> BaseStationListBL = new();
            IEnumerable<DO.Station> StationListDL = dal.GetListStation(i => i.NumOfAvailableChargingSlots > 0 && !i.IsRemoved);//Receive the drone list from the data layer.
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
            listDrone.CopyPropertiesTo(drone);
        }

        public void ReleasingDroneFromBaseStation(Drone drone)
        {
            DroneToList listDrone = DronesBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            //Finds the wanted station:
            int baseStationId = dal.GetListStation().FirstOrDefault(i => i.Latitude == drone.Location.Latitude && i.Longitude == drone.Location.Longitude).Id;
            DO.DroneCharge droneCharge = dal.GetDroneCharge(drone.Id, baseStationId);
            //Calculate the time the drone was in charge
            TimeSpan diff = DateTime.Now - (DateTime)droneCharge.EnterToChargBase;
            int minuteInCharge = (int)diff.TotalSeconds / 60;
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
            listDrone.CopyPropertiesTo(drone);
        }

        public void AssignParcelToDrone(Drone drone)
        {
            DroneToList droneToList = DronesBL.Find(i => i.Id == drone.Id);
            if(droneToList==null)
                throw new ItemNotExistException("Drone does not exist");
            if (drone.Status != DroneStatus.Available)
                throw new WorngStatusException("The drone is not available");
            //Brings the list of parcels sorted by order of urgency
            IEnumerable<DO.Parcel> parcels = dal.GetListParcel(i => i.MyDroneID == 0 && (int)i.Weight <= (int)drone.Weight && i.PickUp==null).
                OrderByDescending(currentParcel => currentParcel.Priority).ThenByDescending(currentParcel =>
                 currentParcel.Weight).ThenByDescending(currentParcel =>
                     DistanceCalculation(drone.Location, GetCustomer(currentParcel.Sender).Location));
            if (parcels.Any())//there is a parcel that matched to the drone
            {
                DO.Parcel p = parcels.First(i => BatteryCheckingForDroneAndParcel(i, drone));
                drone.CopyPropertiesTo(droneToList);
                droneToList.Status = DroneStatus.Delivery;
                droneToList.ParcelId = p.Id;
                dal.AssignParcelToDrone(p.Id, drone.Id);
            }
            else
                 throw new ItemNotExistException("There is no parcel to assign with the drone");
        }

        public void CollectionParcelByDrone(Drone drone)
        {
            DroneToList droneToList = DronesBL.Find(i => i.Id == drone.Id);
            if (droneToList == null)
                throw new ItemNotExistException("Drone does not exist");
            if (drone.Status != DroneStatus.Delivery)
                throw new WorngStatusException("The drone is not in delivery mode");
            Parcel parcel = GetParcel(drone.Parcel.Id);//the wanted parcel
            if (parcel.PickUp != null)
                throw new WorngStatusException("The parcel has allready been picked up ");
            double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.SenderCustomer.Id).Location);
            drone.Battery -= (int)(distance * vacant);//update the battery
            drone.CopyPropertiesTo(droneToList);
            droneToList.Location = GetCustomer(parcel.SenderCustomer.Id).Location;
            dal.CollectionOfParcelByDrone(parcel.Id, drone.Id);//send to the function
        }

        public void DeliveryParcelByDrone(Drone drone)
        {
            DroneToList droneToList = DronesBL.Find(i => i.Id == drone.Id);
            if (droneToList == null)
                throw new ItemNotExistException("Drone does not exist");
            Parcel parcel = GetParcel(drone.Parcel.Id);
            if (drone.Status == DroneStatus.Delivery && parcel.PickUp != null && parcel.Delivered == null)
            {
                double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.TargetidCustomer.Id).Location);
                double batteryDropped = setBattery(0, distance, drone.Weight);
                drone.CopyPropertiesTo(droneToList);
                droneToList.Battery -= (int)batteryDropped;
                droneToList.Location = GetCustomer(parcel.TargetidCustomer.Id).Location;
                droneToList.Status = DroneStatus.Available;
                droneToList.CopyPropertiesTo(drone);
                dal.DeliveryParcelToCustomer(parcel.Id);
            }
            else throw new WorngStatusException("The parcel couldnt be delivered");
        }
        public Drone GetDrone(int id)
        {
            try
            {
                Drone droneBO = new();
                DroneToList droneToList = DronesBL.First(item => item.Id == id);//brings the dron from BL
                //needs to be set by hand
                droneBO.Location = new();
                droneBO.Location = droneToList.Location;
                droneToList.CopyPropertiesTo(droneBO);
                droneBO.Parcel = new();
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
            catch (InvalidOperationException)//drone does not found
            {
                throw new ItemNotExistException("drone does not exist");
            }
        }
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DronesBL.FindAll(i => !i.IsRemoved);
        }
    }
}