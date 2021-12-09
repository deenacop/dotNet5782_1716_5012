using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Drone details
    /// </summary>
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public override string ToString()
        {
            string str = "Drone ID:" + Id + "\nDrone model:" + Model + "\nDrone weight:" + Weight + "\n";
            return str;
        }
    }
}

