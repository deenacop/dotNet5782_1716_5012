﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Mail;
using System.Net;
using BO;
using BL;
using BlApi;
using System.ComponentModel;
using System.Threading;

namespace BL
{
    internal class Simulation
    {
        enum Maintenance { Finding, Going, Charging }
        private const int VELOCITY = 100;//drone speed
        private const int DELAY = 2000;//delay time
        private const double SECONDS_PASSED = DELAY / 1000.0;//Seconds passed
        private const double WayTraveled = VELOCITY * SECONDS_PASSED;//How much distance has passed

        private BL bl;
        private int droneId;
        private Func<bool> func;
        private Action reportProgress;

        public Simulation(BL Bl, int droneId, Func<bool> threadStop, Action reportProgress)
        {
            this.bl = Bl;
            this.droneId = droneId;
            this.func = threadStop;
            var idal = bl.dal;
            this.reportProgress = reportProgress;
            DroneToList drone = bl.GetDroneList(item=>item.Id== droneId).First();
            int? parcelId = null;
            int? StationId = null;
            BaseStation station = null;
            double distance = 0.0;
            double baterryUsage = 0;
            DO.Parcel parcel=new();
            bool pickedUp = false;
            Customer customer = null;
            Maintenance maintenance = Maintenance.Finding;//available

            void initDelivery(int Id)//if the drone is in delivery mode
            {
                parcel = idal.GetParcel(Id);
                baterryUsage = idal.ChargingDrone()[(int)((DO.Parcel)parcel).Weight + 1];
                pickedUp = parcel.PickUp is not null;//if the parcel isnt picked up, so pickup=null.
                customer = bl.GetCustomer((pickedUp ? parcel.Targetid : parcel.Sender));//brings the customer the drone id at
                //if he didnt pick up the parcel he is by the sender else he is by the sender
            }
            do
            { 
                switch (drone)
                {
                    case DroneToList { Status: DroneStatus.Available }:
                        if (!sleepDelayTime()) break;
                        lock (bl)
                        {
                            try
                            {
                                bl.AssignParcelToDrone(bl.GetDrone(droneId));
                                parcelId = drone.ParcelId;
                            }
                            catch (ItemNotExistException) { }
                      
                            //gets the list of stations 
                            List<BaseStation> BaseStationListBL = new();
                            IEnumerable<DO.Station> StationListDL = idal.GetListStation();//Receive the station list from the data layer.
                            StationListDL.CopyPropertiesToIEnumerable(BaseStationListBL);//convret from DalApi to BL
                                                                                         //Set the locations:
                            IEnumerable<int> counter = Enumerable.Range(0, StationListDL.Count());
                            foreach (int j in counter)
                            {
                                BaseStationListBL.ElementAt(j).Location = new()
                                {
                                    Longitude = StationListDL.ElementAt(j).Longitude,
                                    Latitude = StationListDL.ElementAt(j).Latitude
                                };
                            }

                            switch (parcelId, drone.Battery)//checks the drone's battery and parcel if they are null
                            {
                                case (null, 100)://no parcel and full battery
                                    drone.Status = DroneStatus.Available;
                                    break;
                                case (null, _)://no parcel and not full battery
                                    StationId = bl.MinDistanceLocation(BaseStationListBL,drone.Location).Item3;
                                    if (StationId != null)
                                    {
                                        drone.Status = DroneStatus.Maintenance;
                                        maintenance = Maintenance.Finding;
                                        // bl.idal1.
                                        bl.dal.SendingDroneToChargingBaseStation(droneId, (int)StationId);
                                    }
                                    break;
                                case (_, _)://parcel and not full battery
                                    try
                                    {
                                        initDelivery((int)parcelId);
                                        drone.Status = DroneStatus.Delivery;
                                    }
                                    catch (DO.ItemNotExistException ex)
                                    {
                                        throw new ItemNotExistException("Internal error getting parcel or customer", ex);
                                    }
                                    break;
                            }
                        }
                        break;
                    case DroneToList { Status: DroneStatus.Maintenance }: // if the drone is in maintenance
                        switch (maintenance)
                        {
                            case Maintenance.Finding:
                                lock (Bl)
                                {
                                    try
                                    {
                                        station = Bl.GetBaseStation(StationId ?? idal.GetListDroneCharge(dc => dc.Id == drone.Id).First().BaseStationID);
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        throw new ItemNotExistException("Could not find wanted station", ex);
                                    }
                                    distance = bl.DistanceCalculation(drone.Location,  station.Location);
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                if (distance < 0.01)// if its 'at' the station
                                    lock (Bl)
                                    {
                                        drone.Location = station.Location;
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!sleepDelayTime())
                                        break;
                                    lock (Bl)
                                    {
                                        double actualDistance = distance < WayTraveled ? distance : WayTraveled;//checks if the drone has enough battery to do the delivery
                                        distance -= actualDistance;
                                        drone.Battery = (int)Math.Max(0, drone.Battery - ((int)Math.Round(actualDistance) * idal.ChargingDrone()[0]));
                                    }
                                }
                                break;

                            case Maintenance.Charging:
                                if (drone.Battery == 100)
                                    lock (Bl)
                                    {
                                        Bl.ReleasingDroneFromBaseStation(bl.GetDrone(droneId));
                                        drone.Status = DroneStatus.Available;
                                    }
                                else
                                {
                                    if (!sleepDelayTime())
                                        break;
                                    lock (Bl)
                                    {
                                        double batteryCharge = (SECONDS_PASSED) * idal.ChargingDrone()[4];// we have to change the dronepwrusg
                                        drone.Battery = Math.Min(100, drone.Battery + (int)batteryCharge);
                                    }
                                }
                                break;
                            default:
                                throw new NotEnoughBatteryException("Internal error:wrong maintenace substat");
                        }
                        break;
                    case DroneToList { Status: DroneStatus.Delivery }:
                        lock (Bl)
                        {
                            initDelivery(drone.ParcelId);
                            distance = bl.DistanceCalculation(drone.Location, customer.Location);
                        }
                        //checks if the drone has enough battery to do the delivery
                        if (distance < 0.01 || drone.Battery <= drone.Battery - bl.setBattery(drone.Battery,distance, pickedUp ? bl.GetParcel(parcel.Id).Weight +1 : WeightCategories.Light)+3 )
                            lock (Bl)
                            {
                                drone.Location = customer.Location;
                                if (pickedUp)
                                {
                                    bl.DeliveryParcelByDrone(bl.GetDrone(droneId));
                                    drone.Status = DroneStatus.Available;
                                    parcel = new();
                                    parcelId = null;
                                }
                                else
                                {
                                    bl.CollectionParcelByDrone(bl.GetDrone(droneId));
                                    customer = Bl.GetCustomer(parcel.Targetid);
                                    pickedUp = true;
                                }
                            }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock (Bl)
                            {//updates the drone
                                double actualDistance = distance < WayTraveled ? distance : WayTraveled;
                                double Proportion = actualDistance / distance;
                                int usg = pickedUp ? ((int)((DO.Parcel)parcel).Weight + 1) : 0;
                                drone.Battery =(int)Math.Max(0, drone.Battery - ((int)Math.Round(actualDistance) * idal.ChargingDrone()[usg]));
                                double lat = drone.Location.Latitude + (customer.Location.Latitude - drone.Location.Latitude) * Proportion;
                                double lon = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * Proportion;
                                drone.Location = new() { Latitude = lat, Longitude = lon };
                            }
                        }
                        break;

                    default:
                        throw new WrongInputException("Internal error:no suitable status");
                }
                reportProgress();
            } while (!threadStop());
        }
        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); }
            catch (ThreadInterruptedException)
            {
                return false;
            }
            return true;
        }
    }
}