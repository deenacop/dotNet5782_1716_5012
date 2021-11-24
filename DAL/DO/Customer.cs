using Utilities;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Customer details
        /// </summary>
        public struct Customer
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                string str = "Customer ID:" + CustomerID + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                    PhoneNumber + "\nCustomer location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude))+"\n" ;
                return str;
            }
        }
    }
}
