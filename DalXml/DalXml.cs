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
        private static string UserXml = @"UserXml.xml";
        #region singelton

        private static readonly Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml()); //Lazy initialization of an object means that its creation is deferred until it is first used.

        internal static DalXml Instance { get { return instance.Value; } }
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
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the station exists and if not throws an exception
            if (stations.Exists(i => i.Id == station.Id))
                throw new AlreadyExistedItemException("The station already exists");
            stations.Add(station);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public int Add(Parcel parcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (parcels.Exists(item => item.Id == parcel.Id && !parcel.IsRemoved))//checks if parcel exists
                throw new AlreadyExistedItemException("The parcel already exists.\n");
            XElement runNumber = XMLTools.LoadListFromXMLElement(@"config.xml");
            parcel.Id = 1 + int.Parse(runNumber.Element("RunnerIDNumParcels").Value);
            runNumber.Element("RunnerIDNumParcels").Value = parcel.Id.ToString();
            parcels.Add(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            XMLTools.SaveListToXMLElement(runNumber, "config.xml");
            return parcel.Id;
        }

        public void Add(Customer customer)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            XElement newCustomer = (from cus in customerXml.Elements()
                                    where cus.Element("Id").Value == customer.Id.ToString()
                                    select cus).FirstOrDefault();
            if (newCustomer != null)
            {
                throw new AlreadyExistedItemException("The customer already exists.\n");//checks if customer exists
            }
            XElement CustomerElem = new XElement("Customer",
                                 new XElement("Id", customer.Id),
                                 new XElement("Name", customer.Name),
                                 new XElement("Phone", customer.PhoneNumber),
                                 new XElement("Latitude", customer.IsRemoved),
                                 new XElement("Longitude", customer.Longitude),
                                 new XElement("Latitude", customer.Latitude));
            customerXml.Add(CustomerElem);
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }

        //public void Add(DroneCharge droneCharge)
        //{
        //    List<DroneCharge> droneChargeRoot = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
        //    if (droneChargeRoot.Exists(item => item.Id == droneCharge.Id))//chcks if the charger already exists
        //    {
        //        throw new AlreadyExistedItemException("The Drone Charge already exists.\n");
        //    }
        //    droneChargeRoot.Add(droneCharge);
        //    XMLTools.SaveListToXMLSerializer(droneChargeRoot, DroneChargeXml);
        //}

        public void Add(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            //checks if the station exists and if not throws an exception
            if (users.Exists(i => i.Id == user.Id))
                throw new AlreadyExistedItemException("The station already exists");
            users.Add(user);
            XMLTools.SaveListToXMLSerializer(users, UserXml);
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
            int indexParcel = parcels.FindIndex(parcel => parcel.Id == parcel.Id);//finding parcel that was collected by drone
            if (indexParcel == -1)
                throw new ItemNotExistException("No parcel found with this id");
            Parcel newParcel = parcels[indexParcel];
            newParcel.IsRemoved = true;
            parcels[indexParcel] = newParcel;
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
            if (index == -1 || parcels[index].IsRemoved)//not found
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
            if (index == -1 || parcels[index].IsRemoved)//not found
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
            if (index == -1 || parcels[index].IsRemoved)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            parcels[index] = tmp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the drone exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = stations.FindIndex(i => i.Id == stationID);
            if (index == -1)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge

            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.Id = droneID;
            ChargingDroneBattery.BaseStationID = stationID;
            ChargingDroneBattery.EnterToChargBase = DateTime.Now;
            ChargingDroneBattery.FinishedRecharging = null;


            //adds
            droneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            Station tmp = stations[index];
            tmp.NumOfAvailableChargingSlots--;
            stations[index] = tmp;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        //??????????
        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the drone and station exists and if not throws an exception
            if (!drones.Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            int index = stations.FindIndex(i => i.Id == baseStationID);
            if (index == -1)
                throw new ItemNotExistException("The station does not exists");

            int indexDC = droneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.Id == droneID);//finds index where drone is
            if (indexDC == -1)//checks if drone exists
                throw new ItemNotExistException("The drone does not exist.\n");

            Station station = stations[index];
            station.NumOfAvailableChargingSlots++;
            stations[index] = station;

            droneCharges.Remove(droneCharges[indexDC]);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
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
        public void UpdateCustomer(int Id, string name = null, string phone = null)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new ItemNotExistException("The customer does not exist.\n");

            customer.Element("Id").Value = Id.ToString();
            if (name != null)
                customer.Element("Name").Value = name;
            if (phone != null)
                customer.Element("PhoneNumber").Value = phone;
            customer.Element("Longitude").Value = customer.Element("Longitude").ToString();
            customer.Element("Latitude").Value = customer.Element("Latitude").ToString();
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        public void updateUser(string mail, string password)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            int index = users.FindIndex(i => i.EmailAddress == mail);
            if (index == -1)//not exist
                throw new ItemNotExistException("The user does not exsit");
            User tmp = DataSource.Users[index];
            tmp.Password = password;
            DataSource.Users[index] = tmp;
            XMLTools.SaveListToXMLSerializer(users, UserXml);
        }


        #endregion

        #region Get item
        public DroneCharge GetDroneCharge(int droneID, int baseStationId)
        {
            try
            {
                List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
                return droneCharges.First(x => x.Id == droneID);
            }
            catch (InvalidOperationException)
            {
                throw new ItemNotExistException("The drone does not exists.\n");
            }
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
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            Customer customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == customerID.ToString()
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     PhoneNumber = cus.Element("PhoneNumber").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value)
                                    
                                 }
                        ).FirstOrDefault();
            if (customer.Id != 0)
                return customer;
            else
                throw new ItemNotExistException("The customer does not exist.\n");
        }

        public Parcel GetParcel(int parcelID)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (!parcels.Exists(i => i.Id == parcelID || i.IsRemoved))
                throw new ItemNotExistException("The parcel does not exists");
            return parcels.Find(i => i.Id == parcelID);
        }
        public User GetUser(string mail)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            if (!users.Exists(i => i.EmailAddress == mail || i.IsRemoved))
                throw new ItemNotExistException("The user does not exists");
            return users.Find(i => i.EmailAddress == mail);
        }
        #endregion

        #region Lists of items
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from item in drones
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in customerXml.Elements()
                                             select new Customer()
                                             {
                                                 Id = int.Parse(cus.Element("Id").Value),
                                                 Name = cus.Element("Name").Value,
                                                 PhoneNumber = cus.Element("Phone").Value,
                                                 Longitude = double.Parse(cus.Element("Longitude").Value),
                                                 Latitude = double.Parse(cus.Element("Latitude").Value),
                                                 IsRemoved = bool.Parse(cus.Element("IsRemoved").Value)
                                             };
            return customer.Select(item => item);
        }

        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return from item in stations
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null)
        {
            //List<Parcel> ListOfParcel = new();
            //foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
            //return ListOfParcel.FindAll(i => predicate == null ? true : predicate(i));
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from item in parcels
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null)
        {
            //List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            //return from item in droneCharges
            //       where predicate == null ? true : predicate(item)
            //       select item;

            List<DroneCharge> droneCharging = new();
            foreach (DroneCharge currentDrone in DataSource.DroneCharges) { droneCharging.Add(currentDrone); }
            return droneCharging.FindAll(i => predicate == null ? true : predicate(i));
            //IEnumerable<XElement> elements = XMLTools.LoadListFromXMLElement(DroneChargeXml).Elements();
            //return from drone in elements
            //       let tmp = GetDroneCharge(int.Parse(drone.Element("Id").Value), int.Parse(drone.Element("BaseStationID").Value))
            //       where predicate == null || predicate((DO.DroneCharge)(tmp.Item1))
            //       select tmp;
        }
        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            return from item in users
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        #endregion

        public double[] ChargingDrone()
        {
            return new double[] { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };

            XElement p = XMLTools.LoadListFromXMLElement(@"config.xml");
            return new double[] {
           int.Parse(p.Element("vacant").Value),
            int.Parse(p.Element("CarriesLightWeight").Value),
            int.Parse(p.Element("CarriesMediumWeight").Value),
             int.Parse(p.Element("CarriesHeavyWeight").Value),
           int.Parse(p.Element("DroneLoadingRate").Value),
        };

        }


    }
}

