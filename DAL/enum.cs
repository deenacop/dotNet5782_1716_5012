using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct @enum
        {
            /// <summary>
            /// The drone weights
            /// </summary>
            public enum WeightCategories { Light, Midium, Heavy };
            /// <summary>
            /// Delivery priorities
            /// </summary>
            public enum Priorities { Normal, Fast, Urgent };
        }
    }
}
