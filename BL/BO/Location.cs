using System;
using Utilities;

namespace BO
{
    /// <summary>
    /// Location object. BL type - contains all the details of a location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// the Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// the Latitude
        /// </summary>
        public double Latitude { get; set; }
        // Longitude + Latitude

        /// <summary>
        /// Latitude as a string
        /// </summary>
        public string LatitudeSTR { get; set; }
        /// <summary>
        /// Longitude as a string
        /// </summary>
        public string LongitudeSTR { get; set; }
        /// <summary>
        /// print
        /// </summary>
        public string _ToString { get { return ToString(); } set { } }

        public override string ToString()
        {
            return Util.SexagesimalCoordinate(Longitude, Latitude, LongitudeSTR, LatitudeSTR);
        }
    }
}
