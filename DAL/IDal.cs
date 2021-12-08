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
        #region Add
        /// <summary>
        /// Add a drone
        /// </summary>
        /// <param name="drone">The new drone that will be added to the list of drones</param>
        ///<exception cref="DalObject.AlreadyExistedItemException"></exception>
        public void Add(Drone drone);

        /// <summary>
        /// Add a station
        /// </summary>
        /// <param name="station">The new station that will be added to the list of stations</param>
        ///<exception cref="DalObject.AlreadyExistedItemException"></exception>
        ///
        public void Add(Station station);

        /// <summary>
        /// Adds a new parcel
        /// </summary>
        /// <param name="parcel">The new parcel that will be added to the list of parcels</param>
        public void Add(Parcel parcel);

        /// <summary>
        /// Add a customer
        /// </summary>
        /// <param name="customer">The new Customer that will be added to the list of customer</param>
        /// <exception cref="DalObject.AlreadyExistedItemException>"
        public void Add(Customer customer);
        #endregion

        #region Drone operations
        /// <summary>
        /// Assigns a drone to the parcel
        /// </summary>
        /// <param name="parcelID">The parcel that needs to be assigns</param>
        /// <param name="droneID">The drone that needs to be assigns</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void AssignParcelToDrone(int parcelID, int droneID);
        /// <summary>
        /// Collection of a requested parcel (by a drone)
        /// </summary>
        /// <param name="parcelID">The parcel that needs to be collected</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void CollectionOfParcelByDrone(int parcelID, int droneID);

        /// <summary>
        /// Delivery parcel to customer
        /// </summary>
        /// <param name="parcelID">The parcel that needs to be collected</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void DeliveryParcelToCustomer(int parcelID);
        /// <summary>
        /// Sending drone to charging base station
        /// </summary>
        /// <param name="droneID">the wanted drone</param>
        /// <param name="stationID">the wanted station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void SendingDroneToChargingBaseStation(int droneID, int stationID);
        /// <summary>
        /// Releasing a drone from a charging Base Station.
        /// </summary>
        /// <param name="droneID">The ID of the wanted drone</param>
        /// <param name="baseStationID">The ID of the wanted station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID);
        #endregion

        #region Updates
        /// <summary>
        /// Puts the updated drone on the list in IDAL
        /// </summary>
        /// <param name="drone">the update drone</param>
        void UpdateDrone(Drone drone);

        /// <summary>
        /// Puts the updated station on the list in IDAL
        /// </summary>
        /// <param name="station">the update station</param>
        /// <param name="numOfSlots">new number of slots</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void UpdateStation(Station station);

        /// <summary>
        /// Puts the updated customer on the list in IDAL
        /// </summary>
        /// <param name="customer">the update customer</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public void UpdateCustomer(int ID, string name = null, string phone = null);
        #endregion

        #region Get a single item
        /// <summary>
        /// Return the wanted drone
        /// </summary>
        /// <param name="droneID">The requested drone</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Drone GetDrone(int droneID);
        /// <summary>
        /// Return the wanted droneCharge
        /// </summary>
        /// <param name="droneID">The requested drone</param>
        /// <param name="baseStationId">The requested station</param>
        /// <returns>wanted droneCharge, the index that the drone is in</returns>
        /// <exception cref="IDAL.DO.ItemNotExistException"></exception>
        public (DroneCharge, int) GetDroneCharge(int droneID, int baseStationId);

        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="stationID">The requested station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Station GetStation(int stationID);

        /// <summary>
        /// Return the wanted customer
        /// </summary>
        /// <param name="customerID">The requested customer</param>
        /// <summary>
        /// Return the wanted station
        /// </summary>
        /// <param name="StationID">The requested station</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Customer GetCustomer(int customerID);

        /// <summary>
        /// Return the wanted parcel
        /// </summary>
        /// <param name="parcelID"> The requested parcel</param>
        /// <exception cref="DalObject.ItemNotExistException"></exception>
        public Parcel GetParcel(int parcelID);
        #endregion

        #region List of items
        /// <summary>
        /// Returns all the drones in the list.
        /// </summary>
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null);

        /// <summary>
        /// Returns all the customers in the list.
        /// </summary>
        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null);

        /// <summary>
        /// Returns all the station in the list
        /// </summary>
        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null);

        /// <summary>
        /// Returns all the parcel in the list
        /// </summary>
        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null);

        /// <summary>
        /// Returns all the drones in charging
        /// </summary>
        /// <returns>list of the drone in charging</returns>
        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null);

        /// <summary>
        /// requests power consumption by a drone
        /// </summary>
        /// <returns>returns an array of numbers of double type</returns>  
        #endregion
        public double[] ChargingDrone();
    }
}
