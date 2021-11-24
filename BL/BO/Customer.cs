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
        public List<ParcelByCustomer> FromCustomer { get; set; } = new();
        public List<ParcelByCustomer> TOCustomer { get; set; } = new();
        public override string ToString()
        {
            string str = "Customer ID:" + CustomerID + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nCustomer location:\n" + CustomerLocation+ "\n" ;
            if (FromCustomer.Capacity > 0)
                str+="All the parcels that were send from the customer:\n" + String.Join(" ", FromCustomer)+"\n";
            if(TOCustomer.Capacity>0)
                //str+= "All the parcels that the customer got:\n" + String.Join(" ", TOCustomer) + "\n";
            return str;
        }
    }
}
