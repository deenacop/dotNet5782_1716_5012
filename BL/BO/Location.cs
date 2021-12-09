using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace BO
{
    /// <summary>
    /// location longitude and latitude
    /// </summary>
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        // Longitude + Latitude

        public string _Latitude { get; set; }
       
        public string _Longitude { get; set; }
        public string _ToString { get { return ToString(); } set { } }

        public override string ToString()
        {
            return Util.SexagesimalCoordinate(Longitude, Latitude, _Longitude, _Latitude);
        }
    }
}
