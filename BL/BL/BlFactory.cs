using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
   public class BlFactory
    {
        /// <summary>
        /// returm the Instance of BL
        /// </summary>
        /// <returns>Instance</returns>
        public static IBL GetBl()
        {
            return BL.BL.Instance;

        }
    }
}
