using System;

namespace BO
{
    /// <summary>
    ///The details of the parcel that is by a customer
    /// </summary>
    public class ParcelByCustomer
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// parcel weight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// parcel priority
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// parcel status
        /// </summary>
        public ParcelStatus Status { get; set; }
        /// <summary>
        /// The other side that receives the parcel. If it is a parcel frome sender - a second side it receives and if the reverse
        /// </summary>
        public CustomerInParcel SecondSideOfParcelCustomer { get; set; }
        public override string ToString()
        {
            return "ID: " + Id + "\nweight: " + Weight + "\npriority: " +
                Priority + "\nstatus: " + Status + "\nThe second side of the parcel custumer: " +
                SecondSideOfParcelCustomer + "\n";
        }
    }
}
