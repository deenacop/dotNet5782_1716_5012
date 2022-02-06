using System;
using System.Linq;
using System.Collections.Generic;
using DO;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal
{
    internal sealed class DalObject : DalApi.IDal
    {
        #region singelton

        private static readonly Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject()); //Lazy initialization of an object means that its creation is deferred until it is first used.

        internal static DalObject Instance { get { return instance.Value; } }
        static DalObject() { }
        private DalObject() { DataSource.Initialize(); }//ctor

        #endregion

        #region Add
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Add(Drone drone)
        {
            try
            {
                //checks if the drone exists and if not throws an exception
                Drone isExistDrone = DataSource.Drones.First(i => i.Id == drone.Id);
                if (isExistDrone.IsRemoved == true)//the drone exist but have been removed
                    return false;
                else
                    throw new AlreadyExistedItemException("The drone already exists");
            }
            catch (InvalidOperationException)//if isExistDrone is null its mean that the drone that we want to add isnt exist
            {
                DataSource.Drones.Add(drone);
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Add(Station station)
        {
            try
            {
                //checks if the station exists and if not throws an exception
                Station isExistStation = DataSource.Stations.First(i => i.Id == station.Id);
                if (isExistStation.IsRemoved == true)//the drone exist but have been removed
                    return false;
                else
                    throw new AlreadyExistedItemException("The station already exists");
                //station.IsRemoved = false;
            }
            catch (InvalidOperationException)//if isExistStation is null its mean that the drone that we want to add isnt exist
            {
                DataSource.Stations.Add(station);
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Add(Parcel parcel)
        {//the parcel is struct and thats why,
         //when we update the parcel's id- it doesnt send it to the BL because its not by ref.
         // so we sent to the BL the id, that the parcel of BO also will have the id.
            parcel.Id = DataSource.Config.RunnerIDNumParcels++;
            //parcel.IsRemoved = false;
            DataSource.Parcels.Add(parcel);
            return parcel.Id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Add(Customer customer)
        {//For us the manager can add a non-user client !! That is, a customer who does not have the option to log in.
         //Therefore we will not add it as a user either
         //checks if the customer exists and if not throws an exception
            try
            {
                //checks if the customer exists and if not throws an exception
                Customer isExistCustomer = DataSource.Customers.First(i => i.Id == customer.Id);
                if (isExistCustomer.IsRemoved == true)//the customer exist but have been removed
                    return false;
                else
                    throw new AlreadyExistedItemException("The customer already exists");
            }
            catch (InvalidOperationException)//if isExistCustomer is null its mean that the drone that we want to add isnt exist
            {
                DataSource.Customers.Add(customer);
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(User user)
        {
            if (DataSource.Users.Exists(i => i.EmailAddress == user.EmailAddress || i.Id == user.Id))
                throw new AlreadyExistedItemException("The user already exists");
            Customer customer = user.ConverUserToCustomer();
            //Our program is based on the fact that every user is also a customer.
            //Therefore, when we add a user, we must also add him as a customer.
            //But first we need to check -Is the user already registered as a customer ? 
            if (!(DataSource.Customers.Exists(i => i.Id == user.Id)))
                DataSource.Customers.Add(customer);
            DataSource.Users.Add(user);
        }
        #endregion

        #region Remove

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(int id)
        {
            //checks if the drone exists and if not throws an exception
            int index = DataSource.Drones.FindIndex(item => item.Id == id);
            if (index == -1)//not exist
                throw new ItemNotExistException("Drone does not exist");
            Drone drone = DataSource.Drones[index];
            drone.IsRemoved = true;
            DataSource.Drones[index] = drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(int id)
        {
            //checks if the station exists and if not throws an exception
            int index = DataSource.Stations.FindIndex(item => item.Id == id);
            if (index == -1)//not exist
                throw new ItemNotExistException("Station does not exist");
            Station station = DataSource.Stations[index];
            station.IsRemoved = true;
            DataSource.Stations[index] = station;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            //checks if the parcel exists and if not throws an exception
            int index = DataSource.Parcels.FindIndex(item => item.Id == id);
            if (index == -1)//not exist
                throw new ItemNotExistException("parcel does not exist");
            Parcel parcel = DataSource.Parcels[index];
            parcel.IsRemoved = true;
            DataSource.Parcels[index] = parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(int id)
        {
            //checks if the customer exists and if not throws an exception
            int index = DataSource.Customers.FindIndex(item => item.Id == id);
            if (index == -1)//not exist
                throw new ItemNotExistException("customer does not exist");
            Customer customer = DataSource.Customers[index];
            customer.IsRemoved = true;
            DataSource.Customers[index] = customer;
            //Because our program is based on the fact that every user must also be a customer. 
            //If we delete a customer, we must make sure he was not a user in the first place. 
            //And if he was also a user - delete it completely from the list
            index = DataSource.Users.FindIndex(i => i.Id == id);
            if (index != -1)
                DataSource.Users[index].IsRemoved = true;
        }

        #endregion

        #region Drone operations

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignParcelToDrone(int parcelID, int droneID)
        {
            //checks if the drone exists and if not throws an exception
            int index = DataSource.Drones.FindIndex(i => i.Id == droneID);
            if (index == -1 || DataSource.Drones[index].IsRemoved)//not found
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);
            if (index == -1)//not found
                throw new ItemNotExistException("The parcel does not exists");
            //updates the parcel
            Parcel parcel = DataSource.Parcels[index];
            parcel.MyDroneID = droneID;
            parcel.Scheduled = DateTime.Now;//updates the scheduled time
            DataSource.Parcels[index] = parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectionOfParcelByDrone(int parcelID, int droneID)
        {
            //checks if the drone exists and if not throws an exception
            int index = DataSource.Drones.FindIndex(i => i.Id == droneID);
            if (index == -1)//not found
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);
            if (index == -1)//not found
                throw new ItemNotExistException("The parcel does not exists");
            //updates the parcel
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = droneID;
            tmp.PickUp = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryParcelToCustomer(int parcelID)
        {
            int index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);//finds the parcel
            if (index == -1)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = DataSource.Parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            DataSource.Parcels[index] = tmp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            //checks if the drone exists and if not throws an exception
            int index = DataSource.Drones.FindIndex(i => i.Id == droneID);
            if (index == -1 || DataSource.Drones[index].IsRemoved)//not found
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            index = DataSource.Stations.FindIndex(i => i.Id == stationID);
            if (index == -1 || DataSource.Stations[index].IsRemoved)//not found
                throw new ItemNotExistException("The station does not exists");
            Station tmp = DataSource.Stations[index];
            if (tmp.NumOfAvailableChargingSlots == 0)
                throw new NegetiveException("There is no available slots in this current station. Try a different station");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new()
            {
                Id = droneID,
                BaseStationID = stationID,
                EnterToChargBase = DateTime.Now
            };
            //adds
            DataSource.DroneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots

            tmp.NumOfAvailableChargingSlots--;
            DataSource.Stations[index] = tmp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID)
        {
            //checks if the drone and station exists and if not throws an exception
            int index = DataSource.Drones.FindIndex(i => i.Id == droneID);
            if (index == -1 || DataSource.Drones[index].IsRemoved)//not found
                throw new ItemNotExistException("The drone does not exists");
            index = DataSource.Stations.FindIndex(i => i.Id == baseStationID);
            if (index == -1)//not found
                throw new ItemNotExistException("The station does not exists");
            if (!DataSource.Stations[index].IsRemoved)
            {
                Station station = DataSource.Stations[index];
                station.NumOfAvailableChargingSlots++;
                DataSource.Stations[index] = station;
            }
            int indexDC = DataSource.DroneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.Id == droneID);//finds index where drone is
            if (indexDC == -1)//checks if drone exists
                throw new ItemNotExistException("The drone does not exist.\n");
            DroneCharge drone = DataSource.DroneCharges[indexDC];
            drone.FinishedRecharging = DateTime.Now; ;//delete the drone from the list of the drone charge
            drone.IsRemoved = true;
            DataSource.DroneCharges[indexDC] = drone;

        }
        #endregion

        #region Updates

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(item => item.Id == drone.Id);
            if (index == -1 || drone.IsRemoved)//not exist
                throw new ItemNotExistException("Drone does not exist");
            DataSource.Drones[index] = drone;
        }
        public void UpdateStation(Station station)
        {
            int index = DataSource.Stations.FindIndex(i => i.Id == station.Id);
            if (index == -1 || station.IsRemoved)//not exist
                throw new ItemNotExistException("The station does not exist");
            DataSource.Stations[index] = station;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int Id, string name = null, string phone = null, double lon = 0, double lat = 0)

        {
            int index = DataSource.Customers.FindIndex(item => item.Id == Id);
            if (index == -1 || DataSource.Customers[index].IsRemoved)//not exist
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateUser(string mail, string password)
        {
            int index = DataSource.Users.FindIndex(i => i.EmailAddress == mail);
            if (index == -1)//not exist
                throw new ItemNotExistException("The user does not exsit");
            User tmp = DataSource.Users[index];
            tmp.Password = password;
            DataSource.Users[index] = tmp;
        }
        #endregion

        #region Get item

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneID, int baseStationId)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.Id == droneID && i.BaseStationID == baseStationId);//finds the drone charge
            if (index == -1)//not found
                throw new ItemNotExistException("the drone does not exist in the wanted base station");
            return DataSource.DroneCharges[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneID)
        {
            int index = DataSource.Drones.FindIndex(i => i.Id == droneID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Drones[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            int index = DataSource.Stations.FindIndex(i => i.Id == stationID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The station does not exists");
            return DataSource.Stations[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {

            int index = DataSource.Customers.FindIndex(i => i.Id == customerID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The customer does not exists");
            return DataSource.Customers[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelID)
        {
            int index = DataSource.Parcels.FindIndex(i => i.Id == parcelID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The parcel does not exists");
            return DataSource.Parcels[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string mail)
        {
            int index = DataSource.Users.FindIndex(i => i.EmailAddress == mail);
            if (index == -1)//not exist
                throw new ItemNotExistException("The parcel does not exists");
            return DataSource.Users[index];
        }
        #endregion

        #region Lists of items

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null)
            => DataSource.Drones.FindAll(i => predicate == null ? true : predicate(i));


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null)
            => DataSource.Customers.FindAll(i => predicate == null ? true : predicate(i));

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null)
            => DataSource.Stations.FindAll(i => predicate == null ? true : predicate(i));

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null)
            => DataSource.Parcels.FindAll(i => predicate == null ? true : predicate(i));

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null)
            => DataSource.DroneCharges.FindAll(i => predicate == null ? true : predicate(i));

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null)
            => DataSource.Users.FindAll(i => predicate == null ? true : predicate(i));
        #endregion

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] ChargingDrone()
            => new double[] { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };
    }
}



