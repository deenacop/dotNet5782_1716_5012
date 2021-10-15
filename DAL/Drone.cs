﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Drone details
        /// </summary>
        public struct Drone
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public @enum.WeightCategories Weight { get; set; }
            public @enum.DroneStatus Status { get; set; }
            public double Battery { get; set; }
            public @enum.Priorities Priority { get; set; }
        }
    }

}
