﻿using System;

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
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel TargetidCustomer { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel MyDrone { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickUp { get; set; }
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
