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
    public class DroneInParcel
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            string str = "Drone ID:" + Id + "\nDrone battery:" + Battery +
                "\nDrones's Current Location:\n" + Location + "\n";
            return str;
        }
    }
}
