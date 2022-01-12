using System;
using System.Collections.Generic;

namespace BO
{
    /// <summary>
    /// Charging station object. The BL type includes all the details related to the station
    /// </summary>
    public class BaseStation
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of the station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// location of the station
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// amount of available slots at the station
        /// </summary>
        public int NumOfAvailableChargingSlots { get; set; }
        /// <summary>
        /// list of the drones that charged at the station
        /// </summary>
        public IEnumerable<DroneInCharging> DronesInCharging { get; set; }
        public override string ToString()
        {
           string str = "ID:" + Id + "\nName:" + Name + "\nLocation:" +
                 Location + "\nNumber Of Available Charging Slots:" + NumOfAvailableChargingSlots + 
                 "\nList of all the drones that are charging in the base station"+ String.Join(" ", DronesInCharging)+"\n"; 
            return str;
        }
    }
}