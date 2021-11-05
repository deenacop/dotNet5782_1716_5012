using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;
using IDAL;
//We wanted to shorten the main so we added this function
namespace ConsoleUI
{
    /// <summary>
    /// Auxiliary function to add an item to one of the lists
    /// </summary>
    public class MainFuncAdd
    {
        static String input;
        static int tmpInt;
        static double tmpDouble;
        /// <summary>
        /// Asks station details from the user and adds the new station to the current list.
        /// </summary>
        public static void AddStation()
        {
            IDAL.DO.Station NewStation = new IDAL.DO.Station();
            Console.WriteLine("Enter the new station details: *station ID (4 digits)");
            input = Console.ReadLine();
            if (int.TryParse(input, out tmpInt))
                NewStation.ID = tmpInt;
            Console.WriteLine("*station name");
            NewStation.Name = Console.ReadLine();
            Console.WriteLine("*station number of available charge slots");
            input = Console.ReadLine();
            int.TryParse(input, out tmpInt);
            NewStation.NumOfAvailableChargeSlots = tmpInt;
            Console.WriteLine("*station longitude");
            input = Console.ReadLine();
            if (double.TryParse(input, out tmpDouble))
                NewStation.Longitude = tmpDouble;
            Console.WriteLine("*station latitude");
            input = Console.ReadLine();
            if (double.TryParse(input, out tmpDouble))
                NewStation.Latitude = tmpDouble;
            Program.DalObj.Add(NewStation);
        }

        /// <summary>
        /// Asks parcel details from the user and adds the new parcel to the current list.
        /// </summary>
        public static void AddParcel()
        {
            DateTime DateAndTime = new DateTime(1, 1, 1);
            IDAL.DO.Parcel NewParcel = new IDAL.DO.Parcel();
            Console.WriteLine("*parcel sender ID (6 digits)");
            input = Console.ReadLine();
            if (int.TryParse(input, out tmpInt))
                NewParcel.Sender = tmpInt;
            Console.WriteLine("*parcel targetid");
            input = Console.ReadLine();
            if (int.TryParse(input, out tmpInt))
                NewParcel.Targetid = tmpInt;
            Console.WriteLine("*weight- for light type 0, for midium type 1, for heavy type 2");
            input = Console.ReadLine();
            int.TryParse(input, out tmpInt);
            NewParcel.Weight = (@enum.WeightCategories)tmpInt;
            Console.WriteLine("*parcel Priority- for normal type 0, for fast type 1, for urgent type 2");
            input = Console.ReadLine();
            int.TryParse(input, out tmpInt);
            NewParcel.Priority = (@enum.Priorities)tmpInt;
            NewParcel.MyDroneID = 0;
            NewParcel.Requested = DateTime.Now;
            NewParcel.Scheduled = DateAndTime;
            NewParcel.PickUp = DateAndTime;
            NewParcel.Delivered = DateAndTime;
            Program.DalObj.Add(NewParcel);
        }

        /// <summary>
        /// Asks customer details from the user and adds the new customer to the current list
        /// </summary>
        public static void AddCustomer()
        {
            IDAL.DO.Customer NewCustomer = new IDAL.DO.Customer();
            Console.WriteLine("Enter the new customer details: *customer ID (9 digits)");
            input = Console.ReadLine();
            if (int.TryParse(input, out tmpInt))
                NewCustomer.ID = tmpInt;
            Console.WriteLine("*customer name");
            NewCustomer.Name = Console.ReadLine();
            Console.WriteLine("*customer phone number");
            NewCustomer.PhoneNumber = Console.ReadLine();
            Console.WriteLine("*customer longitude");
            input = Console.ReadLine();
            double.TryParse(input, out tmpDouble);
            NewCustomer.Longitude = tmpDouble;
            Console.WriteLine("*customer latitude");
            input = Console.ReadLine();
            double.TryParse(input, out tmpDouble);
            NewCustomer.Latitude = tmpDouble;
            Program.DalObj.Add(NewCustomer);
        }

        /// <summary>
        /// Asks drone details from the user and adds the new drone to the current list
        /// </summary>
        public static void AddDrone()
        {
            IDAL.DO.Drone NewDrone = new IDAL.DO.Drone();
            Console.WriteLine("Enter the new drone details: *drone ID (3 digits)");
            input = Console.ReadLine();
            if (int.TryParse(input, out tmpInt))
                NewDrone.ID = tmpInt;
            Console.WriteLine("*drone model");
            NewDrone.Model = Console.ReadLine();
            Console.WriteLine("*drones weight (for light type 0, midium type 1, heavy type 2)");
            input = Console.ReadLine();
            int.TryParse(input, out tmpInt);
            NewDrone.Weight = (@enum.WeightCategories)tmpInt;
            Program.DalObj.Add(NewDrone);
        }
    }
}
