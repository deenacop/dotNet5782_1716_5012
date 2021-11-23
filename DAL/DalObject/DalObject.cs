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
            if (DataSource.Drones.Exists(item => item.DroneID == NewDrone.DroneID))
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
            if (DataSource.Stations.Exists(item => item.StationID == NewStation.StationID))
                throw new AlreadyExistedItemException("The station already exists");
            DataSource.Stations.Add(NewStation);
        }

        /// <summary>
        /// Adds a new parcel
        /// </summary>
        /// <param name="NewParcel">The new parcel that will be added to the list of parcels</param>
        public void Add(Parcel NewParcel)
        {
            NewParcel.ParcelID = DataSource.Config.RunnerIDNumParcels++;
            DataSource.Parcels.Add(NewParcel);
        }

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="NewCustomer">The new Customer that will be added to the list of customer</param>
        public void Add(Customer NewCustomer)
        {
            //checks if the customer exists and if not throws an exception
            if (DataSource.Customers.Exists(item => item.CustomerID == NewCustomer.CustomerID))
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
            if (!DataSource.Drones.Exists(item => item.DroneID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(item => item.ParcelID == ParcelID);
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
            if (!DataSource.Drones.Exists(item => item.DroneID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = DataSource.Parcels.FindIndex(item => item.ParcelID == ParcelID);
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
            int index = DataSource.Parcels.FindIndex(item => item.ParcelID == ParcelID);//finds the parcel
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
            if (!DataSource.Drones.Exists(item => item.DroneID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = DataSource.Stations.FindIndex(item => item.StationID == StationID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.DroneID = DroneID;
            ChargingDroneBattery.BaseStationID = StationID;
            ChargingDroneBattery.FinishedRecharging = DateTime.MinValue;
            //adds
            DataSource.DroneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            Station tmp = DataSource.Stations[index];
            tmp.NumOfAvailableChargingSlots--;
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
            if (!DataSource.Drones.Exists(item => item.DroneID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            if (!DataSource.Stations.Exists(item => item.StationID == BaseStationID))
                throw new ItemNotExistException("The station does not exists");
            int index = DataSource.DroneCharges.FindIndex(item => item.DroneID == DroneID && item.BaseStationID == BaseStationID);//finds the drone charge
            if (index < 0)//not found
                throw new ItemNotExistException("the drone does not exist in the wanted base station");
            DroneCharge tmp1 = DataSource.DroneCharges[index];
            tmp1.FinishedRecharging = DateTime.Now; ;//delete the drone from the list of the drone charge
            DataSource.DroneCharges[index] = tmp1;
            index = DataSource.Stations.FindIndex(item => item.StationID == BaseStationID);//finds the station
            Station tmp2 = DataSource.Stations[index];
            tmp2.NumOfAvailableChargingSlots++;
            DataSource.Stations[index] = tmp2;
            //IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();
            ////Finds the station that the drone was released from and updates the number of available charging slots.
            //for (int i = 0; iter.MoveNext(); i++)
            //{
            //    if (iter.Current.StationID == BaseStationID)
            //    {
            //        Station tmp = iter.Current;
            //        tmp.NumOfAvailableChargingSlots++;
            //        DataSource.Stations[i] = tmp;
            //        break;
            //    }
            //}
            //DataSource.DroneCharges.Fi(i => i.DroneID == DroneID && i.BaseStationID == BaseStationID);

            //foreach (DroneCharge delDroneCharge in DataSource.DroneCharges)//delete the dronecharge from the list of DroneCharge)
            //{
            //    if (delDroneCharge.DroneID == DroneID && delDroneCharge.BaseStationID == BaseStationID)
            //    {
            //        DroneCharge tmp = delDroneCharge;
            //        tmp.FinishedRecharging = DateTime.Now;
            //        delDroneCharge = tmp;
            //        break;
            //    }
            //}
        }
        /// <summary>
        /// The function updates station name or number of slots by the user request
        /// </summary>
        /// <param name="ID">station ID</param>
        /// <param name="name">new station name</param>
        /// <param name="numOfSlots">new number of slots</param>
        public void UpdateStation(int ID, string name = null, int? numOfSlots = null)
        {
            int index = DataSource.Stations.FindIndex(item => item.StationID == ID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exist");
            Station tmp = DataSource.Stations[index];
            if (name != null)
            {
                tmp.Name = name;
                DataSource.Stations[index] = tmp;
            }
            if (numOfSlots != null)
            {
                int numOfFullSlots = DataSource.DroneCharges.FindAll(item => item.BaseStationID == ID).Count;//does it work??
                tmp.NumOfAvailableChargingSlots = (int)(numOfSlots - numOfFullSlots);
                DataSource.Stations[index] = tmp;
            }
        }
        /// <summary>
        /// The function updates parcel name or phone number by the user request
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <param name="name">new customer name</param>
        /// <param name="phone">new customer phone number</param>
        public void UpdateCustomer(int ID, string name = null, string phone = null)
        {
            int index = DataSource.Customers.FindIndex(item => item.CustomerID == ID);
            if (index < 0)
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
        /// <summary>
        /// The function updates the drone model
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="model">new drone model</param>
        public void UpdateDroneModel(int ID, string model)
        {
            int index = DataSource.Drones.FindIndex(item => item.DroneID == ID);
            if (index == -1)
                throw new ItemNotExistException("Drone does not exist");
            Drone tmp = DataSource.Drones[index];
            tmp.Model = model;
            DataSource.Drones[index] = tmp;
        }
        #endregion

        #region Display one item

        /// <summary>
        /// Return the wanted drone
        /// </summary>
        /// <param name="DroneID">The requested drone</param>
        public Drone DroneDisplay(int DroneID)
        {
            if (!DataSource.Drones.Exists(item => item.DroneID == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Drones.Find(item => item.DroneID == DroneID);
        }

        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        public Station StationDisplay(int StationID)
        {
            if (!DataSource.Stations.Exists(item => item.StationID == StationID))
                throw new ItemNotExistException("The station does not exists");
            return DataSource.Stations.Find(item => item.StationID == StationID);
        }

        /// <summary>
        /// Return the wanted customer
        /// </summary>
        /// <param name="CustomerID">The requested customer</param>
        public Customer CustomerDisplay(int CustomerID)
        {
            if (!DataSource.Customers.Exists(item => item.CustomerID == CustomerID))
                throw new ItemNotExistException("The customer does not exists");
            return DataSource.Customers.Find(item => item.CustomerID == CustomerID);
        }

        /// <summary>
        /// Return the wanted parcel
        /// </summary>
        /// <param name="ParcelID"> The requested parcel</param>
        public Parcel ParcelDisplay(int ParcelID)
        {
            if (!DataSource.Parcels.Exists(item => item.ParcelID == ParcelID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Parcels.Find(item => item.ParcelID == ParcelID);
        }
        #endregion

        #region Lists of items
        /// <summary>
        /// Returns all the drones in the list.
        /// </summary>
        public IEnumerable<Drone> ListDroneDisplay(Predicate<Drone> predicate = null)
        {
            List<Drone> ListOfDrones = new();
            foreach (Drone currentDrone in DataSource.Drones) { ListOfDrones.Add(currentDrone); }
            return ListOfDrones.FindAll(i => predicate == null ? true : predicate(i)); ;
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
        /// Returns all the drones in charging
        /// </summary>
        /// <returns>list of the drone in charging</returns>
        public IEnumerable<DroneCharge> ListOfDroneCharge(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharging = new();
            foreach (DroneCharge currentDrone in DataSource.DroneCharges)
            {
                droneCharging.Add(currentDrone);
            }
            return droneCharging.FindAll(i => predicate == null ? true : predicate(i)); ;
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


