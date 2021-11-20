using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        /// <summary>
        /// Adds a parcel to the list of parcels in the IDAL
        /// </summary>
        /// <param name="parcel">The wanted parcel</param>
        public void AddParcel(Parcel parcel)
        {
            if (ChackingNumOfDigits(parcel.Sender.CustomerID) != 9)
                throw new WrongIDException("Wrong ID"); 
            if (ChackingNumOfDigits(parcel.Targetid.CustomerID) != 9)
                throw new WrongIDException("Wrong ID");
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.PickUp = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            parcel.MyDrone = null;
            try
            {
                IDAL.DO.Parcel tmpParcel = new();
                parcel.CopyPropertiesTo(tmpParcel);
                dal.Add(tmpParcel);
            }
            catch (IDAL.DO.AlreadyExistedItemException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
