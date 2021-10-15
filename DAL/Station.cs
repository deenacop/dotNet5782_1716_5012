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
        /// Staitions details
        /// </summary>
        public struct Station
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int NumOfChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
    }
}
