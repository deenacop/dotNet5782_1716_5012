using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace BL
{
    public partial class BL
    {
        #region Distance
        const double PI = Math.PI;
        const int RADIUS = 6371;//the earth radius
        /// <summary>
        /// calculate distance between tow different location
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
        /// Finds the minimum distance from the Station to a location
        /// </summary>
        /// <param name="BaseStationListBL">list of stationBL</param>
        /// <param name="location">current location</param>
        /// <returns>location/distance/StationID</returns>
        private (Location, double, int) MinDistanceLocation(List<BaseStation> BaseStationListBL, Location location)
        {
            List<double> locations = new ();
            foreach (BaseStation currentStation in BaseStationListBL)
            {
                locations.Add(DistanceCalculation(location, currentStation.StationLocation));
            }
            return (BaseStationListBL[locations.FindIndex(i => i == locations.Min())].StationLocation, locations.Min(), BaseStationListBL[locations.FindIndex(i => i == locations.Min())].StationID);
        }
        #endregion

        /// <summary>
        /// checks if the ID of the item has the right amount of digits
        /// </summary>
        /// <param name="num">ID item</param>
        /// <returns>amount of digits in the ID</returns>       
        internal static int ChackingNumOfDigits(int num)
        {
            return (int)(Math.Round(Math.Floor(Math.Log10(num))) + 1);
        }
        /// <summary>
        /// Function that checks if a drone can carry a parcel according to there weight
        /// </summary>
        /// <param name="parcel">The parcel that needs to be picked up</param>
        /// <param name="drone">The drone that needs to do the delivary</param>
        /// <returns>true or false</returns>        
        static bool legalParcel(IDAL.DO.Parcel parcel, Drone drone)
        {
            if ((int)drone.Weight < (int)parcel.Weight)//not legal parcel for this drone
                return false;

            return true;
        }
        /// <summary>
        /// Function that checks if the drone has enough battery to do his delivary
        /// </summary>
        /// <param name="parcel">The parcel that needs to be picked up</param>
        /// <param name="drone">The drone that needs to do the delivary</param>
        /// <returns>true or false</returns>
        bool BatteryCheckingForDroneAndParcel(IDAL.DO.Parcel parcel, Drone drone)
        {
            int minBattery;
            double distance = DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(parcel.Sender).CustomerLocation);
            minBattery = (int)distance * (int)vacant;
            distance = DistanceCalculation(CustomerDisplay(parcel.Sender).CustomerLocation, CustomerDisplay(parcel.Targetid).CustomerLocation);
            switch ((int)parcel.Weight)//calculate from the sender to the targetid
            {
                case (int)WeightCategories.Light:
                    minBattery += (int)(distance * carriesLightWeight);
                    break;
                case (int)WeightCategories.Midium:
                    minBattery += (int)(distance * carriesMediumWeight);
                    break;
                case (int)WeightCategories.Heavy:
                    minBattery += (int)(distance * carriesHeavyWeight);
                    break;
            }
            List<BaseStation> BaseStationListBL = null;
            List<IDAL.DO.Station> StationListDL = dal.ListStationDisplay().ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesTo(BaseStationListBL);//convret from IDAT to IBL
            minBattery += (int)MinDistanceLocation(BaseStationListBL, CustomerDisplay(parcel.Targetid).CustomerLocation).Item2 * (int)vacant;
            if (minBattery <= drone.Battery)
                return true;
            return false;
        }
    }
}
