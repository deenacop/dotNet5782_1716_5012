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
            public int Sender { get; set; }//acording the ID
            public int Targetid { get; set; }//acording the ID
            public @enum.WeightCategories Weight { get; set; }
            public bool DroneActionMode { get; set; }
            public DateTime Requested { get; set; }
            public DateTime PickUp { get; set; }
            public DateTime Delivered { get; set; }
            public DateTime Scheduled { get; set; }
        }
    }
}
