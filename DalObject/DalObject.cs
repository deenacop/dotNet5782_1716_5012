﻿using System;
using System.Linq;
using System.Collections.Generic;
using DO;
using DalApi;

namespace Dal
{
    internal sealed class DalObject : DalApi.IDal
    {
        #region singelton
        internal static DalObject instance { get { return Instance.Value; } }
        private static readonly Lazy<DalObject> Instance = new Lazy<DalObject>(() => new DalObject()); //Lazy initialization of an object means that its creation is deferred until it is first used.
        static DalObject() {  }
        private DalObject() { DataSource.Initialize(); }//ctor
     
        #endregion

        #region Add
        public void Add(Drone drone)
        {
            //checks if the drone exists and if not throws an exception
            if (DataSource.Drones.Exists(i => i.Id == drone.Id))
                throw new AlreadyExistedItemException("The drone already exists");
            DataSource.Drones.Add(drone);
        }

        public void Add(Station station)
        {
            //checks if the station exists and if not throws an exception
            if (DataSource.Stations.Exists(i => i.Id == station.Id))
                throw new AlreadyExistedItemException("The station already exists");
            DataSource.Stations.Add(station);
        }

        public void Add(Parcel parcel)
        {
            parcel.Id = DataSource.Config.RunnerIDNumParcels++;
            DataSource.Parcels.Add(parcel);
        }

        public void Add(Customer customer)
        {
            //checks if the customer exists and if not throws an exception
            if (DataSource.Customers.Exists(i => i.Id == customer.Id))
                throw new AlreadyExistedItemException("The customer already exists");
            DataSource.Customers.Add(customer);
        }
        #endregion

        #region Drone operations
        public void AssignParcelToDrone(int parcelID, int droneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);
            if (index ==-1)//not found
                throw new ItemNotExistException("The station does not exists");
            //updates the parcel
            Parcel parcel = DataSource.Parcels[index];
            parcel.MyDroneID = droneID;
            parcel.Scheduled = DateTime.Now;//updates the scheduled time
            DataSource.Parcels[index] = parcel;
        }

        public void CollectionOfParcelByDrone(int parcelID, int droneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            //updates the parcel
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = droneID;
            tmp.PickUp = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }

        public void DeliveryParcelToCustomer(int parcelID)
        {
            int index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);//finds the parcel
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }

        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = DataSource.Stations.FindIndex(i => i.Id == stationID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.Id = droneID;
            ChargingDroneBattery.BaseStationID = stationID;
            ChargingDroneBattery.EnterToChargBase = DateTime.Now;
            //adds
            DataSource.DroneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            Station tmp = DataSource.Stations[index];
            tmp.NumOfAvailableChargingSlots--;
            DataSource.Stations[index] = tmp;
        }

        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID)
        {
            //checks if the drone and station exists and if not throws an exception
            if (!DataSource.Drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            int index = DataSource.Stations.FindIndex(i => i.Id == baseStationID);
            if (index==-1)
                throw new ItemNotExistException("The station does not exists");
            Station station = DataSource.Stations[index];
            station.NumOfAvailableChargingSlots++;
            DataSource.Stations[index] = station;
            DroneCharge drone = GetDroneCharge(droneID, baseStationID).Item1;
            index = GetDroneCharge(droneID, baseStationID).Item2;//get the specific index of the wanted dronecharge
            drone.FinishedRecharging = DateTime.Now; ;//delete the drone from the list of the drone charge
            DataSource.DroneCharges[index] = drone;

        }
        #endregion

        #region Updates

        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(item => item.Id == drone.Id);
            if (index == -1)//not exist
                throw new ItemNotExistException("Drone does not exist");
            DataSource.Drones[index] = drone;
        }
        public void UpdateStation(Station station)
        {
            int index = DataSource.Stations.FindIndex(i => i.Id == station.Id);
            if (index == -1)//not exist
                throw new ItemNotExistException("The station does not exist");
            DataSource.Stations[index] = station;

        }
        public void UpdateCustomer(int ID, string name = null, string phone = null)
        {
            int index = DataSource.Customers.FindIndex(item => item.Id == ID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The customer does not exsit");
            Customer tmp = DataSource.Customers[index];
            if (name != null)
            {
                tmp.Name = name;
                DataSource.Customers[index] = tmp;
            }
            if (phone != null)
            {
                tmp.PhoneNumber = phone;
                DataSource.Customers[index] = tmp;
            }
        }

        #endregion

        #region Get item
        public (DroneCharge, int) GetDroneCharge(int droneID, int baseStationId)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.Id == droneID && i.BaseStationID == baseStationId);//finds the drone charge
            if (index < 0)//not found
                throw new ItemNotExistException("the drone does not exist in the wanted base station");
            return (DataSource.DroneCharges[index], index);
        }
        public Drone GetDrone(int droneID)
        {
            if (!DataSource.Drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Drones.Find(i => i.Id == droneID);
        }

        public Station GetStation(int stationID)
        {
            if (!DataSource.Stations.Exists(i => i.Id == stationID))
                throw new ItemNotExistException("The station does not exists");
            return DataSource.Stations.Find(i => i.Id == stationID);
        }

        public Customer GetCustomer(int customerID)
        {
            if (!DataSource.Customers.Exists(i => i.Id == customerID))
                throw new ItemNotExistException("The customer does not exists");
            return DataSource.Customers.Find(i => i.Id == customerID);
        }

        public Parcel GetParcel(int parcelID)
        {
            if (!DataSource.Parcels.Exists(i => i.Id == parcelID))
                throw new ItemNotExistException("The parcel does not exists");
            return DataSource.Parcels.Find(i => i.Id == parcelID);
        }
        #endregion

        #region Lists of items
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null)
        {
            return from item in DataSource.Drones
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null)
        {
            //List<Customer> ListOfCustomers = new();
            //foreach (Customer currentCostomer in DataSource.Customers) { ListOfCustomers.Add(currentCostomer); }
            //return ListOfCustomers.FindAll(i => predicate == null ? true : predicate(i));
            return from item in DataSource.Customers
                   where (predicate == null ? true : predicate(item))
                   select item;

        }

        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null)
        {
            return from item in DataSource.Stations
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null)
        {
            //List<Parcel> ListOfParcel = new();
            //foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
            //return ListOfParcel.FindAll(i => predicate == null ? true : predicate(i));
            return from item in DataSource.Parcels
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null)
        {
            //List<DroneCharge> droneCharging = new();
            //foreach (DroneCharge currentDrone in DataSource.DroneCharges) { droneCharging.Add(currentDrone); }
            //return droneCharging.FindAll(i => predicate == null ? true : predicate(i)); 
            return from item in DataSource.DroneCharges
                   where (predicate == null ? true : predicate(item))
                   select item;
        }
        #endregion

        public double[] ChargingDrone()
        {
            return new double[] { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };

        }
    }
}



