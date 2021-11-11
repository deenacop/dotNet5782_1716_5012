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

            List<Customer> CustomerListBL = null;
            List<IDAL.DO.Customer> CustomerListDL = DalObj.ListCustomerDisplay();//Receive the customer list from the data layer.
            CustomerListBL.CopyPropertiesTo(CustomerListDL);//convret from IDAT to IBL

            List<BaseStation> BaseStationListBL = null;
            List<IDAL.DO.Station> StationListDL = DalObj.ListStationDisplay();//Receive the drone list from the data layer.
            BaseStationListBL.CopyPropertiesTo(StationListDL);//convret from IDAT to IBL

            foreach (DroneToList currentDrone in DroneListBL)
            {
                ParcelToList parcelInDrone = ParcelListBL.Find(item => item.ParcelID == currentDrone.ParcelNumberTransfered);//finds the parcel which is assigned to the current drone.
                if (currentDrone.ParcelNumberTransfered != 0)//if the drone is assigned
                {
                    if (parcelInDrone.Status != @enum.ParcelStatus.Delivered)//if the parcel is not provided
                    {
                        currentDrone.DroneStatus = @enum.DroneStatus.Delivery;
                        Customer sender = CustomerListBL.Find(item => item.Name == parcelInDrone.NameCustomerSendidng);//found the customer that is getting the parcel
                        double minDistance = 0;
                        if (parcelInDrone.Status != @enum.ParcelStatus.PickedUp)
                        {
                            Location closestStation = null;
                            //finds the closest station from the sender
                            foreach (BaseStation currentStation in BaseStationListBL)
                            {
                                if (DistanceCalculation(sender.CustomerLocation, currentStation.StationLocation) < minDistance)
                                {
                                    minDistance = DistanceCalculation(sender.CustomerLocation, currentStation.StationLocation);
                                    closestStation = currentStation.StationLocation;
                                }
                            }
                            currentDrone.MyCurrentLocation = closestStation;
                        }
                        else
                        {
                            currentDrone.MyCurrentLocation = sender.CustomerLocation;
                        }
                        Customer receiver = CustomerListBL.Find(item => item.Name == parcelInDrone.NameCustomerReceiving);//found the customer that is
                        //finds the closest station from the targeted
                        foreach (BaseStation currentStation in BaseStationListBL)
                        {
                            if (DistanceCalculation(receiver.CustomerLocation, currentStation.StationLocation) < minDistance)
                            {
                                minDistance = DistanceCalculation(receiver.CustomerLocation, currentStation.StationLocation);
                            }
                        }
                        double distanceToTargeted = DistanceCalculation(receiver.CustomerLocation, currentDrone.MyCurrentLocation);
                        int minBatteryDrone = 0;
                        int weight = (int)currentDrone.Weight;
                        switch (weight)
                        {
                            case (int)@enum.WeightCategories.Light:
                                minBatteryDrone = (int)distanceToTargeted * (int)ElectricityUse[1];
                                break;
                            case (int)@enum.WeightCategories.Midium:
                                minBatteryDrone = (int)distanceToTargeted * (int)ElectricityUse[2];
                                break;
                            case (int)@enum.WeightCategories.Heavy:
                                minBatteryDrone = (int)distanceToTargeted * (int)ElectricityUse[3];
                                break;
                        }
                        minBatteryDrone += (int)minDistance * (int)ElectricityUse[0];
                        currentDrone.Battery = rand.Next(minBatteryDrone, 100);
                    }
                }
            }
            //if the drone is not in delivery mode
            foreach (DroneToList currentDrone in DroneListBL)
            {
                if (currentDrone.DroneStatus != @enum.DroneStatus.Delivery)
                    currentDrone.DroneStatus = (@enum.DroneStatus)rand.Next(0, 1);
            }

            //if the drone is not in maintenance mode
            foreach (DroneToList currentDrone in DroneListBL)
            {
                int index = rand.Next(0, BaseStationListBL.Capacity);
                if (currentDrone.DroneStatus == @enum.DroneStatus.Maintenance)
                {
                    if (BaseStationListBL[index].NumOfAvailableChargingSlots == 0)
                        index = rand.Next(0, BaseStationListBL.Capacity);
                    else
                    {
                        currentDrone.MyCurrentLocation = BaseStationListBL[index].StationLocation; 
                        break;
                    }
                }


            }
        }



    }
}
