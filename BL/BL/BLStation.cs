using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using BL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        /// <summary>
        /// Adds a station to the list of stations in the IDAL
        /// </summary>
        /// <param name="station">The station that is being </param>
        public void AddBaseStation(BaseStation station)
        {
            if (ChackingNumOfDigits(station.StationID) != 4)
                throw new WrongIDException("Wrong ID");
            if (station.StationLocation.Latitude < 31 || station.StationLocation.Latitude > 32
                || station.StationLocation.Longitude < 35 || station.StationLocation.Longitude > 36)
                throw new UnlogicalLocation("the location is not logical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new ArgumentOutOfRangeException("cant be negative");
            station.DronesInCharging.Clear();
            try
            {
                IDAL.DO.Station tmpStation = new();
                station.CopyPropertiesTo(tmpStation);
                dal.Add(tmpStation);
            }
            catch(Exception ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }


        ///// <summary>
        ///// Return the wanted station
        ///// </summary>
        ///// <param name="StationID">The requested station</param>
        //public BaseStation StationDisplay(int StationID)
        //{
        //    BaseStation baseStation;
        //    baseStation =FindBaseStation(StationID);
        //    for(IDAL.DO.DroneCharge droneCharge; dal.)
        //}

        public void UpdateStation(int ID, string name=null,int? numOfSlots=null)
        {
            try
            {
                dal.UpdateStation(ID, name, numOfSlots);
            }
            catch(Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }






    }
}