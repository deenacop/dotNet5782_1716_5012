using System;

namespace BO
{
    /// <summary>
    /// Station type object. A concise object for a list
    /// </summary>
    public class BaseStationToList
    {
        /// <summary>
        /// id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of the station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// amount of available slots at the station
        /// </summary>
        public int NumOfAvailableChargingSlots { get; set; }
        /// <summary>
        /// amount of busy slots at the station
        /// </summary>
        public int NumOfBusyChargingSlots { get; set; }
        public override string ToString()
        {
            string str = "ID:" + Id + "\nName:" + Name 
                 + "\nNumber Of Available Charging Slots:" + NumOfAvailableChargingSlots +
                  "\nNumber Of Busy Charging Slots:" + NumOfBusyChargingSlots+"\n";
            return str;
        }

    }
}
