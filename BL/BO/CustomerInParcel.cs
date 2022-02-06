using System;

namespace BO
{
    /// <summary>
    /// The Customer in a parcel details
    /// </summary>
    public class CustomerInParcel
    {
        /// <summary>
        /// Id number
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// name of customer
        /// </summary>
        public string Name { get; set; }
        public override string ToString()
        {
            return "ID: " + Id + "\nName: " + Name + "\n";
        }
    }
}
