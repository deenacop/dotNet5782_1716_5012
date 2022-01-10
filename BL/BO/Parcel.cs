using System;

namespace BO
{
    /// <summary>
    /// Parcel object. BL type - contains all the details of a parcel
    /// </summary>
    public class Parcel
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the customer that send the parcel
        /// </summary>
        public CustomerInParcel SenderCustomer { get; set; }
        /// <summary>
        /// the customer that recieved the parcel
        /// </summary>
        public CustomerInParcel TargetidCustomer { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// Priority
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// the drone tha assign to the parcel
        /// </summary>
        public DroneInParcel MyDrone { get; set; }
        /// <summary>
        /// time of Requeste
        /// </summary>
        public DateTime? Requested { get; set; }
        /// <summary>
        /// time of Scheduled
        /// </summary>
        public DateTime? Scheduled { get; set; }
        /// <summary>
        /// time of PickUp
        /// </summary>
        public DateTime? PickUp { get; set; }
        /// <summary>
        /// time of Delivery
        /// </summary>
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            string str = "ID: " + Id + "\nThe sender details: " + SenderCustomer + "\nThe tergeted details: " + TargetidCustomer +
                "\nweight: " + Weight + "\npriority: " + Priority + "\nThe drone that associated with this parcel: " +
                MyDrone + "\nTime of requested: " + Requested;
            if (Scheduled != null)
                str = str + "\nTime of schedual: " + Scheduled;
            if (PickUp != null)
                str = str + "\nTime of pick up: " + PickUp;
            if (Delivered != null)
                str = str + "\nTime of delivery " + Delivered + "\n";
            return str;
        }

    }
}
