using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public class DalObject : IDal
    {
        #region Add
        /// <summary>
        /// DalObject constructor
        /// </summary>
        public DalObject() { DataSource.Initialize(); }

        /// <summary>
        /// Adds a new drone
        /// </summary>
        /// <param name="NewDrone">The new drone that will be added to the list of drones</param>
        public void Add(Drone NewDrone)
        {
            //checks if the drone exists and if not throws an exception
            if (DataSource.Drones.Exists(item => item.ID == NewDrone.ID))
                throw new AlreadyExistedItemException("The drone already exists");
            DataSource.Drones.Add(NewDrone);
        }

        /// <summary>
        /// Adds a new station 
        /// </summary>
        /// <param name="NewStation">The new station that will be added to the list of stations</param>
        public void Add(Station NewStation)
        {
            //checks if the station exists and if not throws an exception
            if (DataSource.Stations.Exists(item => item.ID == NewStation.ID))
                throw new AlreadyExistedItemException("The station already exists");
            DataSource.Stations.Add(NewStation);
        }

        /// <summary>
        /// Adds a new parcel
        /// </summary>
        /// <param name="NewParcel">The new parcel that will be added to the list of parcels</param>
        public void Add(Parcel NewParcel)
        {
            NewParcel.ID = DataSource.Config.RunnerIDNumParcels++;
            DataSource.Parcels.Add(NewParcel);
        }

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="NewCustomer">The new Customer that will be added to the list of customer</param>
        public void Add(Customer NewCustomer)
        {
            //checks if the customer exists and if not throws an exception
            if (DataSource.Customers.Exists(item => item.ID == NewCustomer.ID))
                throw new AlreadyExistedItemException("The customer already exists");
            DataSource.Customers.Add(NewCustomer);
        }
        #endregion

        #region Updates
        /// <summary>
        /// Assigns a drone to the parcel
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be assigns</param>
        /// <param name="DroneID">The drone's ID that needs to be assigns</param>
        public void AssignParcelToDrone(int ParcelID, int DroneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.ID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(item => item.ID == ParcelID);
            if (index < 0)//not found
                throw new ItemNotExistException("The station does not exists");
            //updates the parcel
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = DroneID;
            tmp.Scheduled = DateTime.Now;//updates the scheduled time
            DataSource.Parcels[index] = tmp;
        }

        /// <summary>
        /// Collection of a requested parcel by a drone
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
        public void CollectionOfParcelByDrone(int ParcelID, int DroneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.ID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(item => item.ID == ParcelID);
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            //updates the parcel
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = DroneID;
            tmp.PickUp = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }

        /// <summary>
        /// Delivery parcil to customer
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
        public void DeliveryParcelToCustomer(int ParcelID)
        {
            int index = DataSource.Parcels.FindIndex(item => item.ID == ParcelID);//finds the parcel 
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }
        /// <summary>
        /// Sending drone to charging base station
        /// </summary>
        /// <param name="DroneID">the wanted drone</param>
        /// <param name="StationID">the wanted station</param>
        public void SendingDroneToChargingBaseStation(int DroneID, int StationID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.ID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = DataSource.Stations.FindIndex(item => item.ID == StationID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.RecDrone = DroneID;
            ChargingDroneBattery.RecBaseStation = StationID;
            //adds
            DataSource.DroneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            Station tmp = DataSource.Stations[index];
            tmp.NumOfAvailableChargeSlots--;
            DataSource.Stations[index] = tmp;
        }

        /// <summary>
        /// Releasing a drone from a charging Base Station.
        /// </summary>
        /// <param name="DroneID">The ID of the wanted drone</param>
        /// <param name="BaseStationID">The ID of the wanted station</param>
        public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID)
        {
            //checks if the drone and station exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.ID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            if (!DataSource.Stations.Exists(item => item.ID == BaseStationID))
                throw new ItemNotExistException("The station does not exists");
            IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();
            //Finds the station that the drone was released from and updates the number of available charging slots.
            for (int i = 0; iter.MoveNext(); i++)
            {
                if (iter.Current.ID == BaseStationID)
                {
                    Station tmp = iter.Current;
                    tmp.NumOfAvailableChargeSlots++;
                    DataSource.Stations[i] = tmp;
                    break;
                }
            }
            foreach (DroneCharge delDroneCharge in DataSource.DroneCharges)//delete the dronecharge from the list of DroneCharge)
            {
                if (delDroneCharge.RecDrone == DroneID && delDroneCharge.RecBaseStation == BaseStationID)
                {
                    DataSource.DroneCharges.Remove(delDroneCharge);
                    break;
                }
            }


            //int index = DataSource.DroneCharges.FindIndex(item => item.RecDrone == DroneID  && item.RecBaseStation == BaseStationID);//finds the drone charge 
            //if (index< 0)//not found
            //    throw new ItemNotExistException("the drone does not exist in the wanted base station");
            //DataSource.DroneCharges.RemoveAt(index);//delete the drone from the list of the drone charge
            //index = DataSource.Stations.FindIndex(item => item.ID == BaseStationID);//finds the station
            //Station tmp = DataSource.Stations[index];
            //tmp.NumOfAvailableChargeSlots++;
            //DataSource.Stations[index] = tmp;
        }

        public void UpdateStation(int ID, string name = null, int? numOfSlots = null)
        {
            int index = DataSource.Stations.FindIndex(item => item.ID == ID);
            if (index == -1)
                throw new ItemNotExistException("The station does not exist");
            Station tmp = DataSource.Stations[index];
            if (name != null)
            {
                tmp.Name = name;
                DataSource.Stations[index] = tmp;
            }
            if (numOfSlots != null)
            {
                int numOfFullSlots = DataSource.DroneCharges.Count(item => item.RecBaseStation == ID);//does it work??
                tmp.NumOfAvailableChargeSlots = (int)(numOfSlots - numOfFullSlots);
                DataSource.Stations[index] = tmp;
            }
        }

        public void UpdateCustomer(int ID, string name = null, string phone = null)
        {
            int index = DataSource.Customers.FindIndex(item => item.ID == ID);
            if (index == -1)
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

        public void UpdateDroneName(int ID, string model)
        {
            int index = DataSource.Drones.FindIndex(item => item.ID == ID);
            if (index == -1)
                throw new ItemNotExistException("Drone does not exist");
            Drone tmp = DataSource.Drones[index];
            tmp.Model = model;
            DataSource.Drones[index]=tmp;
        }
        #endregion 

        #region Display one item
        /// <summary>
        /// Return the wanted drone
        /// </summary>
        /// <param name="DroneID">The requested drone</param>
        public Drone DroneDisplay(int DroneID)
        {
            if (!DataSource.Drones.Exists(item => item.ID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Drones.Find(item => item.ID == DroneID);
        }

        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        public Station StationDisplay(int StationID)
        {
            if (!DataSource.Stations.Exists(item => item.ID == StationID))
                throw new ItemNotExistException("The station does not exists");
            return DataSource.Stations.Find(item => item.ID == StationID);
        }

        /// <summary>
        /// Return the wanted customer
        /// </summary>
        /// <param name="CustomerID">The requested customer</param>
        public Customer CustomerDisplay(int CustomerID)
        {
            if (!DataSource.Customers.Exists(item => item.ID == CustomerID))
                throw new ItemNotExistException("The customer does not exists");
            return DataSource.Customers.Find(item => item.ID == CustomerID);
        }

        /// <summary>
        /// Return the wanted parcel
        /// </summary>
        /// <param name="ParcelID"> The requested parcel</param>
        public Parcel ParcelDisplay(int ParcelID)
        {
            if (!DataSource.Parcels.Exists(item => item.ID == ParcelID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Parcels.Find(item => item.ID == ParcelID);
        }
        #endregion

        #region Lists of items
        /// <summary>
        /// Returns all the drones in the list.
        /// </summary>
        public IEnumerable<Drone> ListDroneDisplay()
        {
            List<Drone> ListOfDrones = new();
            foreach (Drone currentDrone in DataSource.Drones) { ListOfDrones.Add(currentDrone); }
            return ListOfDrones;
        }

        /// <summary>
        /// Returns all the customers in the list.
        /// </summary>
        public IEnumerable<Customer> ListCustomerDisplay(Predicate<Customer> predicate = null)
        {
            List<Customer> ListOfCustomers = new();
            foreach (Customer currentCostomer in DataSource.Customers) { ListOfCustomers.Add(currentCostomer); }
            return ListOfCustomers.FindAll(i => predicate == null ? true : predicate(i));
        }

        /// <summary>
        /// Returns all the station in the list 
        /// </summary>
        public IEnumerable<Station> ListStationDisplay(Predicate<Station> predicate = null)
        {
            List<Station> ListOfStation = new();
            foreach (Station currentStation in DataSource.Stations) { ListOfStation.Add(currentStation); }
            return ListOfStation.FindAll(i => predicate == null ? true : predicate(i));
        }

        /// <summary>
        /// Returns all the parcel in the list
        /// </summary>
        public IEnumerable<Parcel> ListParcelDisplay(Predicate<Parcel> predicate = null)
        {
            List<Parcel> ListOfParcel = new();
            foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
            return ListOfParcel.FindAll(i => predicate == null ? true : predicate(i));
        }

        /// <summary>
        /// Returns all the Unassigned parcels.
        /// </summary>
        public IEnumerable<Parcel> ListOfUnassignedParcels()
        {
            List<Parcel> UnassignedParcels = new();
            foreach (Parcel currentParcel in DataSource.Parcels)
            {
                if (currentParcel.MyDroneID == 0) UnassignedParcels.Add(currentParcel);
            }
            return UnassignedParcels;
        }

        /// <summary>
        /// Returns all the base stations with available charging stations
        /// </summary>
        public IEnumerable<Station> ListOfAvailableChargingStations()
        {
            List<Station> AvailableChargingStations = new();
            foreach (Station currentStation in DataSource.Stations)
            {
                if (currentStation.NumOfAvailableChargeSlots != 0) AvailableChargingStations.Add(currentStation);
            }
            return AvailableChargingStations;
        }
        #endregion

        /// <summary>
        /// requests power consumption by a drone 
        /// </summary>
        /// <returns>returns an array of numbers of double type</returns>
        public double[] ChargingDrone()
        {
            double[] arr = { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };
            return arr;

        }

    }
}



