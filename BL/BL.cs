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
    internal partial class BL : IBL.IBL
    {
        static internal readonly Random rand = new(DateTime.Now.Millisecond);

        public BL()
        {
            IDal DalObj = new DalObject.DalObject();
            double[] ElectricityUse = DalObj.ChargingDrone();//*צריך לבדוק מה הוא מעתיק
            List<DroneToList> DroneListBL = null;
            List<IDAL.DO.Drone> DroneListDL = DalObj.ListDroneDisplay();//Receive the drone list from the data layer.
            DroneListBL.CopyPropertiesTo(DroneListDL);//convret from IDAT to IBL
            List<ParcelToList> ParcelListBL = null;
            List<IDAL.DO.Parcel> ParcelListDL = DalObj.ListParcelDisplay();//Receive the parcel list from the data layer.
            ParcelListBL.CopyPropertiesTo(ParcelListDL);//convret from IDAT to IBL
            foreach (DroneToList currentDrone in DroneListBL)
            {
                if (currentDrone.ParcelNumberTransfered != 0)
                    if (ParcelListBL.Find(item => item.ParcelID == currentDrone.ParcelNumberTransfered).Status != @enum.ParcelStatus.Delivered)
                    {
                        currentDrone.DroneStatus = @enum.DroneStatus.Delivery;
                        currentDrone.MyCurrentLocation;
                        List<Customer> CustomerListBL = null;
                        List<IDAL.DO.Customer> CustomerListDL = DalObj.ListCustomerDisplay();//Receive the drone list from the data layer.
                        CustomerListBL.CopyPropertiesTo(CustomerListDL);//convret from IDAT to IBL

                        CustomerListBL.Find(item=>item.Name==)
                    }
            }

            foreach (DroneToList currentDrone in DroneListBL)
            {
                if (currentDrone.DroneStatus != @enum.DroneStatus.Delivery)
                    currentDrone.DroneStatus = (@enum.DroneStatus)rand.Next(0, 1);
            }

        }


    }
}
