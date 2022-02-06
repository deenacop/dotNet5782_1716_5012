using System;
using System.Collections.Generic;

namespace DO
{
    /// <summary>
    /// Parcel details. Type DO i.e. filtered information
    /// </summary>
    public struct Parcel
    {
        /// <summary>
        /// id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of seder customer 
        /// </summary>
        public string Sender { get; set; }//according the ID
        /// <summary>
        /// Id of targetid customer
        /// </summary>
        public string Targetid { get; set; }//according the ID
        /// <summary>
        /// parcel weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// parcel priority
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// If the parcel is associated: The number of the drone to which the parcel is associated
        /// </summary>
        public int MyDroneID { get; set; }
        /// <summary>
        /// time of request
        /// </summary>
        public DateTime? Requested { get; set; }
        /// <summary>
        /// time of scheduled
        /// </summary>
        public DateTime? Scheduled { get; set; }
        /// <summary>
        /// pick up time (from the sender)
        /// </summary>
        public DateTime? PickUp { get; set; }
        /// <summary>
        /// time of arrival at the destination
        /// </summary>
        public DateTime? Delivered { get; set; }
        ///<summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
        public override string ToString()
        {
            string str = "Parcel ID:" + Id + "\nParcel sender:" + Sender + "\nParcel targetid:" + Targetid +
            "\nParcel weight:" + Weight + "\nParcel priority:" + Priority +
            "\nParcel drone ID:" + MyDroneID + "\nParcel time of request:" + Requested;
            if (Scheduled != DateTime.MinValue)
                str += "\nParcel time of schedule:" + Scheduled;
            if (PickUp != DateTime.MinValue)
                str += "\nParcel time of pick up:" + PickUp;
            if (Delivered != DateTime.MinValue)
                str += "\nParcel time of delivery:" + Delivered + "\n";
            return str;
        }
    }
}

