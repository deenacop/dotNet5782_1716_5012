using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Parcel to list
    /// </summary>
    public class ParcelToList
    {
        public int ParcelID { get; set; }
        public string  NameOfSender { get; set; }
        public string NameOfTargetaed { get; set; }
        public @enum.WeightCategories Weight { get; set; }
        public @enum.Priorities Priority { get; set; }
        public @enum.ParcelStatus ParcelStatus { get; set; }
        public override string ToString()
        {
            string str = "Parcel ID:" + ParcelID + "\nSender's Customer name:" + NameOfSender + "\nReceiver's Customer name:" +
                 NameOfTargetaed + "\nParcel weight:" + Weight + "\nParcel Priority:" + Priority +
                 "\nParcel Status:" + ParcelStatus + "\n";
            return str;
        }

    }
}
