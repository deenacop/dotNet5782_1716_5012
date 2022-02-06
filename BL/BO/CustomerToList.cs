using System;

namespace BO
{
    /// <summary>
    /// Customer type object. A concise object for a list
    /// </summary>
    public class CustomerToList
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of customer
        /// </summary>
        public string  Name { get; set; }
        /// <summary>
        /// the phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// amount of parcel that have been sent an delivered
        /// </summary>
        public int NumberParcelSentAndDelivered { get; set; }
        /// <summary>
        /// amount of parcel that only sent
        /// </summary>
        public int NumberParcelSentAndNOTDelivered { get; set; }
        /// <summary>
        /// amount of parcel that have been received
        /// </summary>
        public int NumberOfParcelReceived { get; set; }
        /// <summary>
        /// amount of parcel thar are on the way to the customer
        /// </summary>
        public int NumberOfParcelOnTheWayToCustomer { get; set; }
        public override string ToString()
        {
            string str = "ID:" + Id + "\nName:" + Name + "\nPhone number:" +
                PhoneNumber + "\nNumber Of Parcels Sent And Delivered " + NumberParcelSentAndDelivered +
                "\nNumber Of Parcels Sent And NOT Delivered " + NumberParcelSentAndNOTDelivered +
                "\nNumber Of Parcel Received " + NumberOfParcelReceived +
                "\nNumber Of Parcel OnThe Way To Customer " + NumberOfParcelOnTheWayToCustomer + "\n";
            return str;

        }
    }
}
