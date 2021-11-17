using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        IDal dal = new DalObject.DalObject();

        static internal readonly Random rand = new(DateTime.Now.Millisecond);

        #region ctor
        public BL()
        {

            double[] ElectricityUse = dal.ChargingDrone();//*צריך לבדוק מה הוא מעתיק

            #region Brings lists from IDAL

            List<DroneToList> DroneListBL = null;
            IEnumerable<IDAL.DO.Drone> DroneListDL = dal.ListDroneDisplay();//Receive the drone list from the data layer.
            DroneListBL.CopyPropertiesTo(DroneListDL);//convret from IDAT to IBL

            List<ParcelToList> ParcelListBL = null;
            IEnumerable<IDAL.DO.Parcel> ParcelListDL = dal.ListParcelDisplay();//Receive the parcel list from the data layer.
            ParcelListBL.CopyPropertiesTo(ParcelListDL);//convret from IDAT to IBL

            List<Customer> CustomerBL = null;
            IEnumerable<IDAL.DO.Customer> CustomerDL = dal.ListCustomerDisplay();//Receive the customer list from the data layer.
            CustomerBL.CopyPropertiesTo(CustomerDL);//convret from IDAT to IBL

            List<CustomerToList> CustomerListBL = null;
            IEnumerable<IDAL.DO.Customer> CustomerListDL = dal.ListCustomerDisplay();//Receive the customer list from the data layer.
            CustomerListBL.CopyPropertiesTo(CustomerListDL);//convret from IDAT to IBL

            List<BaseStation> BaseStationListBL = null;
            IEnumerable<IDAL.DO.Station> StationListDL = dal.ListStationDisplay();//Receive the drone list from the data layer.
            BaseStationListBL.CopyPropertiesTo(StationListDL);//convret from IDAT to IBL
            #endregion


            foreach (DroneToList currentDrone in DroneListBL)
            {
                ParcelToList parcelInDrone = ParcelListBL.Find(item => item.ParcelID == currentDrone.ParcelNumberTransfered);//finds the parcel which is assigned to the current drone.
                if (currentDrone.ParcelNumberTransfered != 0)//if the drone is assigned
                {
                    if (parcelInDrone.ParcelStatus != @enum.ParcelStatus.Delivered)//if the parcel is not provided
                    {
                        currentDrone.DroneStatus = @enum.DroneStatus.Delivery;
                        Customer sender = CustomerBL.Find(item => item.Name == parcelInDrone.NameOfSender);//found the customer that is getting the parcel
                        double minDistance = 0;
                        if (parcelInDrone.ParcelStatus != @enum.ParcelStatus.PickedUp)
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
                        Customer receiver = CustomerBL.Find(item => item.Name == parcelInDrone.NameOfTargetaed);//found the customer that is
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
                        minBatteryDrone += (int)minDistance * (int)ElectricityUse[0];//minimum battery the drone needs
                        currentDrone.Battery = rand.Next(minBatteryDrone, 100);
                    }
                }
            }

            foreach (DroneToList currentDrone in DroneListBL)
            {
                int index = rand.Next(0, BaseStationListBL.Capacity);//one of the staitions
                if (currentDrone.DroneStatus != @enum.DroneStatus.Delivery)//if the drone is not in delivery mode
                    currentDrone.DroneStatus = (@enum.DroneStatus)rand.Next(0, 1);

                if (currentDrone.DroneStatus == @enum.DroneStatus.Maintenance)//if the drone is not in maintenance mode
                {
                    currentDrone.MyCurrentLocation = BaseStationListBL[index].StationLocation;
                    currentDrone.Battery = rand.Next(0, 20);
                    break;
                }

                if (currentDrone.DroneStatus == @enum.DroneStatus.Available)//if the drone is available
                {
                    foreach (CustomerToList currentCustomer in CustomerListBL)
                    {
                        Customer customerDelivery = CustomerBL.Find(item => item.CustomerID == currentCustomer.CustomerID);//finds the current customer

                        if (currentCustomer.NumberParcelSentAndDelivered > 0)//the custumer had deliveries
                        {
                            currentDrone.MyCurrentLocation = customerDelivery.CustomerLocation;
                            break;
                        }
                    }
                    double minDistance = 0;
                    //finds the closest station from the targeted
                    foreach (BaseStation currentStation in BaseStationListBL)
                    {
                        if (DistanceCalculation(currentDrone.MyCurrentLocation, currentStation.StationLocation) < minDistance)
                        {
                            minDistance = DistanceCalculation(currentDrone.MyCurrentLocation, currentStation.StationLocation);
                        }
                    }
                    //the minimum battery the drones needs
                    int minBatteryDrone = 0;
                    int weight = (int)currentDrone.Weight;
                    switch (weight)
                    {
                        case (int)@enum.WeightCategories.Light:
                            minBatteryDrone = (int)minDistance * (int)ElectricityUse[1];
                            break;
                        case (int)@enum.WeightCategories.Midium:
                            minBatteryDrone = (int)minDistance * (int)ElectricityUse[2];
                            break;
                        case (int)@enum.WeightCategories.Heavy:
                            minBatteryDrone = (int)minDistance * (int)ElectricityUse[3];
                            break;
                    }
                    currentDrone.Battery = rand.Next(minBatteryDrone, 100);
                }
            }
        }
        #endregion

    }
}
