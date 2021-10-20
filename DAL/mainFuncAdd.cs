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
        }
    }
}
