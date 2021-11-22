using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        public IEnumerable<CustomerToList> ListCustomerDisplay();
        public Customer CustomerDisplay(int ID);
        public void UpdateCustomer(int ID, string name = null, string phone = null);
        public void AddCustomer(Customer customer);
        public IEnumerable<DroneToList> ListDroneDisplay();
        public Drone DisplayDrone(int droneID);
        public void ReleasingDroneFromBaseStation(int ID, int minuteInCharge);
        public void SendDroneToCharge(int ID);
        public void AddDrone(Drone drone, int stationID);
        public IEnumerable<ParcelToList> ListOfUnassignedParcelDisplay();
        public IEnumerable<ParcelToList> ListParcelDisplay();
        public Parcel ParcelDisplay(int ID);

        public void AddParcel(Parcel parcel);
        public IEnumerable<BaseStationToList> ListOfAvailableSlotsBaseStationlDisplay();
        public IEnumerable<BaseStationToList> ListBaseStationlDisplay();
        public void UpdateStation(int ID, string name = null, int? numOfSlots = null);
        public BaseStation BaseStationDisplay(int StationID);
        public void AddBaseStation(BaseStation station);
        public void UpdateDroneModel(int ID, string model);


















    }
}
