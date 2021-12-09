using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Customer details
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Location Location { get; set; }
        public List<ParcelByCustomer> FromCustomer { get; set; } = new();
        public List<ParcelByCustomer> ToCustomer { get; set; } = new();
        public override string ToString()
        {
            string str = "ID:" + Id + "\nName:" + Name + "\nPhone number:" +
                PhoneNumber + "\nLocation:\n" + Location+"\n" ;
            if (FromCustomer.Capacity > 0)
                str+="All the parcels that were send from the customer:\n" + String.Join(" ", FromCustomer);
            if(ToCustomer.Capacity>0)
                str+= "All the parcels that the customer got:\n" + String.Join(" ", ToCustomer);
            return str;
        }
    }
}
