using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
//We wanted to shorten the main so we added this function
namespace ConsoleUI
{
    /// <summary>
    /// Auxiliary function to add an item to one of the lists
    /// </summary>
    public static class MainFuncAdd
    {
        static int tmpInt;
        static double tmpDouble;
        /// <summary>
        /// Asks station details from the user and adds the new station to the current list.
        /// </summary>
        #region Add
        public static void AddStation()
        {
            DO.Station NewStation = new();
            Console.WriteLine("Enter the new station details: *station ID (4 digits)");
            if (int.TryParse(Console.ReadLine(), out tmpInt))
                NewStation.Id = tmpInt;
            Console.WriteLine("*station name");
            NewStation.Name = Console.ReadLine();
            Console.WriteLine("*station number of available charge slots");
            int.TryParse(Console.ReadLine(), out tmpInt);
            NewStation.NumOfAvailableChargingSlots = tmpInt;
            Console.WriteLine("*station longitude");
            if (double.TryParse(Console.ReadLine(), out tmpDouble))
                NewStation.Longitude = tmpDouble;
            Console.WriteLine("*station latitude");
            if (double.TryParse(Console.ReadLine(), out tmpDouble))
                NewStation.Latitude = tmpDouble;
            Program.Dal.Add(NewStation);
        }

        /// <summary>
        /// Asks parcel details from the user and adds the new parcel to the current list.
        /// </summary>
        public static void AddParcel()
        {
            DateTime DateAndTime = new();
            DO.Parcel NewParcel = new();
            Console.WriteLine("*parcel sender ID (6 digits)");
            int.TryParse(Console.ReadLine(), out int Id1);

            NewParcel.Sender=Id1;
            Console.WriteLine("*parcel targetid");
            int.TryParse(Console.ReadLine(), out  Id1);

            NewParcel.Targetid = Id1;
            Console.WriteLine("*weight- for light type 0, for midium type 1, for heavy type 2");
            int.TryParse(Console.ReadLine(), out tmpInt);
            NewParcel.Weight = (WeightCategories)tmpInt;
            Console.WriteLine("*parcel Priority- for normal type 0, for fast type 1, for urgent type 2");
            int.TryParse(Console.ReadLine(), out tmpInt);
            NewParcel.Priority = (Priorities)tmpInt;
            NewParcel.MyDroneID = 0;
            NewParcel.Requested = DateTime.Now;
            NewParcel.Scheduled = DateAndTime;
            NewParcel.PickUp = DateAndTime;
            NewParcel.Delivered = DateAndTime;
            Program.Dal.Add(NewParcel);
        }

        /// <summary>
        /// Asks customer details from the user and adds the new customer to the current list
        /// </summary>
        public static void AddCustomer()
        {
            DO.Customer NewCustomer = new();
            Console.WriteLine("Enter the new customer details: *customer ID (9 digits)");
            int.TryParse(Console.ReadLine(), out int Id1);

            NewCustomer.Id = Id1;
            Console.WriteLine("*customer name");
            NewCustomer.Name = Console.ReadLine();
            Console.WriteLine("*customer phone number");
            NewCustomer.PhoneNumber = Console.ReadLine();
            Console.WriteLine("*customer longitude");
            double.TryParse(Console.ReadLine(), out tmpDouble);
            NewCustomer.Longitude = tmpDouble;
            Console.WriteLine("*customer latitude");
            double.TryParse(Console.ReadLine(), out tmpDouble);
            NewCustomer.Latitude = tmpDouble;
            Program.Dal.Add(NewCustomer);
        }

        /// <summary>
        /// Asks drone details from the user and adds the new drone to the current list
        /// </summary>
        public static void AddDrone()
        {
            DO.Drone NewDrone = new();
            Console.WriteLine("Enter the new drone details: *drone ID (3 digits)");
            if (int.TryParse(Console.ReadLine(), out tmpInt))
                NewDrone.Id = tmpInt;
            Console.WriteLine("*drone model");
            NewDrone.Model = Console.ReadLine();
            Console.WriteLine("*drones weight (for light type 0, midium type 1, heavy type 2)");
            int.TryParse(Console.ReadLine(), out tmpInt);
            NewDrone.Weight = (WeightCategories)tmpInt;
            Program.Dal.Add(NewDrone);
        }
        #endregion
    }
}
