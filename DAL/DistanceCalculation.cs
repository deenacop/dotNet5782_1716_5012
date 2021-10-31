﻿using System;
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
                double RadiusOfLon = (lon1 - wanted.Longitude) * PI / 180;
                double RadiusOfLat = (lat1 - wanted.Latitude) * PI / 180;
                double havd = Math.Pow(Math.Sin(RadiusOfLat / 2), 2) + 
                    (Math.Cos(wanted.Latitude)) * (Math.Cos(lat1)) * Math.Pow(Math.Sin(RadiusOfLon), 2);
                double distance = 2 * RADIUS * Math.Asin(havd);
                return distance;
            }
            public static double Calculation(double lon1, double lat1, Station wanted)
            {
                double RadiusOfLon = (lon1 - wanted.Longitude) * PI / 180;
                double RadiusOfLat = (lat1 - wanted.Latitude) * PI / 180;
                double havd = Math.Pow(Math.Sin(RadiusOfLat / 2), 2) + 
                    (Math.Cos(wanted.Latitude)) * (Math.Cos(lat1)) * Math.Pow(Math.Sin(RadiusOfLon), 2);
                double distance = 2 * RADIUS * Math.Asin(havd);
                return distance;
            }
            public static Customer FindTheCustomerCoordinates(int ID)
            {
                Customer ForNotFoundCase = new Customer();
                ForNotFoundCase.ID = 0;
                for (int i = 0; i < DataSource.Config.FirstAvailableCustomer; i++)
                    if (ID == DataSource.Customers[i].ID)
                        return DataSource.Customers[i];
                return ForNotFoundCase;
            }
            public static Station FindTheStationCoordinates(int ID)
            {
                Station ForNotFoundCase = new Station();
                ForNotFoundCase.ID = 0;
                for (int i = 0; i < DataSource.Config.FirstAvailableStation; i++)
                    if (ID == DataSource.Stations[i].ID)
                        return DataSource.Stations[i];
                return ForNotFoundCase;
            }
        }
    }
}
