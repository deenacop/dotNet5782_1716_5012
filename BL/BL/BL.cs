﻿using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        internal static IDal dal = new DalObject.DalObject();

        static internal readonly Random rand = new(DateTime.Now.Millisecond);

        readonly List<DroneToList> DroneListBL;
        readonly double vacant,
             carriesLightWeight,
            carriesMediumWeight,
            carriesHeavyWeight,
            droneLoadingRate;
        #region ctor
        public BL()
        {
            #region Electricity use
            vacant = dal.ChargingDrone().ElementAt(0);
            carriesLightWeight = dal.ChargingDrone().ElementAt(1);
            carriesMediumWeight = dal.ChargingDrone().ElementAt(2);
            carriesHeavyWeight = dal.ChargingDrone().ElementAt(3);
            droneLoadingRate = dal.ChargingDrone().ElementAt(4);
            #endregion

            #region Brings lists from IDAL

            DroneListBL = new List<DroneToList>();
            List<IDAL.DO.Drone> DroneListDL = dal.ListDroneDisplay().ToList();//Receive the drone list from the data layer.
            DroneListDL.CopyPropertiesToIEnumerable(DroneListBL);//convret from IDAT to IBL

            List<ParcelToList> ParcelListBL = new();
            //Receive the parcel list of parcels that are assign to drone (from the data layer).
            List<IDAL.DO.Parcel> ParcelListDL = dal.ListParcelDisplay(i => i.MyDroneID != 0).ToList();
            ParcelListDL.CopyPropertiesToIEnumerable(ParcelListBL);//convret from IDAT to IBL

            List<IDAL.DO.Customer> CustomerListDL = dal.ListCustomerDisplay().ToList();//Receive the customer list from the data layer.


            List<BaseStation> BaseStationListBL = new();
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay().ToList();//Receive the drone list from the data layer.

            StationListDL.CopyPropertiesToIEnumerable(BaseStationListBL);//convret from IDAT to IBL

            for (int i = 0; i < StationListDL.Count; i++)
            {
                BaseStationListBL[i].StationLocation = new Location { Latitude = StationListDL[i].Latitude, Longitude = StationListDL[i].Longitude };
            }
            #endregion

            foreach (DroneToList currentDrone in DroneListBL)
            {
                int index = ParcelListDL.FindIndex(item => item.MyDroneID == currentDrone.DroneID
                && item.Delivered == DateTime.MinValue);//finds the parcel which is assigned to the current drone and the drone has been assigned .
                if (index != -1)
                {
                    currentDrone.DroneStatus = DroneStatus.Delivery;//מבצע משלוח

                    IDAL.DO.Customer senderCustomer = CustomerListDL.Find(i => i.CustomerID == ParcelListDL[index].Sender);//sender customer

                    Location locationOfSender = new()
                    {
                        Latitude = senderCustomer.Latitude,
                        Longitude = senderCustomer.Longitude
                    };

                    if (ParcelListDL[index].PickUp == DateTime.MinValue)//שויכה ולא נאספה
                    {
                        //finds the closest station from the sender
                        currentDrone.MyCurrentLocation = MinDistanceLocation(BaseStationListBL, locationOfSender).Item1;
                    }
                    else
                    {
                        currentDrone.MyCurrentLocation = locationOfSender;
                        currentDrone.ParcelNumberTransfered = ParcelListDL[index].ParcelID;
                    }

                    //מצב סוללה:
                    IDAL.DO.Customer receiverCustomer = CustomerListDL.Find(item => item.CustomerID == ParcelListDL[index].Targetid);//found the customer that is the targetid one
                    Location locationOfReceiver = new()
                    {
                        Latitude = receiverCustomer.Latitude,
                        Longitude = receiverCustomer.Longitude
                    };

                    //finds the closest station from the targeted
                    double minDistance = MinDistanceLocation(BaseStationListBL, locationOfReceiver).Item2; //from the targetid to the closest station
                    double distanceToTargeted = DistanceCalculation(locationOfReceiver, currentDrone.MyCurrentLocation);//from the current location to the targetid

                    int minBatteryDrone = 0;
                    switch ((int)currentDrone.Weight)
                    {
                        case (int)WeightCategories.Light:
                            minBatteryDrone = (int)(distanceToTargeted * carriesLightWeight);
                            break;
                        case (int)WeightCategories.Midium:
                            minBatteryDrone = (int)(distanceToTargeted * carriesMediumWeight);
                            break;
                        case (int)WeightCategories.Heavy:
                            minBatteryDrone = (int)(distanceToTargeted * carriesHeavyWeight);
                            break;
                    }
                    minBatteryDrone += (int)(minDistance * vacant);//minimum battery the drone needs
                    currentDrone.Battery = rand.Next(minBatteryDrone, 101);
                }
                //אם הרחפן לא מבצע משלוח:
                else
                {
                    List<IDAL.DO.Parcel> deliveredParcel = dal.ListParcelDisplay(i => i.Delivered != DateTime.MinValue).ToList();//lists of all the delivered parcels
                    List<IDAL.DO.Station> availableStations = dal.ListStationDisplay(i => i.NumOfAvailableChargingSlots > 0).ToList();//lists of all the available stations
                    currentDrone.DroneStatus = (DroneStatus)rand.Next(0, 2);//פנוי לתחזוקה
                    if (currentDrone.DroneStatus == DroneStatus.Maintenance)//if the drone is not in maintenance mode
                    {//בתחזוקה
                        index = rand.Next(0, availableStations.Capacity);//one of the staitions
                        Location location1 = new()
                        {
                            Latitude = availableStations[index].Latitude,
                            Longitude = availableStations[index].Longitude
                        };
                        currentDrone.MyCurrentLocation = location1;
                        IDAL.DO.Station tmp = availableStations[index];
                        if (tmp.NumOfAvailableChargingSlots - 1 == 0)
                            availableStations.RemoveAt(index);
                        else
                        {
                            tmp.NumOfAvailableChargingSlots -= 1;
                            availableStations[index] = tmp;
                        }
                        currentDrone.Battery = rand.Next(0, 21);
                         dal.SendingDroneToChargingBaseStation(currentDrone.DroneID, availableStations[index].StationID);

                    }
                    if (currentDrone.DroneStatus == DroneStatus.Available)//if the drone is not in maintenance mode
                    {//פנוי
                        index = rand.Next(0, deliveredParcel.Capacity);//one of the staitions

                        IDAL.DO.Customer targetid = CustomerListDL.Find(item => item.CustomerID == deliveredParcel[index].Targetid);
                        Location location2 = new()
                        {
                            Latitude = targetid.Latitude,
                            Longitude = targetid.Longitude
                        };
                        currentDrone.MyCurrentLocation = location2;
                        //finds the closest station from the targeted
                        double minDistance = MinDistanceLocation(BaseStationListBL, currentDrone.MyCurrentLocation).Item2;
                        //the minimum battery the drones needs
                        int minBatteryDrone = 0;
                        int weight = (int)currentDrone.Weight;
                        switch (weight)
                        {
                            case (int)WeightCategories.Light:
                                minBatteryDrone = (int)(minDistance * carriesLightWeight);
                                break;
                            case (int)WeightCategories.Midium:
                                minBatteryDrone = (int)(minDistance * carriesMediumWeight);
                                break;
                            case (int)WeightCategories.Heavy:
                                minBatteryDrone = (int)(minDistance * carriesHeavyWeight);
                                break;
                        }
                        if (minBatteryDrone == 0)
                            currentDrone.Battery = 30;
                        currentDrone.Battery = rand.Next(minBatteryDrone, 101);
                    }
                }
            }

        }
        #endregion
    }
}
