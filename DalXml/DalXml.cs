using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    internal sealed class DalXml : IDal
    {
        private static string DroneXml = @"DroneXml.xml";
        private static string ParcelXml = @"ParcelXml.xml";
        private static string StationXml = @"StationXml.xml";
        private static string DroneChargeXml = @"DroneChargeXml.xml";
        private static string CustomerXml = @"CustomerXml.xml";
        #region singelton
        internal static DalXml instance { get { return Instance.Value; } }
        private static readonly Lazy<DalXml> Instance = new Lazy<DalXml>(() => new DalXml()); //Lazy initialization of an object means that its creation is deferred until it is first used.
        static DalXml() { }
        private DalXml() { }//ctor

        #endregion

        #region Add
        public void Add(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //checks if the drone exists and if not throws an exception
            if (drones.Exists(i => i.Id == drone.Id))
                throw new AlreadyExistedItemException("The drone already exists");
            drones.Add(drone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void Add(Station station)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer < Station >(StationXml);
            //checks if the station exists and if not throws an exception
            if (stations.Exists(i => i.Id == station.Id))
                throw new AlreadyExistedItemException("The station already exists");
            stations.Add(station);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public int Add(Parcel parcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            parcel.Id = DataSource.Config.RunnerIDNumParcels++;
            parcels.Add(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            return DataSource.Config.RunnerIDNumParcels;
        }

        public void Add(Customer customer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            //checks if the customer exists and if not throws an exception
            if (customers.Exists(i => i.Id == customer.Id))
                throw new AlreadyExistedItemException("The customer already exists");
            customers.Add(customer);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }
        public void Add(User user)
        {
            if (DataSource.Users.Exists(i => i.EmailAddress == user.EmailAddress))
                throw new AlreadyExistedItemException("The user already exists");
            DataSource.Users.Add(user);
        }
        #endregion

        #region Remove
        public void Remove(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //checks if the drone exists and if not throws an exception
            if (drones.Exists(i => i.Id == drone.Id))
                throw new AlreadyExistedItemException("The drone already exists");
            drones.Remove(drone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void Remove(Station station)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the station exists and if not throws an exception
            if (stations.Exists(i => i.Id == station.Id))
                throw new AlreadyExistedItemException("The station already exists");
            stations.Remove(station);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void Remove(Parcel parcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            parcel.Id = DataSource.Config.RunnerIDNumParcels++;
            parcels.Remove(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void Remove(Customer customer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            //checks if the customer exists and if not throws an exception
            if (customers.Exists(i => i.Id == customer.Id))
                throw new AlreadyExistedItemException("The customer already exists");
            customers.Remove(customer);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }
        public void Remove(User user)
        {
            if (DataSource.Users.Exists(i => i.EmailAddress == user.EmailAddress))
                throw new AlreadyExistedItemException("The user already exists");
            DataSource.Users.Remove(user);
        }
        #endregion


        #region Drone operations
        public void AssignParcelToDrone(int parcelID, int droneID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //checks if the drone exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = parcels.FindIndex(i => i.Id == parcelID);
            if (index == -1)//not found
                throw new ItemNotExistException("The station does not exists");
            //updates the parcel
            Parcel parcel = parcels[index];
            parcel.MyDroneID = droneID;
            parcel.Scheduled = DateTime.Now;//updates the scheduled time
            parcels[index] = parcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void CollectionOfParcelByDrone(int parcelID, int droneID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //checks if the drone exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = parcels.FindIndex(i => i.Id == parcelID);
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            //updates the parcel
            Parcel tmp = parcels[index];
            tmp.MyDroneID = droneID;
            tmp.PickUp = DateTime.Now;
            parcels[index] = tmp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void DeliveryParcelToCustomer(int parcelID)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(i => i.Id == parcelID);//finds the parcel
            if (index < 0)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            parcels[index] = tmp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
       
        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            XElement element = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the drone exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = stations.FindIndex(i => i.Id == stationID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            
            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.Id = droneID;
            ChargingDroneBattery.BaseStationID = stationID;
            ChargingDroneBattery.EnterToChargBase = DateTime.Now;
            ChargingDroneBattery.FinishedRecharging = null;

            XElement droneCharge = new XElement("DroneCharge", new XElement("Id", ChargingDroneBattery.Id),
                new XElement("BaseStationID", ChargingDroneBattery.BaseStationID), new XElement("EnterToChargBase", ChargingDroneBattery.EnterToChargBase),
                new XElement("FinishedRecharging", ChargingDroneBattery.FinishedRecharging));
            //adds
            element.Add(droneCharge);
            //up dates the number of available charging slots
            Station tmp = stations[index];
            tmp.NumOfAvailableChargingSlots--;
            stations[index] = tmp;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        //??????????
        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID)
        {
            XElement element = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the drone and station exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            int index = stations.FindIndex(i => i.Id == baseStationID);
            if (index == -1)
                throw new ItemNotExistException("The station does not exists");
            
            Station station = stations[index];
            station.NumOfAvailableChargingSlots++;
            stations[index] = station;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
            DroneCharge drone = GetDroneCharge(droneID, baseStationID).Item1;
            XElement _drone = (from p in element.Elements()
                               where p.Element("Id").Value == drone.Id.ToString()
                               select p).FirstOrDefault();

            index = GetDroneCharge(droneID, baseStationID).Item2;//get the specific index of the wanted dronecharge
            drone.FinishedRecharging = DateTime.Now; ;//delete the drone from the list of the drone charge
            DataSource.DroneCharges[index] = drone;
            _drone.Element("FinishedRecharging").Value = drone.FinishedRecharging.ToString();
            XMLTools.SaveListToXMLElement(element, DroneChargeXml);

        }
        #endregion

        #region Updates

        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(item => item.Id == drone.Id);
            if (index == -1)//not exist
                throw new ItemNotExistException("Drone does not exist");
            drones[index] = drone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);

        }
        public void UpdateStation(Station station)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int index = stations.FindIndex(i => i.Id == station.Id);
            if (index == -1)//not exist
                throw new ItemNotExistException("The station does not exist");
            stations[index] = station;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);

        }
        public void UpdateCustomer(int ID, string name = null, string phone = null)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int index = customers.FindIndex(item => item.Id == ID);
            if (index == -1)//not exist
                throw new ItemNotExistException("The customer does not exsit");
            Customer tmp = customers[index];
            if (name != null)
            {
                tmp.Name = name;
                customers[index] = tmp;
            }
            if (phone != null)
            {
                tmp.PhoneNumber = phone;
                customers[index] = tmp;
            }
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }
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
        public (DroneCharge, int) GetDroneCharge(int droneID, int baseStationId)
        {
            IEnumerable<XElement> elements = XMLTools.LoadListFromXMLElement(DroneChargeXml).Elements();
            XElement element = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            DroneCharge _drone = (from drone in element.Elements()
                                  where drone.Element("Id").Value == droneID.ToString()
                                  select new DroneCharge() {
                                      Id=int.Parse(drone.Element("Id").Value),
                                      BaseStationID=int.Parse(drone.Element("BaseStationID").Value),
                                      EnterToChargBase=DateTime.Parse(drone.Element("EnterToChargBase").Value),
                                      FinishedRecharging=DateTime.Parse(drone.Element("FinishedRecharging").Value)

                                  }).FirstOrDefault();
            int index = DataSource.DroneCharges.FindIndex(i => i.Id == droneID && i.BaseStationID == baseStationId);//finds the drone charge
            //if (index < 0)//not found
            //    throw new ItemNotExistException("the drone does not exist in the wanted base station");
            //return (DataSource.DroneCharges[index], index);
            //if(_drone)
                //throw new ItemNotExistException("the drone does not exist in the wanted base station");
            return(_drone,index);
        }
        public Drone GetDrone(int droneID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            return drones.Find(i => i.Id == droneID);
        }

        public Station GetStation(int stationID)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (!stations.Exists(i => i.Id == stationID))
                throw new ItemNotExistException("The station does not exists");
            return stations.Find(i => i.Id == stationID);
        }

        public Customer GetCustomer(int customerID)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (!customers.Exists(i => i.Id == customerID))
                throw new ItemNotExistException("The customer does not exists");
            return customers.Find(i => i.Id == customerID);
        }

        public Parcel GetParcel(int parcelID)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (!parcels.Exists(i => i.Id == parcelID))
                throw new ItemNotExistException("The parcel does not exists");
            return parcels.Find(i => i.Id == parcelID);
        }
        public User GetUser(string mail)
        {
            if (!DataSource.Users.Exists(i => i.EmailAddress == mail))
                throw new ItemNotExistException("The user does not exists");
            return DataSource.Users.Find(i => i.EmailAddress == mail);
        }
        #endregion

        #region Lists of items
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from item in drones
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null)
        {
            //List<Customer> ListOfCustomers = new();
            //foreach (Customer currentCostomer in DataSource.Customers) { ListOfCustomers.Add(currentCostomer); }
            //return ListOfCustomers.FindAll(i => predicate == null ? true : predicate(i));
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            return from item in customers
                   where (predicate == null ? true : predicate(item))
                   select item;

        }

        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return from item in stations
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null)
        {
            //List<Parcel> ListOfParcel = new();
            //foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
            //return ListOfParcel.FindAll(i => predicate == null ? true : predicate(i));
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from item in parcels
                   where (predicate == null ? true : predicate(item))
                   select item;
        }

        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharging = new();
            foreach (DroneCharge currentDrone in DataSource.DroneCharges) { droneCharging.Add(currentDrone); }
            return droneCharging.FindAll(i => predicate == null ? true : predicate(i));
            IEnumerable<XElement> elements = XMLTools.LoadListFromXMLElement(DroneChargeXml).Elements();
            //return from drone in elements
            //       let tmp = GetDroneCharge(int.Parse(drone.Element("Id").Value), int.Parse(drone.Element("BaseStationID").Value))
            //       where predicate == null || predicate((DO.DroneCharge)(tmp.Item1))
            //       select tmp;
        }
        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null)
        {
            return from item in DataSource.Users
                   where (predicate == null ? true : predicate(item))
                   select item;
        }
        #endregion

        public double[] ChargingDrone()
        {
            return new double[] { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };

        }

        (DroneCharge, int) IDal.GetDroneCharge(int droneID, int baseStationId)
        {
            throw new NotImplementedException();
        }
    }
}

