using System;

namespace BO
{
    /// <summary>
    /// Drone  in charge object. Type BL -contains all the required details
    /// </summary>
    public class DroneInCharging
    {
        /// <summary>
        /// Id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// battery percentage
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// Entry time for charging
        /// </summary>
        public DateTime? EnterToChargBase { get; set; }
        /// <summary>
        /// Charging end time
        /// </summary>
        public DateTime? FinishedRecharging { get; set; }
        public override string ToString()
        {
            string str = "";
            if (FinishedRecharging == null)
            {
                str += "\nID:" + Id + "\tBattery:" + Battery;
            }
            return str;
        }
    }
}