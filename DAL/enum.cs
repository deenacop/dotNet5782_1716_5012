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
            public enum WeightCategories { light, midium, heavy };
            /// <summary>
            /// In what stage the drone is in
            /// </summary>
            public enum DroneStatus { available, maintenance, delivery };
            /// <summary>
            /// Delivery priorities
            /// </summary>
            public enum Priorities { normal, fast, urgent };
        }
    }
}
