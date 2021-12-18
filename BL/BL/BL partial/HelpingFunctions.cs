using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;



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
        private static (Location, double, int) MinDistanceLocation(List<BaseStation> BaseStationListBL, Location location)
        {
            List<double> locations = new ();//Creates a list of locations
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
        internal static int CheckNumOfDigits(int num)
        {
            return (int)(Math.Round(Math.Floor(Math.Log10(num))) + 1);
        }

        /// <summary>
        /// Function that checks if the drone has enough battery to do his delivary
        /// </summary>
        /// <param name="parcel">The parcel that needs to be picked up</param>
        /// <param name="drone">The drone that needs to do the delivary</param>
        /// <returns>true or false</returns>
        bool BatteryCheckingForDroneAndParcel(DO.Parcel parcel, Drone drone)
        {
            int minBattery=0;
            double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.Sender).Location);
            minBattery = (int)distance * (int)vacant;
            distance = DistanceCalculation(GetCustomer(parcel.Sender).Location, GetCustomer(parcel.Targetid).Location);
            switch ((int)parcel.Weight)//calculate from the sender to the targetid
            {
                case (int)WeightCategories.Light:
                    minBattery += (int)(distance * carriesLightWeight);
                    break;
                case (int)WeightCategories.Medium:
                    minBattery += (int)(distance * carriesMediumWeight);
                    break;
                case (int)WeightCategories.Heavy:
                    minBattery += (int)(distance * carriesHeavyWeight);
                    break;
            }
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
    }
}
