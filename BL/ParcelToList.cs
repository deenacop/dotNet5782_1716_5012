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
        public string  NameCustomerSendidng { get; set; }
        public string NameCustomerReceiving { get; set; }
        public @enum.WeightCategores Weight { get; set; }
        public @enum.Priority Priority { get; set; }
        public @enum.ParcelStatus Status { get; set; }
        public override string ToString()
        {
            string str = "Parcel ID:" + ParcelID + "\nSender's Customer name:" + NameCustomerSendidng + "\nReceiver's Customer name:" +
                 NameCustomerReceiving + "\nParcel weight:" + Weight + "\nParcel Priority:" + Priority +
                 "\nParcel Status:" + Status + "\n";
            return str;
        }

    }
}
