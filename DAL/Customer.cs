using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Customer details
        /// </summary>
        public struct Customer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                string str = "Customer ID:" + ID + "\nCustomer name:" + Name + "\nCustomer phone number:" + PhoneNumber + "\nCustomer location:" + Longitude + "," + Latitude + "\n";
                return str;
            }
        }
    }
}
