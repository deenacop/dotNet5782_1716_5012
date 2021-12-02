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

        public void Add(Drone NewDrone)
        {
            //checks if the drone exists and if not throws an exception
            if (DataSource.Drones.Exists(item => item.Id == NewDrone.Id))
                throw new AlreadyExistedItemException("The drone already exists");
            DataSource.Drones.Add(NewDrone);
        }

        public void Add(Station NewStation)
        {
            //checks if the station exists and if not throws an exception
            if (DataSource.Stations.Exists(item => item.StationID == NewStation.StationID))
                throw new AlreadyExistedItemException("The station already exists");
            DataSource.Stations.Add(NewStation);
        }

        public void Add(Parcel NewParcel)
        {
            NewParcel.ParcelID = DataSource.Config.RunnerIDNumParcels++;
            DataSource.Parcels.Add(NewParcel);
        }

        public void Add(Customer NewCustomer)
        {
            //checks if the customer exists and if not throws an exception
            if (DataSource.Customers.Exists(item => item.CustomerID == NewCustomer.CustomerID))
                throw new AlreadyExistedItemException("The customer already exists");
            DataSource.Customers.Add(NewCustomer);
        }
        #endregion

        #region Updates
        public void AssignParcelToDrone(int ParcelID, int DroneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.Id == DroneID))
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

        public void CollectionOfParcelByDrone(int ParcelID, int DroneID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.Id == DroneID))
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

        public void SendingDroneToChargingBaseStation(int droneID, int stationID)
        {
            //checks if the drone exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.Id == droneID))
                throw new ItemNotExistException("The drone does not exists");
            //find the station
            int index = DataSource.Stations.FindIndex(item => item.StationID == stationID);
            if (index < 0)
                throw new ItemNotExistException("The station does not exists");
            //creates a new varible of drone charge
            DroneCharge ChargingDroneBattery = new();
            ChargingDroneBattery.Id = droneID;
            ChargingDroneBattery.BaseStationID = stationID;
            //adds
            DataSource.DroneCharges.Add(ChargingDroneBattery);
            //up dates the number of available charging slots
            Station tmp = DataSource.Stations[index];
            tmp.NumOfAvailableChargingSlots--;
            DataSource.Stations[index] = tmp;
        }

        public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID)
        {
            //checks if the drone and station exists and if not throws an exception
            if (!DataSource.Drones.Exists(item => item.Id == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            if (!DataSource.Stations.Exists(item => item.StationID == BaseStationID))
                throw new ItemNotExistException("The station does not exists");
            int index = DataSource.DroneCharges.FindIndex(item => item.Id == DroneID && item.BaseStationID == BaseStationID);//finds the drone charge
            if (index < 0)//not found
                throw new ItemNotExistException("the drone does not exist in the wanted base station");
            DroneCharge tmp1 = DataSource.DroneCharges[index];
            tmp1.FinishedRecharging = DateTime.Now; ;//delete the drone from the list of the drone charge
            DataSource.DroneCharges[index] = tmp1;
            index = DataSource.Stations.FindIndex(item => item.StationID == BaseStationID);//finds the station
            Station tmp2 = DataSource.Stations[index];
            tmp2.NumOfAvailableChargingSlots++;
            DataSource.Stations[index] = tmp2;
        }

        public void UpdateStation(int ID, string name, int numOfSlots)
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
            if (numOfSlots != 0)
            {
                int numOfFullSlots = DataSource.DroneCharges.FindAll(item => item.BaseStationID == ID).Count;//does it work??
                tmp.NumOfAvailableChargingSlots = (int)(numOfSlots - numOfFullSlots);
                DataSource.Stations[index] = tmp;
            }
        }

        public void UpdateCustomer(int ID, string name, string phone )
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
      

        public void UpdateDroneModel(int ID, string model)
        {
            int index = DataSource.Drones.FindIndex(item => item.Id == ID);
            if (index == -1)
                throw new ItemNotExistException("Drone does not exist");
            Drone tmp = DataSource.Drones[index];
            tmp.Model = model;
            DataSource.Drones[index] = tmp;
        }
        #endregion

        #region Display one item
        public Drone DroneDisplay(int DroneID)
        {
            if (!DataSource.Drones.Exists(item => item.Id == DroneID))
                throw new ItemNotExistException("The drone does not exists");
            return DataSource.Drones.Find(item => item.Id == DroneID);
        }

        public Station GetStation(int StationID)
        {
            if (!DataSource.Stations.Exists(item => item.StationID == StationID))
                throw new ItemNotExistException("The station does not exists");
            return DataSource.Stations.Find(item => item.StationID == StationID);
        }

        public Customer CustomerDisplay(int CustomerID)
        {
            if (!DataSource.Customers.Exists(item => item.CustomerID == CustomerID))
                throw new ItemNotExistException("The customer does not exists");
            return DataSource.Customers.Find(item => item.CustomerID == CustomerID);
        }

        public Parcel ParcelDisplay(int ParcelID)
        {
            if (!DataSource.Parcels.Exists(item => item.ParcelID == ParcelID))
                throw new ItemNotExistException("The parcel does not exists");
            return DataSource.Parcels.Find(item => item.ParcelID == ParcelID);
        }
        #endregion

        #region Lists of items
        public IEnumerable<Drone> ListDroneDisplay(Predicate<Drone> predicate = null)
        {
            List<Drone> ListOfDrones = new();
            foreach (Drone currentDrone in DataSource.Drones) { ListOfDrones.Add(currentDrone); }
            return ListOfDrones.FindAll(i => predicate == null ? true : predicate(i)); ;
        }
     
        public IEnumerable<Customer> ListCustomerDisplay(Predicate<Customer> predicate = null)
        {
            List<Customer> ListOfCustomers = new();
            foreach (Customer currentCostomer in DataSource.Customers) { ListOfCustomers.Add(currentCostomer); }
            return ListOfCustomers.FindAll(i => predicate == null ? true : predicate(i));
        }
  
        public IEnumerable<Station> ListStationDisplay(Predicate<Station> predicate = null)
        {
            List<Station> ListOfStation = new();
            foreach (Station currentStation in DataSource.Stations) { ListOfStation.Add(currentStation); }
            return ListOfStation.FindAll(i => predicate == null ? true : predicate(i));
        }
  
        public IEnumerable<Parcel> ListParcelDisplay(Predicate<Parcel> predicate = null)
        {
            List<Parcel> ListOfParcel = new();
            foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
            return ListOfParcel.FindAll(i => predicate == null ? true : predicate(i));
        }
   
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

        public double[] ChargingDrone()
        {
            double[] arr = { DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                    DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,DataSource.Config.DroneLoadingRate };
            return arr;

        }

        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(item => item.Id == drone.Id);
            if (index == -1)
                throw new ItemNotExistException("Drone does not exist");
            DataSource.Drones[index] = drone;

            //if (DataSource.Drones.RemoveAll(d => d.Id == drone.Id) == 0)
            //    throw new ItemNotExistException("Drone does not exist");
            //DataSource.Drones.Add(drone);
        }
    }
}


