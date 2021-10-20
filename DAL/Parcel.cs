using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Parcel details
        /// </summary>
        public struct Parcel
        {
            public int ID { get; set; }
            public int Sender { get; set; }//according the ID
            public int Targetid { get; set; }//according the ID
            public @enum.WeightCategories Weight { get; set; }
            public @enum.Priorities Priority { get; set; }

            public int MyDroneID { get; set; }
            public DateTime Requested { get; set; }
            public DateTime PickUp { get; set; }
            public DateTime Delivered { get; set; }
            public DateTime Scheduled { get; set; }
            public override string ToString()
            {
                string str = "Parcel ID:" + ID + "\nParcel sender:" + Sender + "\nParcel targetid:" + Targetid + "\nParcel weight:" + Weight +
                "\nParcel drone ID:" + MyDroneID + "\nParcel time of request:" + Requested + "\nParcel time of schedule:" + Scheduled + "\nParcel time of delivery:" + Delivered + "\nParcel time of pick up:" + PickUp + "\n";
                return str;
            }
        }
    }
}
