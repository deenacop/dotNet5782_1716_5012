using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Base station to list
    /// </summary>
    public class BaseStationToList
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public int NumOfAvailableChargingSlots { get; set; }
        public int NumOfBusyChargingSlots { get; set; }
        public override string ToString()
        {
            string str = "Station ID:" + StationID + "\nStation name:" + Name 
                 + "\nNumber Of Available Charging Slots:" + NumOfAvailableChargingSlots +
                  "\nNumber Of Busy Charging Slots:" + NumOfBusyChargingSlots+"\n";
            return str;
        }

    }
}
