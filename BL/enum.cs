using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class @enum
    {
        /// <summary>
        /// The drone weights
        /// </summary>
        public enum WeightCategories { Light, Midium, Heavy };
        /// <summary>
        /// Delivery priorities
        /// </summary>
        public enum Priorities { Normal, Fast, Urgent };
        /// <summary>
        /// The parcel status(הוגדר,שויך,נאסף עי רחפן,סופק ללקוח)
        /// </summary>
        public enum ParcelStatus { Defined, Associated, PickedUp, Delivered }
        /// <summary>
        /// The Drone status
        /// </summary>
        public enum DroneStatus { Available, Maintenance, Delivery }
    }
}
