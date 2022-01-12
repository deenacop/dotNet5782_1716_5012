using System;
using DO;
using DalApi;
using System.Collections.Generic;



namespace Dal
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
        static internal List<User> Users = new(100);
        #endregion

        /// <summary>
        ///Defines a variable for a parcel
        /// </summary>
        internal class Config
        {
            static internal int RunnerIDNumParcels = 100000;
            static internal double vacant = 2;//per km
            static internal double CarriesLightWeight = 5;//per km
            static internal double CarriesMediumWeight = 8;//per km
            static internal double CarriesHeavyWeight = 10;//per km
            static internal double DroneLoadingRate = 20;//per min
        }

        /// <summary>
        /// Initializes all the lists we have built with default values
        /// </summary>
        public static void Initialize()
        {
            //#region station
            //string[] addresArr = { "Balfour street, Jerusalem", "4 David Remez Street, Jerusalem" };

            //// Initializing variables into 2 stations.
            //for (int i = 0; i < 2; i++)
            //{
            //    Stations.Insert(i, new()
            //    {
            //        Id = rand.Next(1000, 10000),
            //        NumOfAvailableChargingSlots = rand.Next(0, 30),
            //        Name = addresArr[i],
            //        IsRemoved = false,
            //        Latitude = rand.NextDouble() + 31,
            //        Longitude = rand.NextDouble() + 35
            //    });
            //    for (int j = 0; j < i; j++)//Checks that the ID number is unique to each station.
            //    {
            //        if (Stations[j].Id == Stations[i].Id)
            //        {
            //            i--;
            //            break;
            //        }
            //    }
            //}
            //#endregion

            //#region drone
            //string[] modelArr = { "Yuneec H520", "DJI Mavic 2 Zoom", "DJI Phantom 4", "3D Robotics Solo", "Flyability Elios Drone" };

            ////Initializing variables into 10 drones.
            //for (int i = 0; i < 10; i++)
            //{
            //    int flag = rand.Next(0, 50);
            //    Drone drone = new();
            //    drone.Id = rand.Next(100, 1000);
            //    drone.Model = modelArr[rand.Next(5)];
            //    drone.Weight = WeightCategories.Heavy;
            //    drone.IsRemoved = false;
            //    if (flag >= 20)
            //    {
            //        drone.Weight = WeightCategories.Medium;
            //        if (flag >= 30)
            //            drone.Weight = WeightCategories.Light;
            //    }

            //    for (int j = 0; j < i; j++)//Checks that the ID number is unique to each drone.
            //    {
            //        if (Drones[j].Id == drone.Id)
            //        {
            //            i--;
            //            break;
            //        }
            //    }
            //    Drones.Add(drone);
            //}
            //#endregion

            //#region customer
            //string[] nameArr = { "Shira Segal" , "Deena Copperman" , "Naama Segal" , "Yishai Ribu" ,
            //                         "Yossi Cohen","Moshe Leon","Mordechai Glecer","huda Seor","Yigal Eyal","Lior Ackerman"};

            //string[] phoneArr = { "0548482282" , "0504188440", "0548324567" , "0547687689", "0525678997",
            //                          "0537897889","0527689646","0526789997","0547890087","0505678876"};
            ////Initializing variables into 10 customers.
            //for (int i = 0; i < 10; i++)
            //{
            //    Customers.Insert(i, new()
            //    {
            //        Id = rand.Next(100000000, 1000000000),//9 digits
            //        Latitude = rand.NextDouble() + 31,
            //        Longitude = rand.NextDouble() + 35,
            //        Name = nameArr[i],
            //        PhoneNumber = phoneArr[i],
            //        IsRemoved = false

            //    });
            //    for (int j = 0; j < i; j++)//Checks that the ID number is unique to each customer.
            //    {
            //        if (Customers[j].Id == Customers[i].Id)
            //        {
            //            i--;
            //            break;
            //        }
            //    }
            //}
            //#endregion

            //#region parcel
            //for (int index = 0; index < 10; index++)//Updating 10 parcels
            //{
            //    Parcel newParcel = new();
            //    newParcel.Id = Config.RunnerIDNumParcels++;//Updating the ID number of the package
            //    newParcel.Sender = Customers[rand.Next(0, 10)].Id;//Updating the ID number of the sender
            //    newParcel.MyDroneID = 0;//Updating the ID number of the drone
            //    newParcel.IsRemoved = false;
            //    do//checks that the sender and the receiver are noT the same person
            //    {
            //        newParcel.Targetid = Customers[rand.Next(0, 10)].Id;
            //    }
            //    while (newParcel.Sender == newParcel.Targetid);

            //    newParcel.Weight = (WeightCategories)rand.Next(0, (int)WeightCategories.Max + 1);//Updating the weight
            //    newParcel.Priority = (Priorities)rand.Next(0, (int)Priorities.Max + 1);//Updating the urgency of the shipment
            //                                                                           //Putting a random date and time
            //    newParcel.Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29),
            //        rand.Next(24), rand.Next(60), rand.Next(60));
            //    int status = rand.Next(0, 60);
            //    int flag = -1;
            //    if (status >= 10)
            //    {
            //        //Scheduling a time to deliver parcel
            //        newParcel.Scheduled = newParcel.Requested +
            //            new TimeSpan(rand.Next(5), rand.Next(60), rand.Next(60));

            //        if (status >= 30)
            //        {
            //            //Time drone came to deliver parcel
            //            newParcel.PickUp = newParcel.Scheduled +
            //                new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
            //            if (status >= 40)
            //            {
            //                //Time customer recieved parcel
            //                newParcel.Delivered = newParcel.PickUp +
            //                    new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
            //                do
            //                {
            //                    flag = rand.Next(0, 10);
            //                    newParcel.MyDroneID = Drones[flag].Id;
            //                }
            //                while (Drones[flag].Weight < newParcel.Weight);//checks if the drone could carry the drone according to the wheight
            //            }
            //        }
            //        if (flag == -1)
            //        {
            //            do
            //            {
            //                flag = rand.Next(0, 10);
            //                newParcel.MyDroneID = Drones[flag].Id;
            //            }
            //            while (Drones[flag].Weight > newParcel.Weight);
            //        }
            //    }
            //    Parcels.Add(newParcel);
            //}
            //#endregion

            //#region User
            ////Our program is based on the fact that every user is also a customer.
            ////We separated them into 2 personalities for convenience.
            ////We will initialize the list of users based on the list of customers we have already initialized
            //string[] emailArr = { "103shira@gmail.com", "deenacop@gmail.com", "1654naama@gmail.com", "1try@gmail.com", "2try@gmail.com", "3try@gmail.com", "4try@gmail.com", "5try@gmail.com", "6try@gmail.com", "7try@gmail.com" };
            //string[] passwordArr = { "shira1234", "deena1234", "naama1234", "try1", "try2", "try3", "try4", "try5", "try6", "try7" };
            ////2 manager
            //for (int i = 0; i < 3; i++)
            //{
            //    Users.Insert(i, new()
            //    {
            //        Id = Customers[i].Id,
            //        Name = Customers[i].Name,
            //        PhoneNumber = Customers[i].PhoneNumber,
            //        Password = passwordArr[i],
            //        EmailAddress = emailArr[i],
            //        IsManager = true,
            //        Latitude = Customers[i].Latitude,
            //        Longitude = Customers[i].Longitude
            //    });
            //}
            ////7 users
            //for (int i = 3; i < 10; i++)
            //{
            //    Users.Insert(i, new()
            //    {
            //        Id = Customers[i].Id,
            //        Name = Customers[i].Name,
            //        PhoneNumber = Customers[i].PhoneNumber,
            //        Password = passwordArr[i],
            //        EmailAddress = emailArr[i],
            //        IsManager = false,
            //        Latitude = Customers[i].Latitude,
            //        Longitude = Customers[i].Longitude
            //    });
            //}
            ////////neeeddd ttoo beee deleteddd!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //Users.Insert(2, new()
            //{
            //    Id = 567891234,
            //    Name = "try",
            //    PhoneNumber = "0549096309",
            //    Password = "try",
            //    EmailAddress = "try",
            //    IsManager = true,
            //    Longitude = 35.8,
            //    Latitude = 31.4
            //});
            //Customers.Insert(10 ,new()
            //{
            //    Id = 567891234,
            //    Name = "try",
            //    PhoneNumber = "0549096309",
            //    Longitude = 35.8,
            //    Latitude = 31.4
            //});
            //#endregion

            //XMLTools.SaveListToXMLSerializer(Drones, DroneXml);
            //XMLTools.SaveListToXMLSerializer(Parcels, ParcelXml);
            //XMLTools.SaveListToXMLSerializer(Stations, StationXml);
            //XMLTools.SaveListToXMLSerializer(DroneCharges, DroneChargeXml);
            //XMLTools.SaveListToXMLSerializer(Customers, CustomerXml);

        }
        //private static string DroneXml = @"DroneXml.xml";
        //private static string ParcelXml = @"ParcelXml.xml";
        //private static string StationXml = @"StationXml.xml";
        //private static string DroneChargeXml = @"DroneChargeXml.xml";
        //private static string CustomerXml = @"CustomerXml.xml";
    }
}



