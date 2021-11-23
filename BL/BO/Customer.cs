using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Customer details
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Location CustomerLocation { get; set; }
        public List<ParcelByCustomer> FromCustomer { get; set; }
        public List<ParcelByCustomer> TOCustomer { get; set; }
        public override string ToString()
        {
            string str = "Customer ID:" + CustomerID + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nCustomer location:\n" + CustomerLocation + "\nAll the parcels the were send from the customer" + String.Join(" ", FromCustomer) + "\nAll the parcels that the customer got " 
                 + String.Join(" ", TOCustomer);
            return str;
        }
    }
}
