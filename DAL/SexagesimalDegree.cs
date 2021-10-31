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
                double LatDegreesWithFraction = lat;
                char LonDirection;
                char Latdirection;
                if (lat < 0)
                    Latdirection = 'W';
                else
                    Latdirection = 'E';
                double LonDegreesWithFraction = lon;//example: 48.858222
                if (lon < 0)
                    LonDirection = 'S';
                else
                    LonDirection = 'N';//example: =N

                int LatDegrees = (int)LatDegreesWithFraction; // Converts the degrees to an integer
                int LonDegrees = (int)LonDegreesWithFraction; //example:  = 48

                double LatFractionalDegrees = LatDegreesWithFraction - LatDegrees; // Finds the minutes by finding the fraction within the initial number he received and then multiplying by 60
                double LonFractionalDegrees = LonDegreesWithFraction - LonDegrees; //example:  = .858222

                double LatMinutesWithFraction = 60 * LatFractionalDegrees; //multiplying the fraction by 60
                double LonMinutesWithFraction = 60 * LonFractionalDegrees; //example:  = 51.49332

                int LatMinutes = (int)LatMinutesWithFraction; // Converts the minutes to an integer
                int LonMinutes = (int)LonMinutesWithFraction; //example:  = 51

                double LatFractionalMinutes = LatMinutesWithFraction - LatMinutes; // Finds the seconds by finding the fraction within the initial number he received and then multiplying by 60
                double LonFractionalMinutes = LonMinutesWithFraction - LonMinutes; //example:  = .49332

                double LatSecondsWithFraction = 60 * LatFractionalMinutes; // multiplying the fraction by 60
                double LonSecondsWithFraction = 60 * LonFractionalMinutes; //example:  = 29.6

                float LatSeconds = (float)LatSecondsWithFraction; // Convert the seconds to a float
                float LonSeconds = (float)LonSecondsWithFraction; //example:  = 30

                LatitudeAndLongitude += LonDegrees + "°" + LonMinutes + "’" + LonSeconds + "’’" + LonDirection + "\n" +
                       LatDegrees + "°" + LatMinutes + "’" + LatSeconds + "’’" + Latdirection + "\n";

                return LatitudeAndLongitude;
            }
        }
    }
}
