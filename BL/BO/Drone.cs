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
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public DroneStatus Status { get; set; }
        public ParcelInTransfer Parcel { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            return "Drone ID: " + Id + "\nDrone model: " + Model + "\nDrone whight: " + Weight +
                "\nDrone Battery: " + Battery + "\nDrone status: " + Status + "\nThe parcel that is transfer:" +
                Parcel + "\nlocation:" + Location + "\n";
        }
    }
}
