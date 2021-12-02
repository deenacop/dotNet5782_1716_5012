using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        /// <summary>
        /// Adds a new drone
        /// </summary>
        /// <param name="NewDrone">The new drone that will be added to the list of drones</param>
        ///<exception cref="DalObject.AlreadyExistedItemException"></exception>
        public void Add(Drone NewDrone);

        /// <summary>
        /// Adds a new station
        /// </summary>
        /// <param name="NewStation">The new station that will be added to the list of stations</param>
        ///<exception cref="DalObject.AlreadyExistedItemException"></exception>
        ///
        public void Add(Station NewStation);

        /// <summary>
        /// Adds a new parcel
        /// </summary>
        /// <param name="NewParcel">The new parcel that will be added to the list of parcels</param>
        public void Add(Parcel NewParcel);

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="NewCustomer">The new Customer that will be added to the list of customer</param>
        /// <exception cref="DalObject.AlreadyExistedItemException>"
        public void Add(Customer NewCustomer);

        /// <summary>
        /// Assigns a drone to the parcel
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be assigns</param>
        /// <param name="DroneID">The drone's ID that needs to be assigns</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void AssignParcelToDrone(int ParcelID, int DroneID);
        void UpdateDrone(Drone drone);

        /// <summary>
        /// Collection of a requested parcel (by a drone)
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <summary>
        /// Delivery parcil to customer
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// 
        public void CollectionOfParcelByDrone(int ParcelID, int DroneID);

        /// <summary>
        /// Delivery parcil to customer
        /// </summary>
        /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void DeliveryParcelToCustomer(int ParcelID);

        /// <summary>
        /// Sending drone to charging base station
        /// </summary>
        /// <param name="DroneID">the wanted drone</param>
        /// <param name="StationID">the wanted station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void SendingDroneToChargingBaseStation(int DroneID, int StationID);

        /// <summary>
        /// Releasing a drone from a charging Base Station.
        /// </summary>
        /// <param name="DroneID">The ID of the wanted drone</param>
        /// <param name="BaseStationID">The ID of the wanted station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID);

        /// <summary>
        /// The function updates station name or number of slots by the user request
        /// </summary>
        /// <param name="ID">station ID</param>
        /// <param name="name">new station name</param>
        /// <param name="numOfSlots">new number of slots</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void UpdateStation(int ID, string name, int numOfSlots);

        /// <summary>
        /// The function updates customer name or phone number by the user request
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <param name="name">new customer name</param>
        /// <param name="phone">new customer phone number</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void UpdateCustomer(int ID, string name = null, string phone = null);

        /// <summary>
        /// The function updates the drone model
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="model">new drone model</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void UpdateDroneModel(int ID, string model);

        /// <summary>
        /// Return the wanted drone
        /// </summary>
        /// <param name="DroneID">The requested drone</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Drone DroneDisplay(int DroneID);

        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Station GetStation(int StationID);

        /// <summary>
        /// Return the wanted customer
        /// </summary>
        /// <param name="CustomerID">The requested customer</param>
        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Customer CustomerDisplay(int CustomerID);

        /// <summary>
        /// Return the wanted parcel
        /// </summary>
        /// <param name="ParcelID"> The requested parcel</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Parcel ParcelDisplay(int ParcelID);

        /// <summary>
        /// Returns all the drones in the list.
        /// </summary>
        public IEnumerable<Drone> ListDroneDisplay(Predicate<Drone> predicate = null);

        /// <summary>
        /// Returns all the customers in the list.
        /// </summary>
        public IEnumerable<Customer> ListCustomerDisplay(Predicate<Customer> predicate = null);

        /// <summary>
        /// Returns all the station in the list
        /// </summary>
        public IEnumerable<Station> ListStationDisplay(Predicate<Station> predicate = null);

        /// <summary>
        /// Returns all the parcel in the list
        /// </summary>
        public IEnumerable<Parcel> ListParcelDisplay(Predicate<Parcel> predicate = null);

        /// <summary>
        /// Returns all the drones in charging
        /// </summary>
        /// <returns>list of the drone in charging</returns>
        public IEnumerable<DroneCharge> ListOfDroneCharge(Predicate<DroneCharge> predicate = null);

        /// <summary>
        /// requests power consumption by a drone
        /// </summary>
        /// <returns>returns an array of numbers of double type</returns>     
        public double[] ChargingDrone();
    }
}
