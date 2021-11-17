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
            if (ChackingNumOfDigits(station.StationID) == 4)
                throw new Exception("Worng ID");
            if (station.StationLocation.Latitude < 31 || station.StationLocation.Latitude > 32
                || station.StationLocation.Longitude < 35 || station.StationLocation.Longitude > 36)
                throw new Exception("The location is not logical");
            if (station.NumOfAvailableChargingSlots < 0)
                throw new Exception("The number of slots cant be negative");
            try
            {
                IDAL.DO.Station tmpStation = new();
                station.CopyPropertiesTo(tmpStation);
                dal.Add(tmpStation);
            }
            catch(IDAL.DO.AlreadyExistedItemException ex)
            {
                throw new Exception(ex.Message);
            }

        }






    }
}