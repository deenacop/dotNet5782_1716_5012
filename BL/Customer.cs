using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// customer detailes
    /// </summary>
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Location CustomerLocation { get; set; }
        public List<Parcel> FromCustomer { get; set; }
        public List<Parcel> TOCustomer { get; set; }
        public override string ToString()
        {
            string str = "Customer ID:" + ID + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nCustomer location:\n" + CustomerLocation + "\n";
            return str;
        }
    }
}
