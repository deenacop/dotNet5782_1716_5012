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
            /// Adds a new drone
            /// </summary>
            /// <param name="NewDrone">The new drone that will be added to the list of drones</param>
            public void Add(Drone NewDrone)
            {
                DataSource.Drones.Add(NewDrone);
            }

            /// <summary>
            /// Adds a new station 
            /// </summary>
            /// <param name="NewStation">The new station that will be added to the list of stations</param>
            public void Add(Station NewStation)
            {
                DataSource.Stations.Add(NewStation);
            }

            /// <summary>
            /// Adds a new parcel
            /// </summary>
            /// <param name="NewParcel">The new parcel that will be added to the list of parcels</param>
            public void Add(Parcel NewParcel)
            {
                NewParcel.ID = ++IDAL.DO.DataSource.Config.RunnerIDNumParcels;
                DataSource.Parcels.Add(NewParcel);
            }

            /// <summary>
            /// Adds a new customer
            /// </summary>
            /// <param name="NewCustomer">The new Customer that will be added to the list of customer</param>
            public void Add(Customer NewCustomer)
            {
                DataSource.Customers.Add(NewCustomer);
            }

            /// <summary>
            /// Assigns a drone to the parcel
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be assigns</param>
            /// <param name="DroneID">The drone's ID that needs to be assigns</param>
            public void AssignParcelToDrone(int ParcelID, int DroneID)
            {
                IEnumerator<Parcel> iter = DataSource.Parcels.GetEnumerator();
                //finds the wanted parcel and assign the drone to the parcel
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == ParcelID)
                    {
                        Parcel tmp = iter.Current;
                        tmp.MyDroneID = DroneID;
                        tmp.Scheduled = DateTime.Now;
                        DataSource.Parcels[i] = tmp;
                        break;
                    }
                }
            }

            /// <summary>
            /// Collection of a requested parcel by a drone
            /// </summary>
            /// <param name="ParcelID">The parcel's ID that needs to be collected</param>
            public void CollectionOfParcelByDrone(int ParcelID, int DroneID)
            {
                IEnumerator<Parcel> iter = DataSource.Parcels.GetEnumerator();
                //finds the wanted parcel
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == ParcelID)
                    {
                        Parcel tmp = iter.Current;
                        tmp.MyDroneID = DroneID;
                        tmp.PickUp = DateTime.Now;
                        DataSource.Parcels[i] = tmp;
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
                IEnumerator<Parcel> iter = DataSource.Parcels.GetEnumerator();
                //finds the wanted parcel
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == ParcelID)
                    {
                        Parcel tmp = iter.Current;
                        tmp.MyDroneID = 0;
                        tmp.Delivered = DateTime.Now;
                        DataSource.Parcels[i] = tmp;
                        break;
                    }
                }
            }

            public void SendingDroneToChargingBaseStation(int DroneID, int StationID)
            {
                DroneCharge ChargingDroneBattery = new();
                ChargingDroneBattery.RecDrone = DroneID;
                ChargingDroneBattery.RecBaseStation = DroneID;
                DataSource.DroneCharges.Add(ChargingDroneBattery);
                //up dates the number of available charging slots
                IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == StationID)
                    {
                        Station tmp = iter.Current;
                        tmp.NumOfAvailableChargeSlots--;
                        DataSource.Stations[i] = tmp;
                        break;
                    }

                }
            }

            /// <summary>
            /// Releasing a drone from a charging Base Station.
            /// </summary>
            /// <param name="DroneID">The ID of the wanted drone</param>
            /// <param name="BaseStationID">The ID of the wanted station</param>
            /// 
            public void ReleasingDroneFromChargingBaseStation(int DroneID, int BaseStationID)
            {
                IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();
                //Finds the station that the drone was released from and updates the number of available charging slots.
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == BaseStationID)
                    {
                        Station tmp = iter.Current;
                        tmp.NumOfAvailableChargeSlots++;
                        DataSource.Stations[i] = tmp;
                        break;
                    }
                }
                foreach (DroneCharge delDroneCharge in DataSource.DroneCharges)//delete the dronecharge from the list of DroneCharge)
                {
                    if (delDroneCharge.RecDrone == DroneID && delDroneCharge.RecBaseStation == BaseStationID)
                    {
                        DataSource.DroneCharges.Remove(delDroneCharge); break;
                    }
                }
            }

            /// <summary>
            /// Return the wanted drone
            /// </summary>
            /// <param name="DroneID">The requested drone</param>
            public Drone DroneDisplay(int DroneID)
            {
                IEnumerator<Drone> iter = DataSource.Drones.GetEnumerator();

                Drone droneWanted = new Drone();
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == DroneID)
                        droneWanted = DataSource.Drones[i]; break;
                }
                return droneWanted;
            }

            /// <summary>
            /// Return the wanted station
            /// </summary>
            /// <param name="StationID">The requested station</param>
            public Station StationDisplay(int StationID)
            {
                IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();

                Station stationWanted = new Station();
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == StationID)
                        stationWanted = DataSource.Stations[i];
                }
                return stationWanted;
            }

            /// <summary>
            /// Return the wanted customer
            /// </summary>
            /// <param name="CustomerID">The requested customer</param>
            public Customer CustomerDisplay(int CustomerID)
            {
                IEnumerator<Customer> iter = DataSource.Customers.GetEnumerator();

                Customer custumerWanted = new Customer();
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == CustomerID)
                        custumerWanted = DataSource.Customers[i];
                }
                return custumerWanted;
            }

            /// <summary>
            /// Return the wanted parcel
            /// </summary>
            /// <param name="ParcelID"> The requested parcel</param>
            public Parcel ParcelDisplay(int ParcelID)
            {
                IEnumerator<Parcel> iter = DataSource.Parcels.GetEnumerator();

                Parcel parcelWanted = new Parcel();
                for (int i = 0; iter.MoveNext(); i++)
                {
                    if (iter.Current.ID == ParcelID)
                        parcelWanted = DataSource.Parcels[i];
                }
                return parcelWanted;
            }

            /// <summary>
            /// Returns all the drones in the list.
            /// </summary>
            public List<Drone> ListDroneDisplay()
            {
                List<Drone> ListOfDrones = new();
                foreach (Drone currentDrone in DataSource.Drones) { ListOfDrones.Add(currentDrone); }
                return ListOfDrones;
            }

            /// <summary>
            /// Returns all the customers in the list.
            /// </summary>
            public List<Customer> ListCustomerDisplay()
            {
                List<Customer> ListOfCustomers = new();
                foreach (Customer currentCostomer in DataSource.Customers) { ListOfCustomers.Add(currentCostomer); }
                return ListOfCustomers;
            }

            /// <summary>
            /// Returns all the station in the list 
            /// </summary>
            public List<Station> ListStationDisplay()
            {
                List<Station> ListOfStation = new();
                foreach (Station currentStation in DataSource.Stations) { ListOfStation.Add(currentStation); }
                return ListOfStation;
            }

            /// <summary>
            /// Returns all the parcel in the list
            /// </summary>
            public List<Parcel> ListParcelDisplay()
            {
                List<Parcel> ListOfParcel = new();
                foreach (Parcel currentParcel in DataSource.Parcels) { ListOfParcel.Add(currentParcel); }
                return ListOfParcel;
            }

            /// <summary>
            /// Returns all the Unassigned parcels.
            /// </summary>
            public List<Parcel> ListOfUnassignedParcels()
            {
                List<Parcel> UnassignedParcels = new();
                foreach (Parcel currentParcel in DataSource.Parcels)
                {
                    if (currentParcel.MyDroneID == 0) UnassignedParcels.Add(currentParcel);
                }
                return UnassignedParcels;
            }

            /// <summary>
            /// Returns all the base stations with available charging stations
            /// </summary>
            public List<Station> ListOfAvailableChargingStations()
            {
                List<Station> AvailableChargingStations = new();
                foreach (Station currentStation in DataSource.Stations)
                {
                    if (currentStation.NumOfAvailableChargeSlots != 0) AvailableChargingStations.Add(currentStation);
                }
                return AvailableChargingStations;
            }
        }
    }
}

