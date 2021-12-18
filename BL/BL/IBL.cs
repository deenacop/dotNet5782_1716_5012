using BO;
using System;
using System.Collections.Generic;

namespace BlApi
{
    public interface IBL
    {
        #region add
        /// <summary>
        /// Adds a station to the list of stations in the IDAL
        /// </summary>
        /// <param name="station">The wanted station</param>
        /// <exception cref="BlApi.BO.WrongIDException"></exception>
        /// <exception cref="BlApi.BO.UnlogicalLocationException"></exception>
        /// <exception cref="BlApi.BO.NegetiveException"></exception>
        /// <exception cref="BlApi.BO.AlreadyExistedItemException"></exception>
        void AddBaseStation(BaseStation station);

        /// <summary>
        /// Adds a customer to the list of customers in the IDAL
        /// </summary>
        /// <param name="customer">the wanted customer</param>
        /// <exception cref="BlApi.BO.WrongIDException"></exception>
        /// <exception cref="BlApi.BO.UnlogicalLocationException"></exception>
        /// <exception cref="BlApi.BO.AlreadyExistedItemException"></exception>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Adds a drone to the list of drones in the IDAL
        /// </summary>
        /// <param name="drone">The new drone that we asked to add</param>
        /// <param name="stationID">A station ID for a initial charge</param>
        /// <exception cref="BlApi.BO.WrongIDException"></exception>
        void AddDrone(Drone drone, int stationID);

        /// <summary>
        /// Adds a parcel to the list of parcels in the IDAL
        /// </summary>
        /// <param name="parcel">The wanted parcel</param>
        /// <exception cref="BlApi.BO.WrongIDException"></exception>
        /// <exception cref="BlApi.BO.WrongIDException"></exception>
        /// <exception cref="BlApi.BO.AlreadyExistedItemException"></exception>
        void AddParcel(Parcel parcel);
        #endregion

        #region drone operations

        /// <summary>
        /// Assings a drone to a parcel
        /// </summary>
        /// <param name="ID">The drone to assign</param>
        /// <exception cref="BlApi.BO.WorngStatusException"></exception>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void AssignParcelToDrone(Drone drone);
        /// <summary>
        /// Update a drone to collect a parcel
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="BlApi.BO.WorngStatusException"></exception>
        /// <exception cref="BlApi.BO.WorngStatusException"></exception>        
        void CollectionParcelByDrone(Drone drone);
        /// <summary>
        /// The function send a specific drone to the closest available station
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        /// <exception cref="BlApi.BO.NotEnoughBatteryException"></exception>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void SendDroneToCharge(Drone drone);

        /// <summary>
        /// Delivers a parcel to the receiver
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="BlApi.BO.WorngStatusException"></exception>
        void DeliveryParcelByDrone(Drone drone);
        /// <summary>
        /// This function is relaesing a drone
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="minuteInCharge">the amount of time(by minute) that the drone was in charge</param>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        /// <exception cref="BlApi.BO.NotEnoughBatteryException"></exception>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void ReleasingDroneFromBaseStation(Drone drone);
        #endregion

        #region get a single item
        /// <summary>
        /// Display one BL drone
        /// </summary>
        /// <param name="droneID">drone ID</param>
        /// <returns>The wanted drone</returns>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        Drone GetDrone(int droneID);
        /// <summary>
        /// Display parcel
        /// </summary>
        /// <param name="ID">The ID of the wanted parcel</param>
        /// <returns>The wanted parcel</returns>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        Parcel GetParcel(int ID);

        /// <summary>
        /// Display the base station
        /// </summary>
        /// <param name="StationID">The ID of the wanted station</param>
        /// <returns>Returns the wanted base station</returns>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        BaseStation GetBaseStation(int StationID);
        /// <summary>
        /// Display one customer
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <returns>The wanted customer</returns>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        Customer GetCustomer(int ID);
        #endregion

        #region update
        /// <summary>
        /// update drone model and weight
        /// </summary>
        /// <param name="drone">wanted drone</param>
        /// <exception cref="ItemNotExistException"></exception>
        /// <exception cref="WrongInputException"></exception>
        /// <exception cref="ItemNotExistException"></exception>
        void UpdateDrone(Drone drone);
        /// <summary>
        /// The function updates the customer name or phone number, by the users request
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <param name="name">customer name</param>
        /// <param name="phone">customer phone</param>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Updates the station
        /// </summary>
        /// <param name="ID">The ID of the wanted station</param>
        /// <param name="name">The name of the wanted station</param>
        /// <param name="numOfSlots">The numOfSlots of the wanted station</param>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void UpdateStation(BaseStation baseStation);
        #endregion

        #region get list of items
        /// <summary>
        /// Displays the list of the customerToList
        /// </summary>
        /// <returns>The list of the customer</returns>
        IEnumerable<CustomerToList> GetListCustomer(Predicate<CustomerToList> predicate = null);

        /// <summary>
        /// Displays the list of drones
        /// </summary>
        /// <returns>The list of drones</returns>     
        IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);

        /// <summary>
        /// Displays the list of BL base station with available slots
        /// </summary>
        /// <returns>The list of BL base ststion with available slots</returns>  
        IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null);

        /// <summary>
        /// Displays the list of parcels
        /// </summary>
        /// <returns>The list of parceld</returns>
        IEnumerable<ParcelToList> GetListParcel(Predicate<ParcelToList> predicate = null);
        #endregion

    }
}