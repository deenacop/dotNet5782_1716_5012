using System;
using IDAL.DO;
using System.Collections.Generic;

namespace DalObject
{
    /// <summary>
    /// Creates new lists for each type, and initializes them with variables.
    /// </summary>
    public class DataSource
    {
        static internal readonly Random rand = new(DateTime.Now.Millisecond);
        //creates lists for all the entities
        #region Creats lists
        static internal List<Drone> Drones = new(10);
        static internal List<Parcel> Parcels = new(1000);
        static internal List<Station> Stations = new(5);
        static internal List<Customer> Customers = new(100);
        static internal List<DroneCharge> DroneCharges = new(10);
        #endregion
        /// <summary>
        ///Defines a variable for a parcel
        /// </summary>
        internal class Config
        {
            static internal int RunnerIDNumParcels = 100000;
            static internal double vacant = 5;//per km
            static internal double CarriesLightWeight = 8;//per km
            static internal double CarriesMediumWeight = 10;//per km
            static internal double CarriesHeavyWeight = 13;//per km
            static internal double DroneLoadingRate = 0.05;//per min
        }
        #region Initialize
        /// <summary>
        /// Initializes all the lists we have built in the config function at default values
        /// </summary>
        public static void Initialize()
        {
            string[] addresArr = { "Balfour street, Jerusalem", "4 David Remez Street, Jerusalem" };
            // Initializing variables into 2 stations. 
            for (int i = 0; i < 2; i++)
            {
                Stations.Insert(i, new()
                {
                    ID = rand.Next(1000, 10000),
                    NumOfAvailableChargeSlots = rand.Next(0, 100),
                    Name = addresArr[i],
                    Latitude = rand.NextDouble() + 31,
                    Longitude = rand.NextDouble() + 35
                });
                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each station.
                {
                    if (Stations[j].ID == Stations[i].ID)
                    {
                        i--;
                        break;
                    }
                }
            }

            string[] modelArr = { "Yuneec H520", "DJI Mavic 2 Zoom", "DJI Phantom 4", "3D Robotics Solo", "Flyability Elios Drone" };
            //Initializing variables into 5 drones.
            for (int i = 0; i < 5; i++)
            {
                Drones.Insert(i, new()
                {
                    ID = rand.Next(100, 1000),
                    Model = modelArr[i],
                    Weight = (@enum.WeightCategories)rand.Next(0, 2)

                });
                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each drone.
                {
                    if (Drones[j].ID == Drones[i].ID)
                    {
                        i--;
                        break;
                    }
                }
            }

            string[] nameArr = { "Shira Segal" , "Deena Copperman" , "Benjamin Netanyahu" , "Yishai Ribu" ,
                                     "Yossi Cohen","Moshe Leon","Mordechai Glazer","Yehuda Shor","Yigal Eyal","Lior Ackerman"};

            string[] phoneArr = { "0548482282" , "0504188440", "0548324567" , "0547687689", "0525678997",
                                      "0537897889","0527689646","0526789997","0547890087","0505678876"};
            for (int i = 0; i < 10; i++)
            {
                //Initializing variables into 10 customers.
                Customers.Insert(i, new()
                {
                    ID = rand.Next(100000000, 1000000000),//9 digits
                    Latitude = rand.NextDouble() + 31,
                    Longitude = rand.NextDouble() + 35,
                    Name = nameArr[i],
                    PhoneNumber = phoneArr[i]

                });
                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each customer.
                {
                    if (Customers[j].ID == Customers[i].ID)
                    {
                        i--;
                        break;
                    }
                }
            }

            //Initializing variables into 10 parcels.
            for (int i = 0; i < 10; i++)
            {
                DateTime ? DateAndTime = null;
                Parcels.Insert(i, new()
                {
                    ID = Config.RunnerIDNumParcels++,
                    Sender = rand.Next(100000000, 999999999),
                    Targetid = rand.Next(100000000, 999999999),
                    MyDroneID = 0,
                    //In the initialization, the entire ID of the drone is 0 because we did not want to reach contradictions in the introduction of the identity of the drone
                    //and also that no deliveries were made yet.
                    Weight = (@enum.WeightCategories)rand.Next(0, 2),
                    Priority = (@enum.Priorities)rand.Next(0, 2),
                    Requested = DateTime.Now,
                    Scheduled = DateAndTime,
                    PickUp = DateAndTime,
                    Delivered = DateAndTime
                });
            }
        }
        #endregion
    }

}
