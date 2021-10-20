﻿using System;
using IDAL.DO;
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Creates new arrays for each type, and initializes them with variables.
        /// </summary>
        internal class DataSource
        {
            static internal readonly Random rand = new(DateTime.Now.Millisecond);
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
                static internal int FirstAvailableStation = 0;
                static internal int FirstAvailableCustomer = 0;
                static internal int FirstAvailableParcel = 0;
                static internal int RunnerIDNumParcels = 0;
            }
            public static void Initialize()
            {
                string[] AddresArr = { "Balfour street, Jerusalem", "4 David Remez Street, Jerusalem" };
                // Initializing variables into 2 stations. 
                for (int i = 0; i < 2; i++)
                {
                    Stations[i].ID = rand.Next(1000, 9999);
                    for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each station.
                    {
                        if (Stations[j].ID == Stations[i].ID)
                        {
                            while (Stations[j].ID == Stations[i].ID)
                                Stations[i].ID = rand.Next(1000, 9999);
                            j = 0;
                        }
                    }
                    Stations[i].NumOfAvailableChargeSlots = rand.Next(0, 100);
                    Stations[i].Name = AddresArr[i];
                    //In Jerusalem only
                    Stations[i].Latitude = rand.NextDouble() + 31;
                    Stations[i].Longitude = rand.NextDouble() + 35;
                }

                //Updates the indicator of the first free element-Station
                Config.FirstAvailableStation = 2;
                string[] ModelArr = { "Yuneec H520", "DJI Mavic 2 Zoom", "DJI Phantom 4", "3D Robotics Solo", "Flyability Elios Drone" };
                //Initializing variables into 5 drones.
                for (int i = 0; i < 5; i++)
                {
                    Drones[i].ID = rand.Next(100, 999);
                    for (int j = 0; j < i; j++) //Checks that indeed the ID number is unique to each drone.
                    {
                        if (Drones[j].ID == Drones[i].ID)
                        {
                            while (Drones[j].ID == Drones[i].ID)
                                Drones[i].ID = rand.Next(1000, 9999);
                            j = 0;
                        }
                    }
                    Drones[i].Model = ModelArr[i];
                    Drones[i].Battery = rand.Next(0, 100);
                    Drones[i].Status = 0;//No delivery have yet been made so all drones are available.
                    Drones[i].Weight = (@enum.WeightCategories)rand.Next(0, 2);
                }
                //Updates the indicator of the first free element-Drone
                Config.FirstAvailableDrone = 5;

                
                string[] NameArr = { "Shira Segal" , "Deena Copperman" , "Benjamin Netanyahu" , "Yishai Ribu" ,
                                     "Yossi Cohen","Moshe Leon","Mordechai Glazer","Yehuda Shor","Yigal Eyal","Lior Ackerman"};
                
                string[] PhoneArr = { "0548482282" , "0504188440", "0548324567" , "0547687689", "0525678997",
                                      "0537897889","0527689646","0526789997","0547890087","0505678876"};
                //Initializing variables into 10 customers.
                for (int i = 0; i < 10; i++)
                {
                    Customers[i].ID = rand.Next(100000000, 999999999);//9 digits
                    Customers[i].Latitude = rand.NextDouble() + 31;
                    Customers[i].Longitude = rand.NextDouble() + 35;
                    Customers[i].Name = NameArr[i];
                    Customers[i].PhoneNumber = PhoneArr[i];
                }

                //Updates the indicator of the first free element-Customers
                Config.FirstAvailableCustomer = 10;

                //Initializing variables into 10 parcels.
                for (int i = 0; i < 10; i++)
                {
                    //Date and time randomly 
                    DateTime DateAndTime = new DateTime(2021, rand.Next(1, 12), rand.Next(1, 29), rand.Next(1, 24), rand.Next(0, 60), rand.Next(0, 60));
                    Parcels[i].ID = rand.Next(100000, 999999);
                    for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each parcel.
                    {
                        if (Parcels[j].ID == Parcels[i].ID)
                        {
                            while (Parcels[j].ID == Parcels[i].ID)
                                Parcels[i].ID = rand.Next(1000, 9999);
                            j = 0;
                        }
                    }
                    Parcels[i].Sender = rand.Next(100000000, 999999999);
                    Parcels[i].Targetid = rand.Next(100000000, 999999999);
                    Parcels[i].MyDroneID = 0;//In the initialization, the entire ID of the drone is 0 because we did not want to reach contradictions in the introduction of the identity of the drone and also that no deliveries were made yet.
                    Parcels[i].Weight = (@enum.WeightCategories)rand.Next(0, 2);
                    Parcels[i].Priority = (@enum.Priorities)rand.Next(0, 2);
                    Parcels[i].Requested = DateAndTime;
                    Parcels[i].Scheduled = Parcels[i].Requested.AddMinutes(rand.Next(10, 1000));//adds minutes between requested and scheduled 
                    Parcels[i].Delivered = Parcels[i].Scheduled.AddHours(rand.Next(10, 1000));//adds hours between scheduled and delivered 
                    Parcels[i].PickUp = Parcels[i].Delivered.AddHours(rand.Next(10, 1000));//adds hours between delivered and pick up
                }
                //Updates the indicator of the first free element-Parcel
                Config.FirstAvailableParcel = 10;
                //Updates the value to a greater num than all the packages that were added 
                Config.RunnerIDNumParcels = 11;
            }
        }

    }
}
