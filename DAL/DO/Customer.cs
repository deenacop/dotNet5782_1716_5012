using Utilities;

namespace DO
{
    /// <summary>
    /// Customer details. Type DO i.e. filtered information
    /// </summary>
    public struct Customer
    {
        /// <summary>
        /// id number
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// customer name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
            
        public override string ToString()
        {
            string str = "Customer ID:" + Id + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nCustomer location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude)) + "\n";
            return str;
        }
    }
}

