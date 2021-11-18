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
        public int DroneID { get; set; }
        public string Model { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public @enum.DroneStatus DroneStatus { get; set; }
        public ParcelInTransfer MyParcel { get; set; }
        public Location MyCurrentLocation { get; set; }
        public override string ToString()
        {
            return "Drone ID: " + DroneID + "\nDrone model: " + Model + "\nDrone whight: " + Weight +
                "\nDrone Battery: " + Battery + "\nDrone status: " + DroneStatus + "\nThe parcel that is transfer:" +
                MyParcel + "\nlocation:" + MyCurrentLocation + "\n";
        }
    }
}
