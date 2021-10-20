using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class mainFuncAdd
        {
            public static string Input;
            public static int TmpInt;
            public static double TmpDouble;
            public static void AddStation()
            {
                IDAL.DO.Station NewStation = new IDAL.DO.Station();
                Console.WriteLine("Enter the new station details: *station ID (4 digits)");
                NewStation.ID = Console.Read();
                Console.WriteLine("*station name");
                NewStation.Name = Console.ReadLine();
                Console.WriteLine("*station number of available charge slots");
                NewStation.NumOfAvailableChargeSlots = Console.Read();
                Console.WriteLine("*station longitude");
                NewStation.Longitude = Console.Read();
                Console.WriteLine("*station latitude");
                NewStation.Latitude = Console.Read();
                IDAL.DO.DalObject.Add(NewStation);
            }
            public static void AddParcel()
            {
                IDAL.DO.Parcel NewParcel = new IDAL.DO.Parcel();
                Console.WriteLine("Enter the new parcel details: *pracel ID (6 digits)");
                Input = Console.ReadLine();
                if(int.TryParse(Input, out TmpInt))
                NewParcel.ID = TmpInt;
                Console.WriteLine("*parcel sender ID (6 digits)");
                Input = Console.ReadLine();
                if(int.TryParse(Input ,out TmpInt))
                NewParcel.Sender = TmpInt;
                Console.WriteLine("*parcel targetid");
                Input = Console.ReadLine();
                if(int.TryParse(Input, out TmpInt))
                NewParcel.Targetid = TmpInt;
                Console.WriteLine("*weight- for light type 0, for midium type 1, for heavy type 2");
                NewParcel.Weight = (@enum.WeightCategories)(int)Console.Read();
                Console.WriteLine("*parcel Priority- for normal type 0, for fast type 1, for urgent type 2");
                NewParcel.Priority = (@enum.Priorities)(int)Console.Read();
                NewParcel.MyDroneID = 0;
                NewParcel.Requested = DateTime.Now;
                NewParcel.Scheduled = DateTime.Now;
                NewParcel.PickUp = DateTime.Now;
                NewParcel.Delivered = DateTime.Now;
                IDAL.DO.DalObject.Add(NewParcel);
            }
            public static void AddCustomer()
            {
                IDAL.DO.Customer NewCustomer = new IDAL.DO.Customer();
                Console.WriteLine("Enter the new customer details: *customer ID (9 digits)");
                Input= Console.ReadLine();
                if(int.TryParse(Input, out TmpInt))
                    NewCustomer.ID=TmpInt;
                Console.WriteLine("*customer name");
                NewCustomer.Name = Console.ReadLine();
                Console.WriteLine("*customer longitude");
                Input= Console.ReadLine();
                double.TryParse(Input, out TmpDouble);
                NewCustomer.Longitude = TmpDouble;
                Console.WriteLine("*customer latitude");
                Input = Console.ReadLine();
                double.TryParse(Input, out TmpDouble);
                NewCustomer.Latitude = TmpDouble;
                IDAL.DO.DalObject.Add(NewCustomer);
            }
        }
    }
}
