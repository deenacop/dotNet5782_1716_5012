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
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerInParcel SecondSideOfParcelCustomer { get; set; }
        public override string ToString()
        {
            return "ID: " + Id + "\nweight: " + Weight + "\npriority: " +
                Priority + "\nstatus: " + Status + "\nThe second side of the parcel custumer: " +
                SecondSideOfParcelCustomer + "\n";
        }
    }
}
