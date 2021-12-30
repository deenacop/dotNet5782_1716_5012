using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A new class for a user 
    /// </summary>
    public class User
    {
        /// <summary>
        ///user name
        /// </summary>
        public string Name { get; set; }
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

        public override string ToString()
        {
            string str = "User name:" + Name + "\nUser mail address:" + EmailAddress + "\nIs the user a manager?" + IsManager + "\n";
            return str;
        }
    }
}
