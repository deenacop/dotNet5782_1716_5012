using System;

namespace DO
{
    /// <summary>
    /// Drone charge details. Type DO i.e. filtered information
    /// </summary>
    public struct DroneCharge
    {
        public int BaseStationID { get; set; }
        public int Id { get; set; }
        public DateTime? EnterToChargBase { get; set; }

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

