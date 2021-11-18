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
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Targeted { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public @enum.Priorities Priority { get; set; }
        public DroneInParcel MyDrone { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            return "Parcel ID: " + ParcelID + "\nThe sender details: " + Sender + "\nThe tergeted details: " + Targeted +
                "\nParcel weight: " + Weight + "\nParcel priority: " + Priority + "\nThe drone that associated with this parcel: " +
                MyDrone + "\nTime of requested: " + Requested + "\nTime of schedual: " + Scheduled + "\nTime of pick up: " + PickUp +
                "\nTime of delivery: " + Delivered + "\n";
        }

    }
}