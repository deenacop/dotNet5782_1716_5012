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

        //for initialize
        static internal int RunnerIDNumParcels = 100000;
        static internal double vacant = 2;//per km
        static internal double CarriesLightWeight = 5;//per km
        static internal double CarriesMediumWeight = 8;//per km
        static internal double CarriesHeavyWeight = 10;//per km
        static internal double DroneLoadingRate = 20;//per min
        #region singelton

        private static readonly Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml()); //Lazy initialization of an object means that its creation is deferred until it is first used.

        internal static DalXml Instance { get { return instance.Value; } }
        static DalXml()
        {
            //XMLTools.SaveListToXMLElement(new XElement("DroneChargeXml.xml"), DroneChargeXml);
            XElement rootElem = new XElement("configLists", new XElement("RunnerIDNumParcels", 100010),
                new XElement("pwrUsgEmpty", vacant),
                new XElement("pwrUsgLight", CarriesLightWeight),
                new XElement("pwrUsgMedium", CarriesMediumWeight),
                new XElement("pwrUsgHeavy", CarriesHeavyWeight),
                new XElement("chargePH", DroneLoadingRate));

            //rootElem.Add(new int[] { , pwrUsgLight, pwrUsgMedium, pwrUsgHeavy, chargePH });
            XMLTools.SaveListToXMLElement(rootElem, "config.xml");
        }
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
        public void RemoveDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //checks if the drone exists and if not throws an exception
            int index = drones.FindIndex(i => i.Id == id);
            if (index == -1)
                throw new ItemNotExistException("The drone does not exists");
            Drone drone = drones[index];
            drone.IsRemoved = true;
            drones[index] = drone;


            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void RemoveStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the station exists and if not throws an exception
            int index = stations.FindIndex(i => i.Id == id);
            if (index == -1)
                throw new AlreadyExistedItemException("The station already exists");
            Station station = stations[index];
            station.IsRemoved = true;
            stations[index] = station;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void RemoveParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int indexParcel = parcels.FindIndex(parcel => parcel.Id == id);//finding parcel that was collected by drone
            if (indexParcel == -1)
                throw new ItemNotExistException("No parcel found with this id");
            Parcel newParcel = parcels[indexParcel];
            newParcel.IsRemoved = true;
            parcels[indexParcel] = newParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void RemoveCustomer(int id)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            //checks if the customer exists and if not throws an exception
            int index = customers.FindIndex(i => i.Id == id);
            if (index == -1)
                throw new AlreadyExistedItemException("The customer already exists");
            Customer customer = customers[index];
            customer.IsRemoved = true;
            customers[index] = customer;
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);

            //Because our program is based on the fact that every user must also be a customer. 
            //If we delete a customer, we must make sure he was not a user in the first place. 
            //And if he was also a user - delete it completely from the list
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            index = users.FindIndex(i => i.Id == id);
            if (index == -1)
                throw new AlreadyExistedItemException("The user already exists");
            users[index].IsRemoved = true;
            XMLTools.SaveListToXMLSerializer(users, UserXml);
        }


        #endregion

        #region Drone operations
        public void AssignParcelToDrone(int parcelID, int droneID)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //checks if the drone exists and if not throws an exception
            if (!XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).Exists(i => i.Id == droneID))
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
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //checks if the drone exists and if not throws an exception
            if (!XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //finds the wanted parcel
            int index = parcels.FindIndex(i => i.Id == parcelID);
            if (index == -1)//not found
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
            if (index == -1)//not found
                throw new ItemNotExistException("The parcel does not exists");
            Parcel tmp = parcels[index];
            tmp.MyDroneID = 0;
            tmp.Delivered = DateTime.Now;
            parcels[index] = tmp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //checks if the drone exists and if not throws an exception
            if (!XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).Exists(i => i.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = stations.FindIndex(i => i.Id == stationID);
            if (index == -1)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new()
            {
                Id = droneID,
                BaseStationID = stationID,
                EnterToChargBase = DateTime.Now,
                FinishedRecharging = null,
            };
            //adds
            List<DroneCharge> droneCharges= XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
            Station tmp = stations[index];
            tmp.NumOfAvailableChargingSlots--;
            stations[index] = tmp;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        public void ReleasingDroneFromChargingBaseStation(int droneID, int baseStationID)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);

            //checks if the drone and station exists and if not throws an exception
            if (!XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).Exists(i => i.Id == droneID))
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
            XElement customer = (from cus in XMLTools.LoadListFromXMLElement(CustomerXml).Elements()
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
            XMLTools.SaveListToXMLElement(XMLTools.LoadListFromXMLElement(CustomerXml), CustomerXml);
        }

        public void updateUser(string mail, string password)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UserXml);
            int index = users.FindIndex(i => i.EmailAddress == mail);
            if (index == -1)//not exist
                throw new ItemNotExistException("The user does not exsit");
            User tmp = users[index];
            tmp.Password = password;
            users[index] = tmp;
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
            Customer customer = (from cus in XMLTools.LoadListFromXMLElement(CustomerXml).Elements().Where(i => i.Element("Id").Value == customerID.ToString())
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
        public IEnumerable<Drone> GetListDrone(Predicate<Drone> predicate = null) =>

            XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).Where(item => predicate == null ? true : predicate(item));


        public IEnumerable<Customer> GetListCustomer(Predicate<Customer> predicate = null) =>
            from cus in XMLTools.LoadListFromXMLElement(CustomerXml).Elements()
            select new Customer()
            {
                Id = int.Parse(cus.Element("Id").Value),
                Name = cus.Element("Name").Value,
                PhoneNumber = cus.Element("PhoneNumber").Value,
                Longitude = double.Parse(cus.Element("Longitude").Value),
                Latitude = double.Parse(cus.Element("Latitude").Value),
                IsRemoved = bool.Parse(cus.Element("IsRemoved").Value ?? "false")
            };





        public IEnumerable<Station> GetListStation(Predicate<Station> predicate = null) =>
             XMLTools.LoadListFromXMLSerializer<Station>(StationXml).Where(item => predicate == null ? true : predicate(item));

        public IEnumerable<Parcel> GetListParcel(Predicate<Parcel> predicate = null) =>
            XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml).Where(item => predicate == null ? true : predicate(item));

        public IEnumerable<DroneCharge> GetListDroneCharge(Predicate<DroneCharge> predicate = null) =>
            XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml).Where(item => predicate == null ? true : predicate(item));

        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null) =>
            XMLTools.LoadListFromXMLSerializer<User>(UserXml).Where(item => predicate == null ? true : predicate(item));

        #endregion

        public double[] ChargingDrone()
        {

            XElement tempPwr = XMLTools.LoadListFromXMLElement(@"config.xml");
            double[] pwrUsg = { double.Parse(tempPwr.Element("pwrUsgEmpty").Value), double.Parse(tempPwr.Element("pwrUsgLight").Value), 
                double.Parse(tempPwr.Element("pwrUsgMedium").Value), double.Parse(tempPwr.Element("pwrUsgHeavy").Value), double.Parse(tempPwr.Element("chargePH").Value) };
            return pwrUsg;
        }
    }
}

