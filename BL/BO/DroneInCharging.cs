using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Drone in the charging slot
    /// </summary>
    public class DroneInCharging
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public DateTime? EnterToChargBase { get; set; }
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