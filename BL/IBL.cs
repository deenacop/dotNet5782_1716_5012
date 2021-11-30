using IBL.BO;
using System;
using System.Collections.Generic;

namespace IBL
{
    public interface IBL
    {
        /// <summary>
        /// Adds a station to the list of stations in the IDAL
        /// </summary>
        /// <param name="station">The wanted station</param>
        /// <exception cref="IBL.BO.WrongIDException"></exception>
        /// <exception cref="IBL.BO.UnlogicalLocationException"></exception>
        /// <exception cref="IBL.BO.NegetiveException"></exception>
        /// <exception cref="IBL.BO.AlreadyExistedItemException"></exception>
        void AddBaseStation(BaseStation station);

        /// <summary>
        /// Adds a customer to the list of customers in the IDAL
        /// </summary>
        /// <param name="customer">the wanted customer</param>
        /// <exception cref="IBL.BO.WrongIDException"></exception>
        /// <exception cref="IBL.BO.UnlogicalLocationException"></exception>
        /// <exception cref="IBL.BO.AlreadyExistedItemException"></exception>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Adds a drone to the list of drones in the IDAL
        /// </summary>
        /// <param name="drone">The new drone that we asked to add</param>
        /// <param name="stationID">A station ID for a initial charge</param>
        /// <exception cref="IBL.BO.WrongIDException"></exception>
        void AddDrone(DroneToList drone, int stationID);

        /// <summary>
        /// Adds a parcel to the list of parcels in the IDAL
        /// </summary>
        /// <param name="parcel">The wanted parcel</param>
        /// <exception cref="IBL.BO.WrongIDException"></exception>
        /// <exception cref="IBL.BO.WrongIDException"></exception>
        /// <exception cref="IBL.BO.AlreadyExistedItemException"></exception>
        void AddParcel(Parcel parcel);

        /// <summary>
        /// Assings a drone to a parcel
        /// </summary>
        /// <param name="ID">The drone to assign</param>
        /// <exception cref="IBL.BO.WorngStatusException"></exception>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void AssignParcelToDrone(int ID);

        /// <summary>
        /// Display the base station
        /// </summary>
        /// <param name="StationID">The ID of the wanted station</param>
        /// <returns>Returns the wanted base station</returns>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        BaseStation BaseStationDisplay(int StationID);

        /// <summary>
        /// Update a drone to collect a parcel
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="IBL.BO.WorngStatusException"></exception>
        /// <exception cref="IBL.BO.WorngStatusException"></exception>        
        void CollectionOfParcelByDrone(int ID);

        /// <summary>
        /// Display one customer
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <returns>The wanted customer</returns>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        Customer CustomerDisplay(int ID);

        /// <summary>
        /// Display one BL drone
        /// </summary>
        /// <param name="droneID">drone ID</param>
        /// <returns>The wanted drone</returns>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        Drone DisplayDrone(int droneID);

        /// <summary>
        /// Displays the list of the customerToList
        /// </summary>
        /// <returns>The list of the customer</returns>
        IEnumerable<CustomerToList> ListCustomerDisplay();

        /// <summary>
        /// Displays the list of drones
        /// </summary>
        /// <returns>The list of drones</returns>     
        IEnumerable<DroneToList> ListDroneDisplay();

        /// <summary>
        /// Displays the list of BL base station with available slots
        /// </summary>
        /// <returns>The list of BL base ststion with available slots</returns>  
        IEnumerable<BaseStationToList> ListBaseStationlDisplay(Predicate<BaseStationToList> predicate = null);

        /// <summary>
        /// Displays the list of parcels
        /// </summary>
        /// <returns>The list of parceld</returns>
        IEnumerable<ParcelToList> ListParcelDisplay(Predicate<ParcelToList> predicate = null);

        /// <summary>
        /// Display parcel
        /// </summary>
        /// <param name="ID">The ID of the wanted parcel</param>
        /// <returns>The wanted parcel</returns>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        Parcel ParcelDisplay(int ID);

        /// <summary>
        /// This function is relaesing a drone
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="minuteInCharge">the amount of time(by minute) that the drone was in charge</param>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        /// <exception cref="IBL.BO.NotEnoughBatteryException"></exception>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void ReleasingDroneFromBaseStation(int ID, int minuteInCharge);

        /// <summary>
        /// The function send a specific drone to the closest available station
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        /// <exception cref="IBL.BO.NotEnoughBatteryException"></exception>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void SendDroneToCharge(int ID);

        /// <summary>
        /// The function updates the customer name or phone number, by the users request
        /// </summary>
        /// <param name="ID">customer ID</param>
        /// <param name="name">customer name</param>
        /// <param name="phone">customer phone</param>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void UpdateCustomer(int ID, string name = null, string phone = null);

        /// <summary>
        /// Update the drone model
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <param name="model">new midel</param>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void UpdateDroneModel(int ID, string model);

        /// <summary>
        /// Updates the station
        /// </summary>
        /// <param name="ID">The ID of the wanted station</param>
        /// <param name="name">The name of the wanted station</param>
        /// <param name="numOfSlots">The numOfSlots of the wanted station</param>
        /// <exception cref="IBL.BO.ItemNotExistException"></exception>
        void UpdateStation(int ID, string name, int numOfSlots );

        /// <summary>
        /// Delivers a parcel to the receiver
        /// </summary>
        /// <param name="ID">drone ID</param>
        /// <exception cref="IBL.BO.WorngStatusException"></exception>
        void DeliveryParcelByDrone(int ID);
    }
}