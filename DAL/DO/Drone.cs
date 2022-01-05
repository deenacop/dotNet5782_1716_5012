using System;

namespace DO
{
    /// <summary>
    /// Drone details. Type DO i.e. filtered information
    /// </summary>
    public struct Drone
    {
        /// <summary>
        /// id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// drone model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// drone weight
        /// </summary>

        /// <summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
        public WeightCategories Weight { get; set; }
        public override string ToString()
        {
            string str = "Drone ID:" + Id + "\nDrone model:" + Model + "\nDrone weight:" + Weight + "\n";
            return str;
        }
    }
}

