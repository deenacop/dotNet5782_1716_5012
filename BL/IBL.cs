using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    public interface IBL
    {
        void AddBaseStation(BaseStation station);
        void AddCustomer(Customer customer);
        void AddDrone(DroneToList drone, int stationID);
        void AddParcel(Parcel parcel);
        void AssignParcelToDrone(int ID);
        BaseStation BaseStationDisplay(int StationID);
        void CollectionOfParcelByDrone(int ID);
        Customer CustomerDisplay(int ID);
        Drone DisplayDrone(int droneID);
        IEnumerable<BaseStationToList> ListBaseStationlDisplay();
        IEnumerable<CustomerToList> ListCustomerDisplay();
        IEnumerable<DroneToList> ListDroneDisplay();
        IEnumerable<BaseStationToList> ListOfAvailableSlotsBaseStationlDisplay();
        IEnumerable<ParcelToList> ListOfUnassignedParcelDisplay();
        IEnumerable<ParcelToList> ListParcelDisplay();
        Parcel ParcelDisplay(int ID);
        void ReleasingDroneFromBaseStation(int ID, int minuteInCharge);
        void SendDroneToCharge(int ID);
        void UpdateCustomer(int ID, string name = null, string phone = null);
        void UpdateDroneModel(int ID, string model);
        void UpdateStation(int ID, string name = null, int? numOfSlots = null);
        void DeliveryParcelByDrone(int ID);
    }
}