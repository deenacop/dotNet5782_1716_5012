using System;

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
