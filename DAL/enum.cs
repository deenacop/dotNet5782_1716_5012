using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct @enum
        {
            /// <summary>
            /// The drone weights
            /// </summary>
            public enum WeightCategories { light, midium, heavy };
            /// <summary>
            /// In what stage the drone is in
            /// </summary>
            public enum DroneStatus { available, maintenance, delivery };
            /// <summary>
            /// Delivery priorities
            /// </summary>
            public enum Priorities { normal, fast, urgent };
            /// <summary>
            /// Add , Update, DisplayIndividual, DisplayList, FindTheDistance,Exit 
            /// </summary>
            public enum options { Add = 1, Update, DisplayIndividual, DisplayList,FindTheDistance, Exit };
            /// <summary>
            /// Which item to add
            /// </summary>
            public enum AddOptions { Drone = 1, Station, Parcel, Customer };
            /// <summary>
            ///  What kind of update is needed 
            /// </summary>
            public enum UpdateOptions { AssignParcelToDrone = 1, CollectParcelByDrone, DelivereParcelToCustomer,
                SendDroneToChargingBaseStation, ReleaseDroneFromChargingBaseStation };
            /// <summary>
            /// What item to print
            /// </summary>
            public enum DisplayIndividualOptions { DisplyDrone = 1, DisplyStation, DisplayParcel, DisplayCustomer };
            /// <summary>
            /// What list of items to print
            /// </summary>
            public enum DisplayListOptions { DisplyDroneList = 1, DisplyStationList, DisplayParcelList, 
                DisplayCustomerList, ListOfUnassignedParcels, ListOfAvailableChargingStations };
        }
    }
}
