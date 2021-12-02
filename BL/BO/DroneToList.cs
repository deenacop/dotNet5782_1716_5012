using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Drone to a list
    /// </summary>
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public DroneStatus Status { get; set; }
        public Location Location { get; set; }
        /// <summary>
        /// The number of the parcel that is transfered
        /// </summary>
        public int ParcelId { get; set; }
        public override string ToString()
        {
            string str = "Drone ID: " + Id + "\nDrone model: " + Model + "\nDrone Weight: " + Weight + "\nDrone's battery: " +
                Battery + "\nDrone Status: " + Status + "\nDrone's location: " + Location + "\n";
            if (ParcelId != 0)
                str = str + "The parcel number which is transferred: " + ParcelId + "\n";
            return str;
            ;
        }
    }
}
