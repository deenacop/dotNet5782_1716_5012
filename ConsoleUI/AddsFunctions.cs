using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

//We wanted to shorten the main so we added this function
namespace ConsoleUI
{
    //namespace DO
    //{
        /// <summary>
        /// Auxiliary function to add an item to one of the lists
        /// </summary>
        public class MainFuncAdd
        {
            public static String Input;
            public static int TmpInt;
            public static double TmpDouble;
            /// <summary>
            /// Asks station details from the user and adds the new station to the current list.
            /// </summary>
            public static void AddStation()
            {
                IDAL.DO.Station NewStation = new IDAL.DO.Station();
                Console.WriteLine("Enter the new station details: *station ID (4 digits)");
                Input = Console.ReadLine();
                if (int.TryParse(Input, out TmpInt))
                    NewStation.ID = TmpInt;
                Console.WriteLine("*station name");
                NewStation.Name = Console.ReadLine();
                Console.WriteLine("*station number of available charge slots");
                Input = Console.ReadLine();
                int.TryParse(Input, out TmpInt);
                NewStation.NumOfAvailableChargeSlots = TmpInt;
                Console.WriteLine("*station longitude");
                Input = Console.ReadLine();
                if (double.TryParse(Input, out TmpDouble))
                    NewStation.Longitude = TmpDouble;
                Console.WriteLine("*station latitude");
                Input = Console.ReadLine();
                if (double.TryParse(Input, out TmpDouble))
                    NewStation.Latitude = TmpDouble;
                IDAL.DO.DalObject.Add(NewStation);
            }

            /// <summary>
            /// Asks parcel details from the user and adds the new parcel to the current list.
            /// </summary>
            public static void AddParcel()
            {
                IDAL.DO.Parcel NewParcel = new IDAL.DO.Parcel();
                //Console.WriteLine("Enter the new parcel details: *pracel ID (6 digits)");
                //Input = Console.ReadLine();
                //if (int.TryParse(Input, out TmpInt))
            //    NewParcel.ID = ++IDAL.DO.DataSource.Config.RunnerIDNumParcels;
                Console.WriteLine("*parcel sender ID (6 digits)");
                Input = Console.ReadLine();
                if (int.TryParse(Input, out TmpInt))
                    NewParcel.Sender = TmpInt;
                Console.WriteLine("*parcel targetid");
                Input = Console.ReadLine();
                if (int.TryParse(Input, out TmpInt))
                    NewParcel.Targetid = TmpInt;
                Console.WriteLine("*weight- for light type 0, for midium type 1, for heavy type 2");
                Input = Console.ReadLine();
                int.TryParse(Input, out TmpInt);
                NewParcel.Weight = (@enum.WeightCategories)TmpInt;
                Console.WriteLine("*parcel Priority- for normal type 0, for fast type 1, for urgent type 2");
                Input = Console.ReadLine();
                int.TryParse(Input, out TmpInt);
                NewParcel.Priority = (@enum.Priorities)TmpInt;
                NewParcel.MyDroneID = 0;
                NewParcel.Requested = DateTime.Now;
                NewParcel.Scheduled = DateTime.Now;
                NewParcel.PickUp = DateTime.Now;
                NewParcel.Delivered = DateTime.Now;
                IDAL.DO.DalObject.Add(NewParcel);
            }

            /// <summary>
            /// Asks customer details from the user and adds the new customer to the current list
            /// </summary>
            public static void AddCustomer()
            {
                IDAL.DO.Customer NewCustomer = new IDAL.DO.Customer();
                Console.WriteLine("Enter the new customer details: *customer ID (9 digits)");
                Input = Console.ReadLine();
                if (int.TryParse(Input, out TmpInt))
                    NewCustomer.ID = TmpInt;
                Console.WriteLine("*customer name");
                NewCustomer.Name = Console.ReadLine();
                Console.WriteLine("*customer longitude");
                Input = Console.ReadLine();
                double.TryParse(Input, out TmpDouble);
                NewCustomer.Longitude = TmpDouble;
                Console.WriteLine("*customer latitude");
                Input = Console.ReadLine();
                double.TryParse(Input, out TmpDouble);
                NewCustomer.Latitude = TmpDouble;
                IDAL.DO.DalObject.Add(NewCustomer);
            }

            /// <summary>
            /// Asks drone details from the user and adds the new drone to the current list
            /// </summary>
            public static void AddDrone()
            {
                IDAL.DO.Drone NewDrone = new IDAL.DO.Drone();
                Console.WriteLine("Enter the new drone details: *drone ID (3 digits)");
                Input = Console.ReadLine();
                if (int.TryParse(Input, out TmpInt))
                    NewDrone.ID = TmpInt;
                Console.WriteLine("*drone model");
                NewDrone.Model = Console.ReadLine();
                Console.WriteLine("*drones weight (for light type 0, midium type 1, heavy type 2)");
                Input = Console.ReadLine();
                int.TryParse(Input, out TmpInt);
                NewDrone.Weight = (@enum.WeightCategories)TmpInt;
                NewDrone.Battery = 100;
                NewDrone.Status = 0;
                IDAL.DO.DalObject.Add(NewDrone);
            }
        }
    //}
}
