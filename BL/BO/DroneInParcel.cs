using System;

namespace BO
{
    /// <summary>
    /// The drone that is carring the parcel's details
    /// </summary>
    public class DroneInParcel
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// battery percentage
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// location of the drone
        /// </summary>
        public Location Location { get; set; }
        public override string ToString()
        {
            string str = "ID:" + Id + "\nbattery:" + Battery +
                "\nLocation:\n" + Location + "\n";
            return str;
        }
    }
}
