using System;

namespace DO
{
    /// <summary>
    /// Drone charge details. Type DO i.e. filtered information
    /// </summary>
    public struct DroneCharge
    {
        /// <summary>
        /// ID of the station 
        /// </summary>
        public int BaseStationID { get; set; }
        /// <summary>
        /// ID of the drone
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Entery time
        /// </summary>
        public DateTime? EnterToChargBase { get; set; }
        /// <summary>
        /// Departure time
        /// </summary>
        public DateTime? FinishedRecharging { get; set; }
        ///<summary>
        /// Field that tells you if the item has been removed or not
        /// </summary>
        public bool IsRemoved { get; set; }
        public override string ToString()
        {
            string str = "ID of the base station in drone charge:" + BaseStationID + "\nID of the drone in drone charge:" + Id + "\n";
            return str;
        }
    }
}

