using System;

namespace BO
{
    /// <summary>
    /// Parcel in transfer detailels
    /// </summary>
    public class ParcelInTransfer
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// parcel status
        /// </summary>
        public bool Status { get; set; }//Waiting for collection / on the way to the destination
        /// <summary>
        /// parcel weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// the sender. type: CustomerInParcel
        /// </summary>
        public CustomerInParcel SenderCustomer { get; set; }
        /// <summary>
        /// the receiver. type: CustomerInParcel
        /// </summary>
        public CustomerInParcel ReceiverCustomer { get; set; }
        /// <summary>
        ///the location of the collection
        /// </summary>
        public Location Collection { get; set; }
        /// <summary>
        /// the location of the delivery
        /// </summary>
        public Location Delivery { get; set; }
        /// <summary>
        /// distance of shipping
        /// </summary>
        public float TransportDistance { get; set; }
        public override string ToString()
        {
            string str = "ID:" + Id + "\nStatus:" + Status 
                + "\nWeight:" + Weight + "\nSender:" + SenderCustomer + "\nReceiver:" + ReceiverCustomer +
                 "\nCollection Location:" + Collection + "\nDelivery Location:" + Delivery +
                 "\nTransport Distance:" +  Math.Round(TransportDistance)+"\n";
            return str;
        }
    }
}
