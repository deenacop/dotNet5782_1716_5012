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
        public void Add(Drone NewDrone);
        public void Add(Station NewStation);
        public void Add(Parcel NewParcel);
        public void Add(Customer NewCustomer);
        public void AssignParcelToDrone(int ParcelID, int DroneID);
        public void CollectionOfParcelByDrone(int ParcelID, int DroneID);
        public void DeliveryParcelToCustomer(int ParcelID);
        public void SendingDroneToChargingBaseStation(int DroneID, int StationID);
        public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID);
        public void UpdateStation(int ID, string name = null, int? numOfSlots = null);
        public void UpdateCustomer(int ID, string name = null, string phone = null);
        public Drone DroneDisplay(int DroneID);
        public Station StationDisplay(int StationID);
        public Customer CustomerDisplay(int CustomerID);
        public Parcel ParcelDisplay(int ParcelID);
        public IEnumerable<Drone> ListDroneDisplay(Predicate<Drone> predicate = null);
        public IEnumerable<Customer> ListCustomerDisplay(Predicate<Customer> predicate = null);
        public IEnumerable<Station> ListStationDisplay(Predicate<Station> predicate = null);
        public IEnumerable<Parcel> ListParcelDisplay(Predicate<Parcel> predicate = null);
        public IEnumerable<DroneCharge> ListOfDroneCharge(Predicate<DroneCharge> predicate = null);
        public IEnumerable<Parcel> ListOfUnassignedParcels();
        public IEnumerable<Station> ListOfAvailableChargingStations();
        public double[] ChargingDrone();
    }
}
