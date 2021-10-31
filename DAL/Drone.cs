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
            public int ID { get; set; }
            public string Model { get; set; }
            public @enum.WeightCategories Weight { get; set; }
            public @enum.DroneStatus Status { get; set; }
            public int Battery { get; set; }
            public override string ToString()
            {
                string str = "Drone ID:" + ID + "\nDrone model:" + Model + "\nDrone weight:" + Weight + "\nDrone status:" + Status
                    + "\nDrone battery:" + Battery + "\n";
                return str;
            }
        }
    }
}
