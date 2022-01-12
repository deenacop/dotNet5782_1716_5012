using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        public DroneInCharging GetDroneInCharge(int id, int stationId)
        {
            DroneInCharging droneInChargingBO = new();
            try
            {
                DO.DroneCharge droneCharge = dal.GetDroneCharge(id, stationId);
                droneCharge.CopyPropertiesTo(droneInChargingBO);
                droneInChargingBO.Battery = DronesBL.Find(i => i.Id == id).Battery;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            return droneInChargingBO;
        }

        public IEnumerable<DroneInCharging> GetDroneInChargingList(Predicate<DroneInCharging> predicate = null)
        {
            List<DroneInCharging> droneChargeBO = new();
            droneChargeBO.AddRange (from DO.DroneCharge droneCharge in dal.GetListDroneCharge()
                                   select GetDroneInCharge(droneCharge.Id, droneCharge.BaseStationID));
            return droneChargeBO;
        }
    }
}
