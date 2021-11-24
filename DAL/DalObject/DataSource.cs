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

        /// <summary>
        /// Initializes all the lists we have built in the config function at default values
        /// </summary>
        public static void Initialize()
        {
            #region station
            string[] addresArr = { "Balfour street, Jerusalem", "4 David Remez Street, Jerusalem" };
            // Initializing variables into 2 stations.
            for (int i = 0; i < 2; i++)
            {
                Stations.Insert(i, new()
                {
                    StationID = rand.Next(1000, 10000),
                    NumOfAvailableChargingSlots = rand.Next(0, 100),
                    Name = addresArr[i],
                    Latitude = rand.NextDouble() + 31,
                    Longitude = rand.NextDouble() + 35
                });
                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each station.
                {
                    if (Stations[j].StationID == Stations[i].StationID)
                    {
                        i--;
                        break;
                    }
                }
            }
            #endregion

            #region drone

            string[] modelArr = { "Yuneec H520", "DJI Mavic 2 Zoom", "DJI Phantom 4", "3D Robotics Solo", "Flyability Elios Drone" };
            //Initializing variables into 5 drones.
            for (int i = 0; i < 10; i++)
            {
                int flag = rand.Next(0, 50);
                Drone drone = new();
                drone.DroneID = rand.Next(100, 1000);
                drone.Model = modelArr[rand.Next(5)];
                drone.Weight = WeightCategories.Heavy;
                if (flag >= 20)
                {
                    drone.Weight = WeightCategories.Medium;
                    if (flag >= 30)
                        drone.Weight = WeightCategories.Light;
                }

                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each drone.
                {
                    if (Drones[j].DroneID == drone.DroneID)
                    {
                        i--;
                        break;
                    }
                }
                Drones.Add(drone);
            }
            #endregion

            #region customer
            string[] nameArr = { "Shira Segal" , "Deena Copperman" , "Benjamin Netanyahu" , "Yishai Ribu" ,
                                     "Yossi Cohen","Moshe Leon","Mordechai Glazer","huda Seor","Yigal Eyal","Lior Ackerman"};

            string[] phoneArr = { "0548482282" , "0504188440", "0548324567" , "0547687689", "0525678997",
                                      "0537897889","0527689646","0526789997","0547890087","0505678876"};
            for (int i = 0; i < 10; i++)
            {
                //Initializing variables into 10 customers.
                Customers.Insert(i, new()
                {
                    CustomerID = rand.Next(100000000, 1000000000),//9 digits
                    Latitude = rand.NextDouble() + 31,
                    Longitude = rand.NextDouble() + 35,
                    Name = nameArr[i],
                    PhoneNumber = phoneArr[i]

                });
                for (int j = 0; j < i; j++)//Checks that indeed the ID number is unique to each customer.
                {
                    if (Customers[j].CustomerID == Customers[i].CustomerID)
                    {
                        i--;
                        break;
                    }
                }
            }
            #endregion

            #region parcel
            //Initializing variables into 10 parcels.
            //for (int i = 0; i < 10; i++)
            //{

            //Parcels.Insert(i, 
            //{
            //ParcelID = Config.RunnerIDNumParcels++,
            //Sender = Customers[rand.Next(10)].CustomerID,
            //Targetid = Customers[rand.Next(10)].CustomerID,
            //MyDroneID = Drones[i].DroneID,
            ////In the initialization, the entire ID of the drone is 0
            ////because we did not want to reach contradictions in the introduction of the identity of the drone
            ////and also that no deliveries were made yet.
            //Weight = (WeightCategories)rand.Next(0, 2),
            //Priority = (Priorities)rand.Next(0, 2),
            for (int index = 0; index < 10; index++)//Updating 10 parcels
            {
                Parcel newParcel = new();
                newParcel.ParcelID = Config.RunnerIDNumParcels++;//Updating the ID number of the package
                newParcel.Sender = Customers[rand.Next(0,10)].CustomerID;//Updating the ID number of the sender
                newParcel.MyDroneID = 0;//Updating the ID number of the drone
                do
                {
                    newParcel.Targetid = Customers[rand.Next(0, 10)].CustomerID;
                }
                while (newParcel.Sender == newParcel.Targetid);

                newParcel.Weight = (WeightCategories)rand.Next(0, (int)WeightCategories.Max + 1);//Updating the weight
                newParcel.Priority = (Priorities)rand.Next(0, (int)Priorities.Max + 1);//Updating the urgency of the shipment
                                                                       //Putting a random date and time
                newParcel.Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29),
                    rand.Next(24), rand.Next(60), rand.Next(60));
                int status = rand.Next(0,60);
                int flag = -1;
                if (status >= 10)
                {
                    //Scheduling a time to deliver parcel
                    newParcel.Scheduled = newParcel.Requested +
                        new TimeSpan(rand.Next(5), rand.Next(60), rand.Next(60));

                    if (status >= 15)
                    {
                        //Time drone came to deliver parcel
                        newParcel.PickUp = newParcel.Scheduled +
                            new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                        if (status >= 40)
                        {
                            //Time customer recieved parcel
                            newParcel.Delivered = newParcel.PickUp +
                                new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                            do
                            {
                                flag = rand.Next(0,10);
                                newParcel.MyDroneID = Drones[flag].DroneID;
                            }
                            while (Drones[flag].Weight < newParcel.Weight);
                        }
                    }
                    if (flag == -1)
                    {
                        do
                        {
                            flag = rand.Next(0,10);
                            newParcel.MyDroneID = Drones[flag].DroneID;
                        }
                        while (Drones[flag].Weight > newParcel.Weight);
                    }
                }
                Parcels.Add(newParcel);
            }
            //}
            //Requested = DateTime.Now,
            //Scheduled = DateTime.Now,
            //PickUp = DateTime.Now,
            //Delivered = DateTime.Now
            //};
        }
        #endregion
    }
}



