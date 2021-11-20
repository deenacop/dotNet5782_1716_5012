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
        /// <summary>
        /// Display parcel
        /// </summary>
        /// <param name="ID">The ID of the wanted parcel</param>
        /// <returns>The wanted parcel</returns>
        public Parcel ParcelDisplay(int ID)
        {
            IDAL.DO.Parcel parcelDO = new();
            try
            {
                parcelDO = dal.ParcelDisplay(ID);
            }
            catch(Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            Parcel parcelBO = new();
            parcelDO.CopyPropertiesTo(parcelBO);
            parcelBO.Sender.CustomerID = dal.CustomerDisplay(parcelDO.Sender).CustomerID;
            parcelBO.Sender.Name = dal.CustomerDisplay(parcelDO.Sender).Name;
            parcelBO.Targetid.CustomerID = dal.CustomerDisplay(parcelDO.Targetid).CustomerID;
            parcelBO.Targetid.Name = dal.CustomerDisplay(parcelDO.Targetid).Name;
            if(parcelDO.Scheduled!=DateTime.MinValue)//if the parel is assigned
            {
                DroneToList drone = DroneListBL.Find(i => i.DroneID == parcelDO.MyDroneID);
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }
    }
}
