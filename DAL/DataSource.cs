using System;
using IDAL.DO;
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Creates new arrays for each type, and initializes them with variables.
        /// </summary>
        public class DataSource
        {
            static readonly Random rand = new(DateTime.Now.Millisecond);
            //Builds arrays for all the entities
            static internal Drone[] Drones = new Drone[10];
            static internal Station[] Stations = new Station[5];
            static internal Customer[] Customers = new Customer[100];
            static internal Parcel[] Parcels = new Parcel[1000];

            /// <summary>
            ///Defines an index variable for each array that indicates a free space.
            /// </summary>
            internal class Config
            {
                static internal int FirstAvailableDrone = 0;
                static internal int firstAvailableStation = 0;
                static internal int firstAvailableCustomer = 0;
                static internal int firstAvailableParcel = 0;
                static internal int RunnerIDNumParcels = 0;
            }

            public static void Initialize()
            {
                
                
                for (int i = 2; i > 0; i--)
                {
                    Stations[i].ID = rand.Next();
                    Stations[i].Name = rand.Next();
                    Stations[i].ChargeSlots = rand.Next(0, 100);
                    Stations[i].Latitude = rand.Next() / rand.Next();
                    Stations[i].Longitude = rand.Next() / rand.Next();
                }
                
                //initializing variables into 5 drones
                for (int i = 5; i > 0; i--)
                {
                    Drones[i].ID = rand.Next(100,999);
                    Drones[i].Battery = rand.NextDouble() * 100;
                    Drones[i].Status = (@enum.DroneStatus)rand.Next(0, 2);
                    Drones[i].Weight = (@enum.WeightCategories)rand.Next(0, 2);
                    Drones[i].Priority = (@enum.Priorities)rand.Next(0, 2);
                }
                Drones[0].Model = "Yuneec H520";
                Drones[1].Model = "DJI Mavic 2 Zoom";
                Drones[2].Model = "DJI Phantom 4";
                Drones[3].Model = "3D Robotics Solo";
                Drones[4].Model = "Flyability Elios Drone";

                //Updates the indicators of the first free element-Drone
                Config.FirstAvailableDrone = 5;
            }
        }

    }
}
