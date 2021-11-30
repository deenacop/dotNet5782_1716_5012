﻿using System;
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
            public int ParcelID { get; set; }
            public int Sender { get; set; }//according the ID
            public int Targetid { get; set; }//according the ID
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public int MyDroneID { get; set; }
            public DateTime? Requested { get; set; }
            public DateTime? Scheduled { get; set; }
            public DateTime? PickUp { get; set; }
            public DateTime? Delivered { get; set; }

            public override string ToString()
            {
                string str = "Parcel ID:" + ParcelID + "\nParcel sender:" + Sender + "\nParcel targetid:" + Targetid +
                "\nParcel weight:" + Weight + "\nParcel priority:" + Priority +
                "\nParcel drone ID:" + MyDroneID + "\nParcel time of request:" + Requested;
                if (Scheduled != DateTime.MinValue)
                    str += "\nParcel time of schedule:" + Scheduled;
                if (PickUp != DateTime.MinValue)
                    str += "\nParcel time of pick up:" + PickUp;
                if (Delivered != DateTime.MinValue)
                    str += "\nParcel time of delivery:" + Delivered + "\n";
                return str;
            }
        }
    }
}
