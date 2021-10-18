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
            public void Add(Drone NewDrone)
            {
                DataSource.Drones[DataSource.Config.FirstAvailableDrone++] = NewDrone;
            }
            public void Add(Station NewStation)
            {
                DataSource.Stations[DataSource.Config.FirstAvailableStation++] = NewStation;
            }
            public void Add(Parcel NewParcel)
            {
                DataSource.Parcels[DataSource.Config.FirstAvailableParcel++] = NewParcel;
            }
            public void Add(Customer NewCustomer)
            {
                DataSource.Customers[DataSource.Config.FirstAvailableCustomer++] = NewCustomer;
            }
            public static int DroneIDToParcel = 0;

            /// <summary>
            /// Assigns a free drone to the parcel
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be assigns</param>
            public void AssignParcelToDrone(int ParcelID)
            {
                //finds the first available drone
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].Status == @enum.DroneStatus.available)
                    {
                        //DataSource.Drones[i].Status = @enum.DroneStatus.delivery;
                        DroneIDToParcel = DataSource.Drones[i].ID;//The found drone 
                        break;
                    }
                    if (DroneIDToParcel == 0)
                        DroneIDToParcel = DataSource.Drones[DataSource.rand.Next(1, 10)].ID;
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
            public void CollectionOfParcelByDrone(int ParcelID)
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
            public void DeliveryParcelToCustomer(int ParcelID)
            {
                //finds the wanted parcel and updates the pick up time
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                    {
                        DataSource.Parcels[i].PickUp = DateTime.Now;
                        break;
                    }
                }
                //changes the status of the drone after the delivery to be available
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneIDToParcel)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.available;//???
                        break;
                    }
                }

            }

            public void SendingDroneForchargingBaseStation(int DroneID)
            {
                //changes the status of the drone to be maintenance
                for (int i = 0; i < DataSource.Config.FirstAvailableDrone; i++)
                {
                    if (DataSource.Drones[i].ID == DroneID)
                    {
                        DataSource.Drones[i].Status = @enum.DroneStatus.maintenance;
                        break;
                    }
                }

                DroneCharge droneCharge = new DroneCharge();

            }
            /// <summary>
            /// Releasing a drone from a charging Base Station.
            /// </summary>
            /// <param name="DroneID">The ID of the wanted drone</param>
            /// <param name="BaseStationID">The ID of the wanted station</param>
            public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID)
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
            public void DroneDisplay(int DroneID)
            {
                for(int i =0; i<DataSource.Config.FirstAvailableDrone;i++)
                {
                    if(DataSource.Drones[i].ID==DroneID)
                        Console.WriteLine(DataSource.Drones[i].ToString());
                }
                
            }

            /// <summary>
            /// prints the station's detailes 
            /// </summary>
            /// <param name="StationID">The requested station</param>
            public void StationDisplay(int StationID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                {
                    if (DataSource.Stations[i].ID == StationID)
                        Console.WriteLine(DataSource.Stations[i].ToString());
                }

            }
            /// <summary>
            /// prints the parcel's detailes
            /// </summary>
            /// <param name="ParcelID"> The requested parcel</param>
            public void ParcelDisplay(int ParcelID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                    if (DataSource.Parcels[i].ID == ParcelID)
                        Console.WriteLine(DataSource.Parcels[i].ToString());
                }

            }
            /// <summary>
            /// prints all the station in the list 
            /// </summary>
            public void ListStationDisplay()
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                { 
                      Console.WriteLine(DataSource.Stations[i].ToString());
                }

            }
            /// <summary>
            /// prints all the parcel in the list
            /// </summary>
            public void ListParcelDisplay(int ParcelID)
            {
                for (int i = 0; i < DataSource.Config.FirstAvailableParcel; i++)
                {
                        Console.WriteLine(DataSource.Parcels[i].ToString());
                }
            }

        }


    }
}
