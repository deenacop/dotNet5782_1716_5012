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
    public class BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int NumOfAvailableChargingSlots { get; set; }
        public List<DroneInCharging> DronesInCharging { get; set; }
        public override string ToString()
        {
           string str = "Station ID:" + Id + "\nStation name:" + Name + "\nThe Station's Location:" +
                 Location + "\nNumber Of Available Charging Slots:" + NumOfAvailableChargingSlots + 
                 "\nList of all the drones that are charging in the base station"+ String.Join(" ", DronesInCharging)+"\n"; 
            return str;
        }
    }
}