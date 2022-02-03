using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneInCharging GetDroneInCharge(int id, int stationId)
        {
            DroneInCharging droneInChargingBO = new();
            lock (dal)
            {
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
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneInCharging> GetDroneInChargingList(Predicate<DroneInCharging> predicate = null)
        {
            lock (dal)
            {
                List<DroneInCharging> droneChargeBO = new();
                droneChargeBO.AddRange(from DO.DroneCharge droneCharge in dal.GetListDroneCharge()
                                       select GetDroneInCharge(droneCharge.Id, droneCharge.BaseStationID));
                return droneChargeBO;
            }
        }
    }
}
