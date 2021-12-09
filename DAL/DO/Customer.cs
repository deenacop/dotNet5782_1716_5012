using Utilities;


namespace DO
{
    /// <summary>
    /// Customer details
    /// </summary>
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            string str = "Customer ID:" + Id + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nCustomer location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude)) + "\n";
            return str;
        }
    }
}

