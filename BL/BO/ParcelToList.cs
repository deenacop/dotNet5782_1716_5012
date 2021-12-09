using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Parcel to list
    /// </summary>
    public class ParcelToList
    {
        public int Id { get; set; }
        public string  NameOfSender { get; set; }
        public string NameOfTargetaed { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public override string ToString()
        {
            string str = "ID:" + Id + "\nSender's Customer name:" + NameOfSender + "\nReceiver's Customer name:" +
                 NameOfTargetaed + "\nWeight:" + Weight + "\nPriority:" + Priority +
                 "\nStatus:" + Status + "\n";
            return str;
        }

    }
}
