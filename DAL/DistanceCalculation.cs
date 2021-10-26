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
            public static float Calculate(double lat1, double lon1,int ID)
            {
                double[] Coordinates = FindTheCoordinates(ID);
                double Distance;
                double latitude = lat1 - Coordinates[0];//Latitude difference
                double longitude = lon1 - Coordinates[1]; //Longitude difference
                latitude = Math.Pow(latitude, 2);//To the power of 2 to reach the Pythagorean theorem
                longitude = Math.Pow(longitude, 2);//To the power of 2 to reach the Pythagorean theorem
                Distance = latitude + longitude;//a^2+b^2=D^2
               return (float)Math.Sqrt(Distance);//Perform a Sqrt operation on the result and convert to float
            }
            public static double[] FindTheCoordinates(int ID)
            {
                double[] coordinates=new double[2];
                int t = ID -999999;
                if (ID - 999999 >0)//Customer -999999 =>0 but Station -999999=<0
                {
                    for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                        if (ID == DataSource.Customers[i].ID)
                        {
                            coordinates[0] = DataSource.Customers[i].Latitude;
                            coordinates[1] = DataSource.Customers[i].Longitude;
                        }
                }
                else//station
                {
                    for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                        if (ID == DataSource.Stations[i].ID)
                        {
                            coordinates[0] = DataSource.Stations[i].Latitude;
                            coordinates[1] = DataSource.Stations[i].Longitude;
                        }
                }
                return coordinates;
            }
        }
    }
}
