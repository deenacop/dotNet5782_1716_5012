using Utilities;



namespace DO
{
    /// <summary>
    /// Staitions details
    /// </summary>
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumOfAvailableChargingSlots { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            string str = "Station ID:" + Id + "\nStation name:" + Name + "\nNumber of available slots:" + NumOfAvailableChargingSlots
                + "\nStation location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude)) + "\n";
            return str;
        }
    }
}

