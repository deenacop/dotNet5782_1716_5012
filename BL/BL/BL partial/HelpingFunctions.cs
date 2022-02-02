using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        #region Distance functions

        const double PI = Math.PI;
        const int RADIUS = 6371;//the earth radius
        /// <summary>
        /// A function that calculates the distance between two coordinates on the map
        /// </summary>
        /// <param name="from">The location from were we want to calculate the distance</param>
        /// <param name="to">The location to were we want to calculate</param>
        /// <returns>returns the distance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static double DistanceCalculation(Location from, Location to)
        {
            double radiusOfLon = (from.Longitude - to.Longitude) * PI / 180;
            double radiusOfLat = (from.Latitude - to.Latitude) * PI / 180;
            double havd = Math.Pow(Math.Sin(radiusOfLat / 2), 2) +
                (Math.Cos(to.Latitude)) * (Math.Cos(from.Latitude)) * Math.Pow(Math.Sin(radiusOfLon), 2);
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }

        /// <summary>
        /// Finds the minimum distance from a station to a location
        /// </summary>
        /// <param name="BaseStationListBL">list of stationBL</param>
        /// <param name="location">current location</param>
        /// <returns>Item 1:location/Item 2:distance/Item 3:StationID</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static (Location, double, int) MinDistanceLocation(List<BaseStation> BaseStationListBL, Location location)
        {
            List<double> locations = new();//Creates a list of locations
            foreach (BaseStation currentStation in BaseStationListBL)
            {
                locations.Add(DistanceCalculation(location, currentStation.Location));
            }
            return (BaseStationListBL[locations.FindIndex(i => i == locations.Min())].Location, locations.Min(), BaseStationListBL[locations.FindIndex(i => i == locations.Min())].Id);
        }
        #endregion

        /// <summary>
        /// checks if the ID of an item has the right amount of digits
        /// </summary>
        /// <param name="num">ID item</param>
        /// <returns>amount of digits in the ID</returns>  
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static int CheckNumOfDigits(int num) => (int)(Math.Round(Math.Floor(Math.Log10(num))) + 1);

        /// <summary>
        /// Function that checks if the drone has enough battery to do his delivary
        /// </summary>
        /// <param name="parcel">The parcel that needs to be picked up</param>
        /// <param name="drone">The drone that needs to do the delivary</param>
        /// <returns>true or false</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        bool BatteryCheckingForDroneAndParcel(DO.Parcel parcel, Drone drone)
        {
            int minBattery = 0;
            double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.Sender).Location);
            minBattery = (int)distance * (int)vacant;
            distance = DistanceCalculation(GetCustomer(parcel.Sender).Location, GetCustomer(parcel.Targetid).Location);
            minBattery = setBattery(minBattery, distance, (WeightCategories)parcel.Weight);
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
            minBattery += (int)MinDistanceLocation(BaseStationListBL, GetCustomer(parcel.Targetid).Location).Item2 * (int)vacant;
            if (minBattery <= drone.Battery)
                return true;
            return false;
        }

        /// <summary>
        /// The function regulates the battery and takes into account the weight and battery consumption accordingly
        /// </summary>
        /// <param name="battery">the battery that need to be set</param>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private int setBattery(int battery, double distance, WeightCategories weight)
        {
            switch (weight)
            {
                case WeightCategories.Light:
                    battery += (int)(distance * carriesLightWeight);
                    break;
                case WeightCategories.Medium:
                    battery += (int)(distance * carriesMediumWeight);
                    break;
                case WeightCategories.Heavy:
                    battery += (int)(distance * carriesHeavyWeight);
                    break;
            }
            return battery;
        }


        /// <summary>
        /// A function that helps arrange a single customer (Get customer function).
        ///The function arranges the parcel received / sent by the customer
        ///The function is called from a query
        /// </summary>
        /// <param name="item">a parcel(DO.Parcel)</param>
        /// <param name="customerBO">the customer we want to set</param>
        /// <returns>the parcel(ParcelByCustomer)</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        ParcelByCustomer HelpMethodToSetParcelInCustomer(DO.Parcel item, Customer customerBO)
        {
            ParcelByCustomer parcelBy = new();
            item.CopyPropertiesTo(parcelBy);
            if (item.Delivered == null)//item is already delivered
                parcelBy.Status = ParcelStatus.Delivered;
            else if (item.PickUp != null)//not delivere but already picked up
                parcelBy.Status = ParcelStatus.PickedUp;
            else if (item.Scheduled != null)//not delivere and not picked up but already assign
                parcelBy.Status = ParcelStatus.Associated;
            else//not delivere and not picked up and not assign
                parcelBy.Status = ParcelStatus.Defined;
            //check if the other side is the targetid or the sender
            if (item.Targetid == customerBO.Id)
            {
                parcelBy.SecondSideOfParcelCustomer = new()
                {
                    Id = item.Sender,
                    Name = dal.GetCustomer(item.Sender).Name
                };
            }
            else
            {
                parcelBy.SecondSideOfParcelCustomer = new()
                {
                    Id = item.Targetid,
                    Name = dal.GetCustomer(item.Targetid).Name
                };
            }
            return parcelBy;
        }
    }
}
