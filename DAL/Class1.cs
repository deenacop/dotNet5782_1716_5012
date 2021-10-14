using System;
using IDAL.DO;
namespace IDAL
{
    
   
    namespace DO
    {
        /// <summary>
        /// The drone weights
        /// </summary>
        enum WeightCategories { light, midium, heavy };
        /// <summary>
        /// In what stage the drone is in
        /// </summary>
        enum DroneStatus { available, maintenance, delivery };
        /// <summary>
        /// Delivery priorities
        /// </summary>
        enum Priorities { normal, fast, urgent };
        
        /// <summary>
        /// Staitions details
        /// </summary>
        public struct Station
        {
           public int ID { get; set; };
           public int Name { get; set; }
           public int ChargeSlots { get; set; }
           public double Longitude { get; set; }
           public double Latitude { get; set; }
        }
        /// <summary>
        /// Drone details
        /// </summary>
        public struct Drone
        {
            int ID;
            string Model;
            WeightCategories Weight;
            DroneStatus Status;
            double Battery;
            Priorities Priority;

        }
        /// <summary>
        /// Customer details
        /// </summary>
        public struct Customer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
        /// <summary>
        /// Parcel details
        /// </summary>
        public struct Parcel
        {
            int ID;
            int Sender;
            int Targetid;
            WeightCategories Weight;
            bool DroneActionMode;
            public DateTime Requested { get; set; }
            public DateTime PickUp { get; set; }
            public DateTime Delivered { get; set; }
            public DateTime Scheduled { get; set; }
        }
        /// <summary>
        /// Drone charge details
        /// </summary>
        public struct DroneCharge
        {
            public int RecBaseStation { get; set; }
            public int RecDrone { get; set; }
        }
        /// <summary>
        /// Creates new arrays for each type, and initializes them with variables.
        /// </summary>
        public class DataSource
        {
            public static Random R = new Random(DateTime.Now.Millisecond);

            static internal Drone[] drones = new Drone[10];
            static internal Station[] stations = new Station[5];
            static internal Customer[] customers = new Customer[100];
            static internal Parcel[] parcels = new Parcel[1000];
            /// <summary>
            ///Defines an index variable for each array that indicates a free space.
            /// </summary>
            internal class Config
            {
                static internal int firstAvailable_Drone = 0;
                static internal int firstAvailable_Station = 0;
                static internal int firstAvailable_Customer = 0;
                static internal int firstAvailable_Parcel = 0;
                static internal int identify;
            }
            
            public static void Initialize()
            { 
                int i = R.Next(2, 5);
                firstAvailable_Station = i;
                for (;i>0;i--)
                {
                    stations[i].ID = R.Next();
                    stations[i].Name = R.Next();
                    stations[i].ChargeSlot = R.Next(0,100);
                    stations[i].Latitude = R.Next() / R.Next();
                    stations[i].Longitude = R.Next() / R.Next();
                }

                i = R.Next(5, 10);
                for (; i > 0; i--)
                {
                    drones[i].ID = R.Next();
                    drones[i].Status = R.Next(1,3);
                    drones[i].Weight = R.Next(1, 3);
                    drones[i].Battery = R.Next() / R.Next();
                    drones[i].Model = "A";
                }

            }
                
            

        }

        public struct DalObject 
        {

        }

    }
}
