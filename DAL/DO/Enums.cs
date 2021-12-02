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
            /// The drone weights
            /// </summary>
            public enum WeightCategories { Light, Medium, Heavy, Max = Heavy };
            /// <summary>
            /// Delivery priorities
            /// </summary>
            public enum Priorities { Normal, Fast,  Urgent, Max = Urgent };
        }
}
