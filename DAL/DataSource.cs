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
                // Initializing variables into 2 stations. 
                for (int i = 0; i <2; i++)
                {
                    Stations[i].ID = rand.Next(1000,9999);
                    Stations[i].NumOfChargeSlots = rand.Next(0, 100);
                    //In Jerusalem only
                    Stations[i].Latitude = rand.NextDouble() + 31;
                    Stations[i].Longitude = rand.NextDouble() + 35;
                }
                // Initializing variables into 2 stations names. 
                Stations[0].Name = "Balfour street, Jerusalem";
                Stations[1].Name = "4 David Remez Street, Jerusalem";

                //Updates the indicator of the first free elements-Station
                Config.firstAvailableStation = 2;

                //Initializing variables into 5 drones.
                for (int i = 0; i <5; i++)
                {
                    Drones[i].ID = rand.Next(100,999);
                    Drones[i].Battery = rand.Next(0,100);
                    Drones[i].Status = (@enum.DroneStatus)rand.Next(0, 2);
                    Drones[i].Weight = (@enum.WeightCategories)rand.Next(0, 2);
                    Drones[i].Priority = (@enum.Priorities)rand.Next(0, 2);
                }
                Drones[0].Model = "Yuneec H520";
                Drones[1].Model = "DJI Mavic 2 Zoom";
                Drones[2].Model = "DJI Phantom 4";
                Drones[3].Model = "3D Robotics Solo";
                Drones[4].Model = "Flyability Elios Drone";
                //Updates the indicator of the first free elements-Drone
                Config.FirstAvailableDrone = 5;

                //Initializing variables into 10 customers.
                for (int i=0;i<10;i++)
                {
                    Customers[i].ID = rand.Next(100000000, 999999999);//9 digits
                    Customers[i].Latitude = rand.NextDouble() + 31;
                    Customers[i].Longitude = rand.NextDouble() + 35;
                }
                //Initializing variables into customers names.
                Customers[0].Name = "Shira Segal";
                Customers[1].Name = "Deena Copperman";
                Customers[2].Name = "Benjamin Netanyahu";
                Customers[3].Name = "Yishai Ribu";
                Customers[4].Name = "Yossi Cohen";
                Customers[5].Name = "Moshe Leon";
                Customers[6].Name = "Mordechai Glazer";
                Customers[7].Name = "Yehuda Shor";
                Customers[8].Name = "Yigal Eyal";
                Customers[9].Name = "Lior Ackerman";
                //Initializing variables into customers phone numbers.
                Customers[0].PhoneNumber = "0548482282";
                Customers[1].PhoneNumber = "0504188440";
                Customers[2].PhoneNumber = "0548324567";
                Customers[3].PhoneNumber = "0547687689";
                Customers[4].PhoneNumber = "0525678997";
                Customers[5].PhoneNumber = "0537897889";
                Customers[6].PhoneNumber = "0527689646";
                Customers[7].PhoneNumber = "0526789997";
                Customers[8].PhoneNumber = "0547890087";
                Customers[9].PhoneNumber = "0505678876";
                //Updates the indicator of the first free elements-Customers
                Config.firstAvailableCustomer = 10;

                for(int i=0;i<10;i++)
                {
                    DateTime DateAndTime = new DateTime(2021, rand.Next(1, 12), rand.Next(1, 29), rand.Next(1, 24), rand.Next(0, 60), rand.Next(0, 60));
                    Parcels[i].ID = rand.Next(100000, 999999);
                    Parcels[i].Sender = rand.Next(100000000, 999999999);
                    Parcels[i].Targetid = rand.Next(100000000, 999999999);
                    Parcels[i].Requested = rand.Next;
                    Parcels[i].PickUp = rand.Next;
                    Parcels[i].Delivered = rand.Next;
                    Parcels[i].Scheduled = DateAndTime;

                }
            }
        }

    }
}
