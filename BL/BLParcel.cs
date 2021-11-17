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
        public void AddParcel(Parcel parcel)
        {
            if (ChackingNumOfDigits(parcel.Sender.CustomerID) != 9)
                throw new WrongIDException("Wrong ID"); 
            if (ChackingNumOfDigits(parcel.Targeted.CustomerID) != 9)
                throw new WrongIDException("Wrong ID");
            DateTime? DateAndTime = null;
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateAndTime;
            parcel.PickUp = DateAndTime;
            parcel.Delivered = DateAndTime;
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
