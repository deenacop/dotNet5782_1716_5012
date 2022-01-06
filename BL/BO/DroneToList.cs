using System;

namespace BO
{
    /// <summary>
    /// Drone type object. A concise object for a list
    /// </summary>
    public class DroneToList
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// the drone weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// battery percentage
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// the drone status
        /// </summary>
        public DroneStatus Status { get; set; }
        /// <summary>
        /// location of the drone
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// The number of the parcel that is transfered
        /// </summary>
        public int ParcelId { get; set; }
        /// <summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
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
