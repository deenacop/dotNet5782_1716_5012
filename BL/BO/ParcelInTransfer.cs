using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Parcel in transfer detailels
    /// </summary>
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool Status { get; set; }//Waiting for collection / on the way to the destination
        public WeightCategories Weight { get; set; }
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel ReceiverCustomer { get; set; }
        public Location Collection { get; set; }
        public Location Delivery { get; set; }
        public double TransportDistance { get; set; }
        public override string ToString()
        {
            string str = "Parcel ID:" + Id + "\nParcel status:" + Status 
                + "\nParcel weight:" + Weight + "\nParcel's sender:" + SenderCustomer + "\nParcel's receiver:" + ReceiverCustomer +
                 "\nCollection Location:" + Collection + "\nDelivery Location:" + Delivery +
                 "\nTransport Distance:" + TransportDistance+"\n";
            return str;
        }
    }
}
