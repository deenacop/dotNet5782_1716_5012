using Utilities;

namespace DO
{
    /// <summary>
    /// Staitions details. Type DO i.e. filtered information
    /// </summary>
    public struct Station
    {
        /// <summary>
        ///id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// amount of station
        /// </summary>
        public int NumOfAvailableChargingSlots { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }
        ///<summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
        public override string ToString()
        {
            string str = "Station ID:" + Id + "\nStation name:" + Name + "\nNumber of available slots:" + NumOfAvailableChargingSlots
                + "\nStation location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude)) + "\n";
            return str;
        }
    }
}

