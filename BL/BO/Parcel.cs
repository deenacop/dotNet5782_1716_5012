﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Parcel details
    /// </summary>
    public class Parcel
    {
        public int ParcelID { get; set; }
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel TargetidCustomer { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel MyDrone { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            string str = "Parcel ID: " + ParcelID + "\nThe sender details: " + SenderCustomer + "\nThe tergeted details: " + TargetidCustomer +
                "\nParcel weight: " + Weight + "\nParcel priority: " + Priority + "\nThe drone that associated with this parcel: " +
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
