using System;
using IDAL.DO;
namespace IDAL
{
    
   
    namespace DO
    {
        /// <summary>
        /// The drone weights
        /// </summary>
        public enum WeightCategories { light, midium, heavy };
        /// <summary>
        /// In what stage the drone is in
        /// </summary>
        public enum DroneStatus { available, maintenance, delivery };
        /// <summary>
        /// Delivery priorities
        /// </summary>
        public enum Priorities { normal, fast, urgent };
        
        /// <summary>
        /// Staitions details
        /// </summary>
        public struct Station
        {
           public int ID { get; set; }
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
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public DroneStatus Status { get; set; }
            public double Battery { get; set; }
            public Priorities Priority { get; set; }
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
            public int ID { get; set; }
            public int Sender { get; set; }
            public int Targetid { get; set; }
            public WeightCategories Weight { get; set; }
            public bool DroneActionMode { get; set; }
            
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
            static readonly Random rand = new (DateTime.Now.Millisecond);

            static internal Drone[] Drones = new Drone[10];
            static internal Station[] stations = new Station[5];
            static internal Customer[] customers = new Customer[100];
            static internal Parcel[] parcels = new Parcel[1000];
            /// <summary>
            ///Defines an index variable for each array that indicates a free space.
            /// </summary>
            internal class Config
            {
                static internal int FirstAvailableDrone = 0;
                static internal int firstAvailable_Station = 0;
                static internal int firstAvailable_Customer = 0;
                static internal int firstAvailable_Parcel = 0;
                static internal int identify;
            }
            
            public static void Initialize()
            { 
                Config.FirstAvailableDrone = 0;
                for (int i = rand.Next(2, 5); i>0;i--)
                {
                    stations[i].ID = rand.Next();
                    stations[i].Name = rand.Next();
                    stations[i].ChargeSlots = rand.Next(0,100);
                    stations[i].Latitude = rand.Next() / rand.Next();
                    stations[i].Longitude = rand.Next() / rand.Next();
                }

                for (int i = rand.Next(5, 10); i > 0; i--)
                {
                    Drones[i].ID = rand.Next();
                    Drones[i].Status = (DroneStatus)rand.Next(0, 2);
                    Drones[i].Weight =  (WeightCategories)rand.Next(0, 2);
                    Drones[i].Battery = rand.Next() / rand.Next();
                    Drones[i].Model = "A";
                }

            }
                
            

        }

        public struct DalObject 
        {

        }

    }
}
