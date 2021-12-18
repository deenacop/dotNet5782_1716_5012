using System;

namespace BO
{
    /// <summary>
    /// Parcel type object. A concise object for a list
    /// </summary>
    public class ParcelToList
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of the sender customer
        /// </summary>
        public string  NameOfSender { get; set; }
        /// <summary>
        /// name of the targetid 
        /// </summary>
        public string NameOfTargetid { get; set; }
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
        public override string ToString()
        {
            string str = "ID:" + Id + "\nSender's Customer name:" + NameOfSender + "\nReceiver's Customer name:" +
                 NameOfTargetid + "\nWeight:" + Weight + "\nPriority:" + Priority +
                 "\nStatus:" + Status + "\n";
            return str;
        }

    }
}
