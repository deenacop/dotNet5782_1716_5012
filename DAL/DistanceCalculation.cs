using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Calculate the distance beteen tow coordinates
        /// </summary>
        public class DistanceCalculation
        {
            const double PI = Math.PI;
            const int RADIUS = 6371;//the earth radius
            public static double Calculation(double lon1,double lat1,Customer wanted)
            {
                double radiusOfLon = (lon1 - wanted.Longitude) * PI / 180;
                double radiusOfLat = (lat1 - wanted.Latitude) * PI / 180;
                double havd = Math.Pow(Math.Sin(radiusOfLat / 2), 2) + 
                    (Math.Cos(wanted.Latitude)) * (Math.Cos(lat1)) * Math.Pow(Math.Sin(radiusOfLon), 2);
                double distance = 2 * RADIUS * Math.Asin(havd);
                return distance;
            }
            public static double Calculation(double lon1, double lat1, Station wanted)
            {
                double radiusOfLon = (lon1 - wanted.Longitude) * PI / 180;
                double radiusOfLat = (lat1 - wanted.Latitude) * PI / 180;
                double havd = Math.Pow(Math.Sin(radiusOfLat / 2), 2) + 
                    (Math.Cos(wanted.Latitude)) * (Math.Cos(lat1)) * Math.Pow(Math.Sin(radiusOfLon), 2);
                double distance = 2 * RADIUS * Math.Asin(havd);
                return distance;
            }
            public static Customer FindTheCustomerCoordinates(int ID)
            {
                Customer forNotFoundCase = new Customer();//for not found case
                forNotFoundCase.ID = 0;
                IEnumerator<Customer> iter = DataSource.Customers.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (ID == iter.Current.ID)
                        return iter.Current;
                }
                return forNotFoundCase;
            }
            public static Station FindTheStationCoordinates(int ID)
            {
                Station forNotFoundCase = new Station();
                forNotFoundCase.ID = 0;
                IEnumerator<Station> iter = DataSource.Stations.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (ID == iter.Current.ID)
                        return iter.Current;
                }
                return forNotFoundCase;
            }
        }
    }
}
