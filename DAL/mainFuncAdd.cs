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
            public static String Input ;
            public static int TmpInt;
            public static double TmpDouble;
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
                NewStation.NumOfAvailableChargeSlots = (int)Console.Read();
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
                NewDrone.Weight = (@enum.WeightCategories)(int)Console.Read();
                NewDrone.Battery = 100;
                NewDrone.Status = 0;
                IDAL.DO.DalObject.Add(NewDrone);
            }
        }
    }
}
