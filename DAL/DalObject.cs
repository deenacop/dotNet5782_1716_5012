using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class DalObject
        {
            /// <summary>
            /// DalObject constructor
            /// </summary>
            public DalObject() { DataSource.Initialize(); }
            /// <summary>
            /// Adds a new DroneCharge
            /// </summary>
            /// <param name="droneCharge">The new DroneCharge that we will added to the list of DroneCharges</param>
            public static void Add (DroneCharge droneCharge)
            {
                DataSource.DroneCharges[DataSource.Config.FirstAvailableDroneCharge++]= droneCharge;
            }

            /// <summary>
            /// Adds a new drone
            /// </summary>
            /// <param name="NewDrone">The new drone that will be added to the list of drones</param>
            public static void Add(Drone NewDrone)
            {
                DataSource.Drones[DataSource.Config.FirstAvailableDrone++] = NewDrone;
            }

            /// <summary>
            /// Adds a new station 
            /// </summary>
            /// <param name="NewStation">The new station that will be added to the list of stations</param>
            public static void Add(Station NewStation)
            {
                DataSource.Stations[DataSource.Config.FirstAvailableStation++] = NewStation;
            }

            /// <summary>
            /// Adds a new parcel
            /// </summary>
            /// <param name="NewParcel">The new parcel that will be added to the list of parcels</param>
            public static void Add(Parcel NewParcel)
            {
                DataSource.Parcels[DataSource.Config.FirstAvailableParcel++] = NewParcel;
            }

            /// <summary>
            /// Adds a new customer
            /// </summary>
            /// <param name="NewCustomer">The new Customer that will be added to the list of customer</param>
            public static void Add(Customer NewCustomer)
            {
                DataSource.Customers[DataSource.Config.FirstAvailableCustomer++] = NewCustomer;
            }

            public static int DroneIDToParcel = 0;

            /// <summary>
            /// Assigns a free drone to the parcel
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be assigns</param>
            public static void AssignParcelToDrone(int ParcelID)
            {
                //finds the first available drone
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].Status == @enum.DroneStatus.available)
                    {
                        DroneIDToParcel = DataSource.Drones[i].ID;//The found drone 
                        break;
                    }
                }
                //finds the wanted parcel
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                    {
                        DataSource.Parcels[i].MyDroneID = DroneIDToParcel;
                        DataSource.Parcels[i].Scheduled = DateTime.Now;
                        break;
                    }
                }
            }

            /// <summary>
            /// Collection of a requested parcel by a drone
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
            public static void CollectionOfParcelByDrone(int ParcelID)
            {
                //finds the wanted parcel
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                    {
                        DataSource.Parcels[i].Delivered = DateTime.Now;
                        break;
                    }
                }
                //finds the assigned drone to the corrent parcel
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneIDToParcel)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.delivery;
                        break;
                    }
                }

            }
            /// <summary>
            /// Delivery parcil to customer
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
            public static void DeliveryParcelToCustomer(int ParcelID)
            {
                //finds the wanted parcel and updates the pick up time
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                    {
                        DataSource.Parcels[i].PickUp = DateTime.Now;
                        DataSource.Parcels[i].MyDroneID = 0;
                        break;
                    }
                }
                //changes the status of the drone after the delivery to be available
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneIDToParcel)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.available;
                        break;
                    }
                }

            }

            public static void SendingDroneToChargingBaseStation(int DroneID, int ChosenStation)
            {
                DroneCharge ChargingDroneBattery = new DroneCharge();
                ChargingDroneBattery.RecDrone = DroneID;
                ChargingDroneBattery.RecBaseStation = ChosenStation;
                Add(ChargingDroneBattery);
                //changes the status of the drone to be maintenance
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneID)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.maintenance;
                        break;
                    }
                }
                //up dates the number of available charging slots
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].ID == ChosenStation)
                    {
                        DataSource.Stations[i].NumOfAvailableChargeSlots--;
                        break;
                    }
                }
            }
            /// <summary>
            /// Releasing a drone from a charging Base Station.
            /// </summary>
            /// <param name="DroneID">The ID of the wanted drone</param>
            /// <param name="BaseStationID">The ID of the wanted station</param>
            public static void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID)
            {
                //changes the status of the drone to be available
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneID)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.available;
                        DataSource.Drones[i].Battery = 100;
                        break;
                    }
                }

                //Finds the station that the drone was released from and updates the number of available charging slots.
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].ID == BaseStationID)
                    {
                        DataSource.Stations[i].NumOfAvailableChargeSlots++;
                        break;
                    }
                }
                for(int i=0;i<DataSource.Config.FirstAvailableDroneCharge;i++)//Resets the requested instance. (To "delete" it from the array of DroneCharge)
                {
                    if(DataSource.DroneCharges[i].RecDrone== DroneID&& DataSource.DroneCharges[i].RecBaseStation == BaseStationID)
                    {
                        DataSource.DroneCharges[i].RecDrone = 0; DataSource.DroneCharges[i].RecBaseStation = 0;
                    }
                }
            }
            /// <summary>
            /// Return the wanted drone
            /// </summary>
            /// <param name="DroneID">The requested drone</param>
            public static Drone DroneDisplay(int DroneID)
            {
                Drone droneWanted=new Drone();
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneID)
                        droneWanted = DataSource.Drones[i];
                }
                return droneWanted;
            }

            /// <summary>
            /// Return the wanted station
            /// </summary>
            /// <param name="StationID">The requested station</param>
            public static Station StationDisplay(int StationID)
            {
                Station stationWanted = new Station();
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].ID == StationID)
                        stationWanted= DataSource.Stations[i];
                }
                return stationWanted;
            }
            /// <summary>
            /// Return the wanted customer
            /// </summary>
            /// <param name="CustomerID">The requested customer</param>
            public static Customer CustomerDisplay(int CustomerID)
            {
                Customer custumerWanted = new Customer();
                for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                {
                    if (DataSource.Customers[i].ID == CustomerID)
                        custumerWanted = DataSource.Customers[i];
                }
                return custumerWanted;
            }
            /// <summary>
            /// Return the wanted parcel
            /// </summary>
            /// <param name="ParcelID"> The requested parcel</param>
            public static Parcel ParcelDisplay(int ParcelID)
            {
                Parcel parcelWanted = new Parcel();
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                        parcelWanted=DataSource.Parcels[i];
                }
                return parcelWanted;
            }
            /// <summary>
            /// Returns all the drones in the list.
            /// </summary>
            public static Drone[] ListDroneDisplay()
            {
                Drone[] ListOfDrones = new Drone[DataSource.Config.FirstAvailableDrone];
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                    ListOfDrones[i] = DataSource.Drones[i];
                return ListOfDrones;

            }
            /// <summary>
            /// Returns all the customers in the list.
            /// </summary>
            public static Customer[] ListCustomerDisplay()
            {
                Customer[] ListOfCustomers = new Customer[DataSource.Config.FirstAvailableCustomer];

                for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                    ListOfCustomers[i] = DataSource.Customers[i];
                return ListOfCustomers;
            }

            /// <summary>
            /// Returns all the station in the list 
            /// </summary>
            public static Station[] ListStationDisplay()
            {
                Station[] ListOfStation = new Station[DataSource.Config.FirstAvailableStation];

                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                    ListOfStation[i] = DataSource.Stations[i];
                return ListOfStation;
            }
            /// <summary>
            /// Returns all the parcel in the list
            /// </summary>
            public static Parcel[] ListParcelDisplay()
            {
                Parcel[] ListOfParcel = new Parcel[DataSource.Config.FirstAvailableParcel];

                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                    ListOfParcel[i] = DataSource.Parcels[i];
                return ListOfParcel;
            }

            /// <summary>
            /// Returns all the Unassigned parcels.
            /// </summary>
            public static Parcel[] ListOfUnassignedParcels()
            {
                int amountOfUnassignedParcels = 0;
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].MyDroneID == 0)
                        amountOfUnassignedParcels++;
                }
                Parcel[] UnassignedParcels = new Parcel[amountOfUnassignedParcels];
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].MyDroneID == 0)
                        UnassignedParcels[i] = DataSource.Parcels[i];
                }
                return UnassignedParcels;
            }

            /// <summary>
            /// Returns all the base stations with available charging stations
            /// </summary>
            public static Station[] ListOfAvailableChargingStations()
            {
                int amountOfAvailableChargingStations = 0;
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].NumOfAvailableChargeSlots != 0)
                        amountOfAvailableChargingStations++;
                }
                Station[] AvailableChargingStations = new Station[amountOfAvailableChargingStations];
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].NumOfAvailableChargeSlots != 0)
                        AvailableChargingStations[i] = DataSource.Stations[i];
                }
                return AvailableChargingStations;
            }

        }


    }
}
