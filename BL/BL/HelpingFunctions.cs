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
        internal double DistanceCalculation(Location from, Location to)
        {
            double radiusOfLon = (from.Longitude - to.Longitude) * PI / 180;
            double radiusOfLat = (from.Latitude - to.Latitude) * PI / 180;
            double havd = Math.Pow(Math.Sin(radiusOfLat / 2), 2) +
                (Math.Cos(to.Latitude)) * (Math.Cos(from.Latitude)) * Math.Pow(Math.Sin(radiusOfLon), 2);
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }
        #endregion

        /// <summary>
        /// checks if the ID of the item has the right amount of digits
        /// </summary>
        /// <param name="num">ID item</param>
        /// <returns>amount of digits in the ID</returns>
        internal int ChackingNumOfDigits(int num)
        {
            return (int)(Math.Round(Math.Floor(Math.Log10(num))) + 1); 
        }

        #region Find
        T FindBaseStation(int ID)
        {
            List<T> BaseStationListBL = null;
            IEnumerable<IDAL.DO.Station> StationListDL = dal.ListStationDisplay();//Receive the station from the data layer.
            BaseStationListBL.CopyPropertiesTo(StationListDL);//convret from IDAT to IBL
            //if(BaseStationListBL.)
                //////////////////////////////////////
                return BaseStationListBL.Find(item => item.StationID == ID);
        }

        /// <summary>
        /// Finds the minimum distance from the Station to a location
        /// </summary>
        /// <param name="BaseStationListBL">list of stationBL</param>
        /// <param name="location">current location</param>
        /// <returns>location/distance</returns>
        private (Location, double) MinDistanceLocation(List<BaseStation> BaseStationListBL, Location location)
        {
            List<double> locations = new List<double>();
            foreach (BaseStation currentStation in BaseStationListBL)
            {
                locations.Add(DistanceCalculation(location, currentStation.StationLocation));
            }
            return (BaseStationListBL[locations.FindIndex(i => i == locations.Min())].StationLocation, locations.Min());
        }
        #endregion 

    }
}
