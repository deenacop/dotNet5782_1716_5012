using System;

namespace BO
{
    /// <summary>
    /// Drone object. BL type - contains all the details of a drone
    /// </summary>
    public class Drone
    {
        ///   /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// drone model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// the weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// battery percentage
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// status of the drone
        /// </summary>
        public DroneStatus Status { get; set; }
        /// <summary>
        /// The parcel that associated to the drone - in case and exists
        /// </summary>
        public ParcelInTransfer Parcel { get; set; }
        /// <summary>
        /// location of the drone
        /// </summary>
        public Location Location { get; set; }
        public override string ToString()
        {
            return "ID: " + Id + "\nModel: " + Model + "\nWhight: " + Weight +
                "\nBattery: " + Battery + "\nStatus: " + Status + "\nThe parcel that is transfer:" +
                Parcel + "\nLocation:" + Location + "\n";
        }
    }
}
