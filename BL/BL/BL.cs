using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DalApi;
using BlApi;


namespace BL
{
    internal partial class BL : IBL
    {
        //Variables into which we will later enter the data on battery consumption
        readonly double vacant,
               carriesLightWeight,
               carriesMediumWeight,
               carriesHeavyWeight,
               droneLoadingRate;

        private static readonly IDal dal = DalFactory.GetDal();//instance of IDal
        private static readonly Lazy<BL> instance = new Lazy<BL>(() => new BL()); //Lazy initialization of an object means that its creation is deferred until it is first used.

        public static BL Instance { get { return instance.Value; } }// The internal Instance property to use


        List<DroneToList> DronesBL;//The list of drones that we will maintain throughout the project
        //I chose to use the list and not IEnumerable, because IEnumerable mainly intended for viewing records and not for * maintaining * lists.
        //The purpose of this list is to maintain the list of drones in the logical layer - the BL layer-  so I used the list!

        //static BL() { }// default => private
        private BL()
        {
            Random rand = new(DateTime.Now.Millisecond);//Random variable for use along the constructor
            //Brings all the details we entered in the datasource file- about battery consumption
            double[] tmpArr = dal.ChargingDrone();
            vacant = tmpArr[0];
            carriesLightWeight = tmpArr[1];
            carriesMediumWeight = tmpArr[2];
            carriesHeavyWeight = tmpArr[3];
            droneLoadingRate = tmpArr[4];

            #region Brings some of the lists that are needed from IDAL

            DronesBL = new List<DroneToList>();
            dal.GetListDrone().CopyPropertiesToIEnumerable(DronesBL);//convret from DalApi to BL

            List<ParcelToList> ParcelListBL = new();
            IEnumerable<DO.Parcel> ParcelListDL = dal.GetListParcel(i => i.MyDroneID != 0);//Receive the parcel list of parcels that are assign to drone (from the data layer).
            dal.GetListParcel(i => i.MyDroneID != 0).CopyPropertiesToIEnumerable(ParcelListBL);//convret from DalApi to BL

            List<BaseStation> BaseStationListBL = new();
            IEnumerable<DO.Station> StationListDL = dal.GetListStation();//Receive the station list from the data layer.
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
            #endregion

            foreach (DroneToList currentDrone in DronesBL)
            {
                try
                {
                    DO.Parcel parcelDO = ParcelListDL.First(item => item.MyDroneID == currentDrone.Id
                     && item.Delivered == null);//Finds the parcel which is assigned to the current drone *and* the drone has been assigned .
                                                //if !=-1 else: move to the catch block
                    #region DELIVERY MODE
                    currentDrone.Status = DroneStatus.Delivery;//in delivery mode (מבצע משלוח)
                    currentDrone.ParcelId = parcelDO.Id;

                    DO.Customer senderCustomer = dal.GetCustomer(parcelDO.Sender); //gets the sender customer (for setting the location)

                    Location locationOfSender = new()
                    {
                        Latitude = senderCustomer.Latitude,
                        Longitude = senderCustomer.Longitude
                    };

                    if (parcelDO.PickUp == null)//parcel was associated and not collected (שויכה ולא נאספה)
                    {
                        //finds the closest station from the sender
                        currentDrone.Location = MinDistanceLocation(BaseStationListBL, locationOfSender).Item1;
                    }
                    else
                        currentDrone.Location = locationOfSender;

                    //Battery status (מצב סוללה):
                    DO.Customer receiverCustomer = dal.GetCustomer(parcelDO.Targetid); //Found the customer that is the custumer that is targetid one (for setting the battery)
                    Location locationOfReceiver = new()
                    {
                        Latitude = receiverCustomer.Latitude,
                        Longitude = receiverCustomer.Longitude
                    };

                    //Finds the closest station from the targeted
                    double minDistance = MinDistanceLocation(BaseStationListBL, locationOfReceiver).Item2; //The distance between the targetid and the closest station
                    double distanceToTargeted = DistanceCalculation(locationOfReceiver, currentDrone.Location);//The distance between the current location to the targetid

                    int minBatteryDrone = 0;
                    minBatteryDrone = setBattery(minBatteryDrone, distanceToTargeted, currentDrone.Weight);
                    minBatteryDrone += (int)(minDistance * vacant);//minimum battery that the drone needs
                    currentDrone.Battery = rand.Next(minBatteryDrone, 101);//between minimum to maximum(=>100)
                }
                #endregion
                //If the drone is not currently shipping(אם הרחפן לא מבצע משלוח):
                catch (InvalidOperationException)
                {
                    //IEnumerable<DO.Station> availableStations = dal.GetListStation(i => i.NumOfAvailableChargingSlots > 0);//Receive the station list of stations that have available slots (from the data layer).
                    //currentDrone.Status = (DroneStatus)rand.Next(0, 2);
                    //if (currentDrone.Status == DroneStatus.Maintenance)
                    //DO.Station station = availableStations.Skip(rand.Next(0, availableStations.Count())).FirstOrDefault();//Randomly selects one of the available station
                    //Location location1 = new()//set the location
                    //{
                    //    Latitude = station.Latitude,
                    //    Longitude = station.Longitude
                    //};
                    //currentDrone.Location = location1;
                    //currentDrone.Battery = rand.Next(0, 21);//Randomly selects a battery percentage between 0 and 20
                    //dal.SendingDroneToChargingBaseStation(currentDrone.Id, station.Id);//Sends the drone for charging                                                                                                      //available or maintenance(פנוי / תחזוקה)
                    #region In maintenance mode
                    try
                    {
                        DO.DroneCharge droneInCharging = dal.GetListDroneCharge(i => i.Id == currentDrone.Id).First();
                        //In maintenance(בתחזוקה):
                        currentDrone.Status = DroneStatus.Maintenance;
                        DO.Station station = dal.GetStation(droneInCharging.BaseStationID);//finds the station that the drone is charged in
                        Location location1 = new()//set the location
                        {
                            Latitude = station.Latitude,
                            Longitude = station.Longitude
                        };
                        currentDrone.Location = location1;
                        currentDrone.Battery = rand.Next(0, 21);//Randomly selects a battery percentage between 0 and 20
                        currentDrone.ParcelId = 0;
                    }
                    #endregion
                    #region Available mode
                    catch (InvalidOperationException)//AVAILABLE
                    {
                        currentDrone.Status = DroneStatus.Available;
                        currentDrone.ParcelId = 0;
                        IEnumerable<DO.Parcel> deliveredParcel = dal.GetListParcel(i => i.Delivered != null);//Receive the parcel list of parcels that are in delivery mode (from the data layer)

                        //Randomly selects one of the delivered parcel
                        //After finding a parcel that was sent, asks to receive the location of the
                        //customer to whom the parcel was sent (in order to enter a logical location for the drone)
                        DO.Parcel parcel = deliveredParcel.Skip(rand.Next(0, deliveredParcel.Count())).First();
                        //for finds the location
                        DO.Customer targetid = dal.GetCustomer(parcel.Targetid);
                        Location location2 = new()//set the location
                        {
                            Latitude = targetid.Latitude,
                            Longitude = targetid.Longitude
                        };
                        currentDrone.Location = location2;
                        //finds the closest station from the targetid to be sure that we will not set an unlogical battery
                        double minDistance = MinDistanceLocation(BaseStationListBL, currentDrone.Location).Item2;
                        //the minimum battery the drones needs
                        int minBatteryDrone = setBattery(0, minDistance, currentDrone.Weight);
                        currentDrone.Battery = rand.Next(minBatteryDrone, 101);//between minimum to maximum(=>100)
                    }
                    #endregion
                    //if (currentDrone.Status == DroneStatus.Available)
                    //{//Availabe (פנוי):
                    //    DO.Parcel parcel = deliveredParcel.Skip(rand.Next(0, deliveredParcel.Count())).First();//Randomly selects one of the delivered parcel
                    //    //After finding a parcel that was sent, asks to receive the location of the customer to whom the parcel was sent (in order to enter a logical location for the drone)
                    //    DO.Customer targetid = dal.GetCustomer(parcel.Targetid);
                    //    Location location2 = new()//set the location
                    //    {
                    //        Latitude = targetid.Latitude,
                    //        Longitude = targetid.Longitude
                    //    };
                    //    currentDrone.Location = location2;
                    //    //finds the closest station from the targeted
                    //    double minDistance = MinDistanceLocation(BaseStationListBL, currentDrone.Location).Item2;
                    //    //the minimum battery the drones needs
                    //    int minBatteryDrone = 0;
                    //    minBatteryDrone = setBattery(minBatteryDrone, minDistance, currentDrone.Weight);
                    //    //if (minBatteryDrone == 0)
                    //    //    currentDrone.Battery = 30;
                    //    currentDrone.Battery = rand.Next(minBatteryDrone, 101);//between minimum to maximum(=>100)
                }
            }
        }
    }
}

