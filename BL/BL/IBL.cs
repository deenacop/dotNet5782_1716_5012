using BO;
using System;
using System.Collections.Generic;

namespace BlApi
{
    public interface IBL
    {
        /// <summary>
        /// Fution that starts simulation
        /// </summary>
        /// <param name="droneId">drone to start simulation on</param>
        /// <param name="action">delegate to update the display</param>
        /// <param name="stop">delegate to stop simulation</param>
        public void StartSimulation(int droneId, Func<bool> func, Action action);

        /// <summary>
        /// The function regulates the battery and takes into account the weight and battery consumption accordingly
        /// </summary>
        /// <param name="battery">the battery that need to be set</param>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>the drones battery</returns>
        public int setBattery(int battery, double distance, WeightCategories weight);

        /// <summary>
        /// A function that calculates the distance between two coordinates on the map
        /// </summary>
        /// <param name="from">The location from were we want to calculate the distance</param>
        /// <param name="to">The location to were we want to calculate</param>
        /// <returns>returns the distance</returns>
        public double DistanceCalculation(Location from, Location to);

        /// <summary>
        /// Finds the minimum distance from a station to a location
        /// </summary>
        /// <param name="BaseStationListBL">list of stationBL</param>
        /// <param name="location">current location</param>
        /// <returns>Item 1:location/Item 2:distance/Item 3:StationID</returns>
        public (Location, double, int) MinDistanceLocation(List<BaseStation> BaseStationListBL, Location location);

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

        /// <summary>
        /// Add a user to the list of users in the IDAL
        /// </summary>
        /// <param name="user">a user</param>
        /// <exception cref="BO.ItemAlreadyExistsException"></exception>
        void AddUser(User user);
        #endregion

        #region Remove

        /// <summary>
        /// Removes the drone from the list
        /// </summary>
        /// <param name="drone">the drone that needs to be removed</param>
        /// <exception cref="BO.ItemCouldNotBeRemoved"></exception>
        /// <exception cref="BO.ItemNotExistException"></exception>
        void RemoveDrone(Drone drone);

        /// <summary>
        /// Removes the parcel from the list
        /// </summary>
        /// <param name="parcel">the parcel that needs to be removed</param>
        /// <exception cref="BO.ItemCouldNotBeRemoved"></exception>
        /// <exception cref="BO.ItemNotExistException"></exception>
        void RemoveParcel(Parcel parcel);

        /// <summary>
        /// Removes the customer from the list
        /// </summary>
        /// <param name="customer">the customer that needs to be removed</param>
        /// <exception cref="BO.ItemNotExistException"></exception>
        void RemoveCustomer(Customer customer);

        /// <summary>
        /// Removes the baseStation from the list
        /// </summary>
        /// <param name="baseStation">the baseStation that needs to be removed</param>
        /// <exception cref="BO.ItemCouldNotBeRemoved"></exception>
        /// <exception cref="BO.ItemNotExistException"></exception>
        void RemoveStation(BaseStation baseStation);

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
        Customer GetCustomer(string ID);

        /// <summary>
        /// Display one drone in charge
        /// </summary>
        /// <param name="id">drone ID</param>
        /// <param name="stationId">station id</param>
        /// <returns>the wanted drone</returns>
        /// /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        public DroneInCharging GetDroneInCharge(int id, int stationId);

        /// <summary>
        /// Display one user
        /// </summary>
        /// <param name="mail">user mail</param>
        /// <returns>The wanted user</returns>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        User GetUser(string mail);
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
        /// <summary>
        /// Updates the User
        /// </summary>
        /// <param name="mail">the user mail</param>
        /// <exception cref="BlApi.BO.ItemNotExistException"></exception>
        void updateUser(string mail, string password);

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
        /// <summary>
        /// Displays the list of BL users
        /// </summary>
        /// <returns>The list of users</returns>
        IEnumerable<User> GetListUsers(Predicate<User> predicate = null);

        /// <summary>
        /// Displays the list of BL drone in charging
        /// </summary>
        /// <returns>The list of drones</returns>
        public IEnumerable<DroneInCharging> GetDroneInChargingList(Predicate<DroneInCharging> predicate = null);

        #endregion

        #region recover
        /// <summary>
        /// this function adds a drone that already has been removed 
        /// </summary>
        /// <param name="drone">the drone that is added</param>
        /// <param name="stationID">the station in which he is charging</param>
        /// <exception cref="BO.ItemNotExistException"></exception>
        public void DroneRecover(Drone drone, int stationID);

        /// <summary>
        /// this function adds a customer that already has been removed 
        /// </summary>
        /// <param name="customer">the customer that is added</param>
        /// <exception cref="BO.ItemNotExistException"></exception>
        public void CustonerRecover(Customer customer);

        /// <summary>
        /// this function adds a station that already has been removed 
        /// </summary>
        /// <param name="station">the station that is added</param>
        /// <exception cref="BO.ItemNotExistException"></exception>
        public void StationRecover(BaseStation station);
        #endregion

    }
}