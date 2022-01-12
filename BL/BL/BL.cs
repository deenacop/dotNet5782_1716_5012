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
        

        List<DroneToList> DroneListBL;//The list of drones that we will maintain throughout the project

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

            DroneListBL = new List<DroneToList>();
            IEnumerable<DO.Drone> DroneListDL = dal.GetListDrone();//Receive the drone list from the data layer.
            DroneListDL.CopyPropertiesToIEnumerable(DroneListBL);//convret from DalApi to BL

            List<ParcelToList> ParcelListBL = new();
            IEnumerable<DO.Parcel> ParcelListDL = dal.GetListParcel(i => i.MyDroneID != 0);//Receive the parcel list of parcels that are assign to drone (from the data layer).
            ParcelListDL.CopyPropertiesToIEnumerable(ParcelListBL);//convret from DalApi to BL

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

            foreach (DroneToList currentDrone in DroneListBL)
            {
                try
                {
                    DO.Parcel parcelDO = ParcelListDL.First(item => item.MyDroneID == currentDrone.Id
                     && item.Delivered == null);//Finds the parcel which is assigned to the current drone *and* the drone has been assigned .
                                                //if !=-1 else: move to the catch block
                    #region DELIVERY MODE
                    currentDrone.Status = DroneStatus.Delivery;//in delivery mode (מבצע משלוח)

                    DO.Customer senderCustomer = dal.GetCustomer(parcelDO.Sender); //gets the sender customer

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
                    {
                        currentDrone.Location = locationOfSender;
                        currentDrone.ParcelId = parcelDO.Id;
                    }

                    //Battery status (מצב סוללה):
                    DO.Customer receiverCustomer = dal.GetCustomer(parcelDO.Targetid); //Found the customer that is the custumer that is targetid one
                    Location locationOfReceiver = new()
                    {
                        Latitude = receiverCustomer.Latitude,
                        Longitude = receiverCustomer.Longitude
                    };

                    //Finds the closest station from the targeted
                    double minDistance = MinDistanceLocation(BaseStationListBL, locationOfReceiver).Item2; //The distance between the targetid and the closest station
                    double distanceToTargeted = DistanceCalculation(locationOfReceiver, currentDrone.Location);//The distance between the current location to the targetid

                    int minBatteryDrone = 0;

                    switch ((int)currentDrone.Weight)
                    {
                        case (int)WeightCategories.Light:
                            minBatteryDrone = (int)(distanceToTargeted * carriesLightWeight);
                            break;
                        case (int)WeightCategories.Medium:
                            minBatteryDrone = (int)(distanceToTargeted * carriesMediumWeight);
                            break;
                        case (int)WeightCategories.Heavy:
                            minBatteryDrone = (int)(distanceToTargeted * carriesHeavyWeight);
                            break;
                    }
                    minBatteryDrone += (int)(minDistance * vacant);//minimum battery that the drone needs
                    currentDrone.Battery = rand.Next(minBatteryDrone, 101);//between minimum to maximum(=>100)
                }
                #endregion
                //If the drone is not currently shipping(אם הרחפן לא מבצע משלוח):
                catch (InvalidOperationException)
                {
                    IEnumerable<DO.Parcel> deliveredParcel = dal.GetListParcel(i => i.Delivered != null);//Receive the parcel list of parcels that are in delivery mode (from the data layer).
                    IEnumerable<DO.Station> availableStations = dal.GetListStation(i => i.NumOfAvailableChargingSlots > 0);//Receive the station list of stations that have available slots (from the data layer).
                    currentDrone.Status = (DroneStatus)rand.Next(0, 2);//available or maintenance(פנוי / תחזוקה)
                    #region In maintenance mode
                    if (currentDrone.Status == DroneStatus.Maintenance)
                    {//In maintenance(בתחזוקה):
                        DO.Station station = availableStations.Skip(rand.Next(0, availableStations.Count())).FirstOrDefault();//Randomly selects one of the available station
                        Location location1 = new()//set the location
                        {
                            Latitude = station.Latitude,
                            Longitude = station.Longitude
                        };
                        currentDrone.Location = location1;
                        currentDrone.Battery = rand.Next(0, 21);//Randomly selects a battery percentage between 0 and 20
                        dal.SendingDroneToChargingBaseStation(currentDrone.Id, station.Id);//Sends the drone for charging
                    }
                    #endregion

                    #region Available mode
                    if (currentDrone.Status == DroneStatus.Available)
                    {//Availabe (פנוי):
                        DO.Parcel parcel = deliveredParcel.Skip(rand.Next(0, deliveredParcel.Count())).First();//Randomly selects one of the delivered parcel
                        //After finding a parcel that was sent, asks to receive the location of the customer to whom the parcel was sent (in order to enter a logical location for the drone)
                        DO.Customer targetid = dal.GetCustomer(parcel.Targetid);
                        Location location2 = new()//set the location
                        {
                            Latitude = targetid.Latitude,
                            Longitude = targetid.Longitude
                        };
                        currentDrone.Location = location2;
                        //finds the closest station from the targeted
                        double minDistance = MinDistanceLocation(BaseStationListBL, currentDrone.Location).Item2;
                        //the minimum battery the drones needs
                        int minBatteryDrone = 0;
                        int weight = (int)currentDrone.Weight;
                        switch (weight)
                        {
                            case (int)WeightCategories.Light:
                                minBatteryDrone = (int)(minDistance * carriesLightWeight);
                                break;
                            case (int)WeightCategories.Medium:
                                minBatteryDrone = (int)(minDistance * carriesMediumWeight);
                                break;
                            case (int)WeightCategories.Heavy:
                                minBatteryDrone = (int)(minDistance * carriesHeavyWeight);
                                break;
                        }
                        //if (minBatteryDrone == 0)
                        //    currentDrone.Battery = 30;
                        currentDrone.Battery = rand.Next(minBatteryDrone, 101);//between minimum to maximum(=>100)
                        #endregion
                    }
                }
            }
        }
    }
}
