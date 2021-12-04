using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// The drone weights
    /// </summary>
    public enum WeightCategories { Light, Medium, Heavy };
    /// <summary>
    /// Delivery priorities
    /// </summary>
    public enum Priorities { Normal, Fast, Urgent };
    /// <summary>
    /// The parcel status(הוגדר,שויך,נאסף עי רחפן,סופק ללקוח)
    /// </summary>
    public enum ParcelStatus { Defined, Associated, PickedUp, Delivered }
    /// <summary>
    /// The Drone status(פנוי,תחוזקה,משלוח)
    /// </summary>
    public enum DroneStatus { Available, Maintenance, Delivery }
}
