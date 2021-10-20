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
                String[] AddresArr = { "Balfour street, Jerusalem", "4 David Remez Street, Jerusalem" };
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
                String[] ModelArr = { "Yuneec H520", "DJI Mavic 2 Zoom", "DJI Phantom 4", "3D Robotics Solo", "Flyability Elios Drone" };
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

                //Initializing variables into 10 customers.
                for (int i = 0; i < 10; i++)
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
