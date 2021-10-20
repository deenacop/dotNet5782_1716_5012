﻿using System;
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
                    if (DroneIDToParcel == 0)
                        Console.WriteLine("overflow\n");
                    //DroneIDToParcel = DataSource.Drones[DataSource.rand.Next(1, 10)].ID;
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

            public static void SendingDroneToChargingBaseStation(int DroneID, int ChosenStation)//?????????
            {
                DroneCharge ChargingDroneBattery = new DroneCharge();
                ChargingDroneBattery.RecDrone = DroneID;
                ChargingDroneBattery.RecBaseStation = ChosenStation;
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
            }
            /// <summary>
            /// prints the drone's detailes 
            /// </summary>
            /// <param name="DroneID">The requested drone</param>
            public static void DroneDisplay(int DroneID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneID)
                        Console.WriteLine(DataSource.Drones[i].ToString());
                }

            }

            /// <summary>
            /// prints the station's detailes 
            /// </summary>
            /// <param name="StationID">The requested station</param>
            public static void StationDisplay(int StationID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].ID == StationID)
                        Console.WriteLine(DataSource.Stations[i].ToString());
                }

            }
            /// <summary>
            /// prints the customer's detailes
            /// </summary>
            /// <param name="CustomerID">The requested customer</param>
            public static void CustomerDisplay(int CustomerID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                {
                    if (DataSource.Customers[i].ID == CustomerID)
                        Console.WriteLine(DataSource.Customers[i].ToString());
                }

            }
            /// <summary>
            /// prints the parcel's detailes
            /// </summary>
            /// <param name="ParcelID"> The requested parcel</param>
            public static void ParcelDisplay(int ParcelID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                        Console.WriteLine(DataSource.Parcels[i].ToString());
                }

            }
            /// <summary>
            /// Prints all the drones in the list.
            /// </summary>
            public static Drone[] ListDroneDisplay()
            {
                Drone[] ListOfDrones = new Drone[DataSource.Config.FirstAvailableDrone];
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    ListOfDrones[i] = DataSource.Drones[i];
                    Console.WriteLine(ListOfDrones[i].ToString());
                }
                return ListOfDrones;

            }
            /// <summary>
            /// Prints all the customers in the list.
            /// </summary>
            public static Customer[] ListCustomerDisplay()
            {
                Customer[] ListOfCustomers = new Customer[DataSource.Config.FirstAvailableCustomer];

                for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                {
                    ListOfCustomers[i] = DataSource.Customers[i];
                    Console.WriteLine(DataSource.Customers[i].ToString());
                }
                return ListOfCustomers;
            }

            /// <summary>
            /// prints all the station in the list 
            /// </summary>
            public static Station[] ListStationDisplay()
            {
                Station[] ListOfStation = new Station[DataSource.Config.FirstAvailableStation];

                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    ListOfStation[i] = DataSource.Stations[i];
                    Console.WriteLine((ListOfStation[i].ToString()));

                }
                return ListOfStation;
            }
            /// <summary>
            /// prints all the parcel in the list
            /// </summary>
            public static Parcel[] ListParcelDisplay()
            {
                Parcel[] ListOfParcel = new Parcel[DataSource.Config.FirstAvailableParcel];

                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    ListOfParcel[i] = DataSource.Parcels[i];
                    Console.WriteLine(ListOfParcel[i].ToString());
                }
                return ListOfParcel;
            }

            /// <summary>
            /// Prints all the Unassigned parcels.
            /// </summary>
            public static void ListOfUnassignedParcels()
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].MyDroneID == 0)
                        Console.WriteLine(DataSource.Parcels[i].ToString());
                }
            }

            /// <summary>
            /// Display of base stations with available charging stations
            /// </summary>
            public static void ListOfAvailableChargingStations()
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].NumOfAvailableChargeSlots != 0)
                        Console.WriteLine(DataSource.Stations[i].ToString());
                }
            }

        }


    }
}
