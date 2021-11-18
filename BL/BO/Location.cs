using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// location longitude and latitude
    /// </summary>
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            return (IDAL.DO.SexagesimalDegree.convert(Longitude, Latitude));
        }
    }
}
