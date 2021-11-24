using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Drone details
        /// </summary>
        public struct Drone
        {
            public int DroneID { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public override string ToString()
            {
                string str = "Drone ID:" + DroneID + "\nDrone model:" + Model + "\nDrone weight:" + Weight +  "\n";
                return str;
            }
        }
    }
}
