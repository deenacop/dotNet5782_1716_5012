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
        public int DroneID { get; set; }
        public string Model { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public @enum.DroneStatus DroneStatus { get; set; }
        public Location MyCurrentLocation { get; set; }
        /// <summary>
        /// The number of the parcel that is transfered
        /// </summary>
        public int ParcelNumberTransfered { get; set; }
        public override string ToString()
        {
            return "The drone ID: " + DroneID + "\n" + "The drone model: " + Model + "\n" +
                "Drone Weight: " + Weight + "\n" + "Drone's battery: " + Battery + "\n" +
                "Drone Status: " + DroneStatus + "\n" + "Drone location: " + MyCurrentLocation + "\n" +
                "The parcel number which is transferred: " + ParcelNumberTransfered + "\n";
            ;
        }
    }
}
