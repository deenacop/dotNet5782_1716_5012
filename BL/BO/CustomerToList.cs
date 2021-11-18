using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// costomer to list
    /// </summary>
    public class CustomerToList
    {
        public int CustomerID { get; set; }
        public string  Name { get; set; }
        public int PhoneNumber { get; set; }
        public int NumberParcelSentAndDelivered { get; set; }
        public int NumberParcelSentAndNOTDelivered { get; set; }
        public int NumberOfParcelReceived { get; set; }
        public int NumberOfParcelOnTheWayToCustomer { get; set; }
        public override string ToString()
        {
            string str = "Customer ID:" + CustomerID + "\nCustomer name:" + Name + "\nCustomer phone number:" +
                PhoneNumber + "\nNumber Of Parcels Sent And Delivered" + NumberParcelSentAndDelivered +
                "\nNumber Of Parcels Sent And NOT Delivered" + NumberParcelSentAndNOTDelivered +
                "\nNumber Of Parcel Received" + NumberOfParcelReceived +
                "\nNumber Of Parcel OnThe Way To Customer" + NumberOfParcelOnTheWayToCustomer + "\n";
            return str;
            
        }
    }
}
