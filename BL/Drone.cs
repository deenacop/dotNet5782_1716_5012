using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        /// <summary>
        /// Drone details
        /// </summary>
        public int ID { get; set; }
        public string Model { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public @enum.DroneStatus DroneStatus { get; set; }
        public ParcelInTransfer MyParcel { get; set; }
        public Location MyCurrentLocation { get; set; }
        public override string ToString()
        {
            return "Drone ID: " + ID + "\n" + "Drone model: " + Model + "\n" +
                "Drone whight: " + Weight + "\n" + "Drone Battery: " + Battery + "\n"
                + "Drone status: " + DroneStatus + "\n" + "The parcel that is transfer:" + MyParcel + "\n" +
                "location:" + MyCurrentLocation + "\n";
        }

    }
}
