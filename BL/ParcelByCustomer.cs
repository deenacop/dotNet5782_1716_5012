using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    ///The details of the parcel by customer
    /// </summary>
    public class ParcelByCustomer
    {
        public int ParcelID { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public @enum.Priorities Priority { get; set; }
        public @enum.ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel SecondSideOfParcelCustomer { get; set; }
        public override string ToString()
        {
            return "Parcel ID: " + ParcelID + "\nParcel weight: " + Weight + "\nParcel priority: " +
                Priority + "\nParcel status: " + ParcelStatus + "\nThe second side of the parcel custumer: " +
                SecondSideOfParcelCustomer + "\n";
        }
    }
}
