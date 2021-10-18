using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Drone charge details
        /// </summary>
        public struct DroneCharge
        {
            public int RecBaseStation { get; set; }
            public int RecDrone { get; set; }
            public override string ToString()
            {
                string str = "ID of the base station in drone charge:" + RecBaseStation + "\nID of the drone in drone charge:" + RecDrone + "\n" ;
                return str;
            }
        }
    }
}
