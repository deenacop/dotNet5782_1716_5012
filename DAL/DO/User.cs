using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DO
{
    /// <summary>
    /// A new class for a user 
    /// </summary>
    public class User
    {
        /// <summary>
        /// id number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///user name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// password for the user account
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// user mail
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// true for manager and false for regular customer
        /// </summary>
        public bool IsManager { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }
        public override string ToString()
        {
            string str ="User id"+Id+"\nUser name:"+Name+"\nUser mail address:"+EmailAddress+"\nUser phone number"+PhoneNumber+"\nIs the user a manager?"+IsManager+"\n"
                 +"\nUser location:\n" + (Util.SexagesimalCoordinate(Longitude, Latitude)) + "\n"; ;
            return str;
        }
    }
}
