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
            string str = "ID: " + Id + "\nmodel: " + Model + "\nWeight: " + Weight + "\nbattery: " +
                Battery + "\nStatus: " + Status + "\nlocation: " + Location + "\n";
            if (ParcelId != 0)
                str = str + "The parcel's id which is transferred: " + ParcelId + "\n";
            return str;
            ;
        }
    }
}
