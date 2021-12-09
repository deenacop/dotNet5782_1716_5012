using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            return "ID: " + Id + "\nModel: " + Model + "\nWhight: " + Weight +
                "\nBattery: " + Battery + "\nStatus: " + Status + "\nThe parcel that is transfer:" +
                Parcel + "\nLocation:" + Location + "\n";
        }
    }
}
