using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

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
                object obj = tmpParcel;
                parcel.CopyPropertiesTo(obj);
                tmpParcel = (IDAL.DO.Parcel)obj;
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
            Parcel parcelBO = new();
            try
            {
                parcelDO = dal.ParcelDisplay(ID);
                parcelDO.CopyPropertiesTo(parcelBO);
                parcelBO.Sender.CustomerID = dal.CustomerDisplay(parcelDO.Sender).CustomerID;
                parcelBO.Sender.Name = dal.CustomerDisplay(parcelDO.Sender).Name;
                parcelBO.Targetid.CustomerID = dal.CustomerDisplay(parcelDO.Targetid).CustomerID;
                parcelBO.Targetid.Name = dal.CustomerDisplay(parcelDO.Targetid).Name;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            ///האם קיים הdrone?
            if (parcelDO.Scheduled != DateTime.MinValue)//if the parel is assigned
            {
                DroneToList drone = DroneListBL.Find(i => i.DroneID == parcelDO.MyDroneID);
                drone.CopyPropertiesTo(parcelBO.MyDrone);
            }
            return parcelBO;
        }


        /// <summary>
        /// Displays the list of parcels
        /// </summary>
        /// <returns>The list of parceld</returns>
        public IEnumerable<ParcelToList> ListParcelDisplay()
        {
            List<IDAL.DO.Parcel> parcelsDO = new();
            try
            {
                parcelsDO = dal.ListParcelDisplay().ToList();
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            Parcel tmp = new();
            ParcelToList tmpParcelBO = new();
            List<ParcelToList> listParcelToList = new();
            List<IDAL.DO.Customer> CustomerListDL = dal.ListCustomerDisplay().ToList();//Receive the customer list from the data layer.
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                tmp = ParcelDisplay(currentParcel.ParcelID);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.Sender.Name; 
                tmpParcelBO.NameOfTargetaed = tmp.Targetid.Name;
                if (tmp.Scheduled == DateTime.MinValue)//not schedule yet
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Defined;
                else if (tmp.PickUp == DateTime.MinValue)//scheduled but has not been picked up
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Associated;
                else if (tmp.Delivered == DateTime.MinValue) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.PickedUp;
                else tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList;
        }


        /// <summary>
        /// Displays the list of unassigned parcels
        /// </summary>
        /// <returns>The list of unassigned parceld</returns>
        public IEnumerable<ParcelToList> ListOfUnassignedParcelDisplay()
        {
            List<IDAL.DO.Parcel> parcelsDO = new();
            try
            {
                parcelsDO = dal.ListParcelDisplay(i=>i.MyDroneID!=0).ToList();
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            Parcel tmp = new();
            ParcelToList tmpParcelBO = new();
            List<ParcelToList> listParcelToList = new();
            List<IDAL.DO.Customer> CustomerListDL = dal.ListCustomerDisplay().ToList();//Receive the customer list from the data layer.
            foreach (IDAL.DO.Parcel currentParcel in parcelsDO)
            {
                tmp = ParcelDisplay(currentParcel.ParcelID);
                tmp.CopyPropertiesTo(tmpParcelBO);
                tmpParcelBO.NameOfSender = tmp.Sender.Name;
                tmpParcelBO.NameOfTargetaed = tmp.Targetid.Name;
                if (tmp.Scheduled == DateTime.MinValue)//not schedule yet
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Defined;
                else if (tmp.PickUp == DateTime.MinValue)//scheduled but has not been picked up
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Associated;
                else if (tmp.Delivered == DateTime.MinValue) //scheduled and picked up  but has not been delivered
                    tmpParcelBO.ParcelStatus = @enum.ParcelStatus.PickedUp;
                else tmpParcelBO.ParcelStatus = @enum.ParcelStatus.Delivered;
                listParcelToList.Add(tmpParcelBO);
            }
            return listParcelToList;

        }
    }
}
