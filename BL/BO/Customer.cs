using System;
using System.Collections.Generic;

namespace BO
{
    /// <summary>
    /// Customer object. BL type - contains all the details of a client
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of customer
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// the location of the customer
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// List of parcel sent by the customer 
        /// </summary>
        public List<ParcelByCustomer> FromCustomer { get; set; } = new();
        /// <summary>
        /// List of packages received by the customer
        /// </summary>
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
