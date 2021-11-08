using System;
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
        public int  ParcelID { get; set; }
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
            return "Parcel ID: "+ParcelID + "\n"+"The sender: "+Sender + "\n"+"The tergeted: "+Targeted + 
                "\n"+"Parcel weight: "+Weight + "\n"+"Parcel priority: "+Priority + "\n"+
                "The drone that associated with this parcel: "+MyDrone + "\n"+"Time of requested: "+Requested + 
                "\n"+ "Time of schedual: " + Scheduled + "\n"+ "Time of pick up: " + PickUp + "\n"+ "Time of delivery: " + Delivered + "\n";
        }

    }
}
