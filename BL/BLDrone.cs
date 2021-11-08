using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace BL
{
    internal partial class BL : IBL
    {
        public BL()
        {
            IDal DalObj = new DalObject.DalObject();
            double[] ElectricityUse = DalObj.ChargingDrone();//*צריך לבדוק מה הוא מעתיק
            List <Drone> DroneList= DalObj.ListDroneDisplay();
        }
        
        
    }
}
