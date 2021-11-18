using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Base stations details
    /// </summary>
    public class T
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public Location StationLocation { get; set; }
        public int NumOfAvailableChargingSlots { get; set; }
        public List<DroneInCharging> DronesInCharging { get; set; }
        public override string ToString()
        {
           string str = "Station ID:" + StationID + "\nStation name:" + Name + "\nThe Station's Location:" +
                 StationLocation + "\nNumber Of Available Charging Slots:" + NumOfAvailableChargingSlots + "\n";
            return str;
        }
    }
}
