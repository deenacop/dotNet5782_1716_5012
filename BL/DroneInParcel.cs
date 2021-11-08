using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// The drone that is carring the parcel
    /// </summary>
    class DroneInParcel
    {
        public int DroneID { get; set; }
        public int Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            string str = "Drone ID:" + DroneID + "\nDrone battery:" + Battery +
                "\nDrones's Current Location:\n" + CurrentLocation + "\n";
            return str;
        }
    }
}
