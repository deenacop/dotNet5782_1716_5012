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
        /// View based on 60s of coordinate values.
        /// </summary>
        public class SexagesimalDegree
        {
            public static string convert(double lon, double lat)
            {
                string LatitudeAndLongitude = "";
                double LatDegreesWithFraction = lat;//48.858222;
                char LonDirection;
                char Latdirection;
                if (lat < 0)
                    Latdirection = 'W';
                else
                    Latdirection = 'E';
                double LonDegreesWithFraction = lon;//48.858222
                if (lon < 0)
                    LonDirection = 'S';
                else
                    LonDirection = 'N';

                int LatDegrees = (int)LatDegreesWithFraction; // = 48
                int LonDegrees = (int)LonDegreesWithFraction; // = 48

                double LatFractionalDegrees = LatDegreesWithFraction - LatDegrees; // = .858222
                double LonFractionalDegrees = LonDegreesWithFraction - LonDegrees; // = .858222

                double LatMinutesWithFraction = 60 * LatFractionalDegrees; // = 51.49332
                double LonMinutesWithFraction = 60 * LonFractionalDegrees; // = 51.49332

                int LatMinutes = (int)LatMinutesWithFraction; // = 51
                int LonMinutes = (int)LonMinutesWithFraction; // = 51

                double LatFractionalMinutes = LatMinutesWithFraction - LatMinutes; // = .49332
                double LonFractionalMinutes = LonMinutesWithFraction - LonMinutes; // = .49332

                double LatSecondsWithFraction = 60 * LatFractionalMinutes; // = 29.6
                double LonSecondsWithFraction = 60 * LonFractionalMinutes; // = 29.6

                float LatSeconds = (float)LatSecondsWithFraction; // = 30
                float LonSeconds = (float)LonSecondsWithFraction; // = 30

                LatitudeAndLongitude += LonDegrees + "°" + LonMinutes + "’" + LonSeconds + "’’" + LonDirection + "\n" +
                       LatDegrees + "°" + LatMinutes + "’" + LatSeconds + "’’" + Latdirection + "\n";

                return LatitudeAndLongitude;
            }

        }
    }
}
