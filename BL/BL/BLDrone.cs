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
        public void AddDrone(DroneToList drone, int stationID)
        {
            if (ChackingNumOfDigits(drone.DroneID) != 3)
                throw new WrongIDException("wrorng ID");
            if (drone.Weight < WeightCategories.Light || drone.Weight > WeightCategories.Heavy)
                throw new WrongInputException("Wronge input");
            try
            {
                IDAL.DO.Station wantedStation = dal.StationDisplay(stationID);
                drone.MyCurrentLocation = new() { Longitude = wantedStation.Longitude, Latitude = wantedStation.Latitude };
                drone.Battery = rand.Next(20, 41);
                drone.DroneStatus = DroneStatus.Maintenance;
                IDAL.DO.Drone droneDO = new();
                object obj = droneDO;
                drone.CopyPropertiesTo(obj);
                droneDO = (IDAL.DO.Drone)obj;
                dal.Add(droneDO);//calls the function from DALOBJECT
                DroneListBL.Add(drone);
            }
            catch (ItemNotExistException ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            catch (AlreadyExistedItemException ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }

        public void UpdateDroneModel(int ID, string model)
        {
            try
            {
                dal.UpdateDroneModel(ID, model);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
        }

        public void SendDroneToCharge(int ID)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);//finds the drone with the wanted ID
            if (index < 0 || DroneListBL[index].DroneStatus != DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone does not exist or is not available");

            List<BaseStation> BaseStationListBL = new();
            IEnumerable<IDAL.DO.Station> StationListDL = dal.ListStationDisplay(i => i.NumOfAvailableChargingSlots > 0).ToList();//Receive the drone list from the data layer.
            StationListDL.CopyPropertiesToIEnumerable(BaseStationListBL);//convret from IDAT to IBL
            int i = 0;
            foreach (BaseStation currentStation in BaseStationListBL)
            {
                currentStation.StationLocation = new() { Longitude = StationListDL.ElementAt(i).Longitude, Latitude = StationListDL.ElementAt(i).Latitude };
                i++;
            }
            if (BaseStationListBL.Capacity <= 0)
                throw new ItemNotExistException("There is no stations with available slots");
            //finds the closest station from the drone
            double distance = MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item2;
            if (distance * vacant > DroneListBL[index].Battery)
                throw new NotEnoughBatteryException("The drone cant go to a baseStation");
            try
            {
                dal.SendingDroneToChargingBaseStation(
                    ID, MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item3);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //if the drone *can* go to a charging station:
            DroneToList tmp = DroneListBL[index];
            tmp.Battery -= (int)(distance * vacant);
            tmp.DroneStatus = DroneStatus.Maintenance;
            tmp.MyCurrentLocation = MinDistanceLocation(BaseStationListBL, DroneListBL[index].MyCurrentLocation).Item1;
            DroneListBL[index] = tmp;
        }

        public void ReleasingDroneFromBaseStation(int ID, int minuteInCharge)
        {
            int index = DroneListBL.FindIndex(item => item.DroneID == ID);
            if (index < 0)//NOT FOUND
                throw new ItemNotExistException("The drone does not exist");
            if (DroneListBL[index].Battery + (int)(minuteInCharge * droneLoadingRate) <= 50)//the drone is half charged and can be used
                throw new NotEnoughBatteryException("The drone needs to be charged");
            try
            {//we brought the station that has the right location
                IDAL.DO.Station station = dal.ListStationDisplay(item => item.Latitude == DroneListBL[index].MyCurrentLocation.Latitude
                     && item.Longitude == DroneListBL[index].MyCurrentLocation.Longitude).First();
               //update
                dal.ReleasingDroneFromChargingBaseStation(ID, station.StationID);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //update the drone
            DroneToList tmp = DroneListBL[index];
            tmp.Battery += (int)(minuteInCharge * droneLoadingRate);
            if (tmp.Battery > 100)//cant be more than 100
                tmp.Battery = 100;
            tmp.DroneStatus = DroneStatus.Available;
            DroneListBL[index] = tmp;
        }

        public void AssignParcelToDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.DroneID == ID);//find index of the wanted drone
            Drone drone = DisplayDrone(ID);//drone from IDAL.DO
            if (drone.DroneStatus != DroneStatus.Available)
                throw new WorngStatusException("The drone is not available");
            //Brings the list of parcels sorted by order of urgency
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcelDisplay(i => i.MyDroneID == 0 && (int)i.Weight <= (int)drone.Weight).
                OrderByDescending(currentParcel => currentParcel.Priority).ThenByDescending(currentParcel =>
                 currentParcel.Weight).ThenByDescending(currentParcel =>
                     DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(currentParcel.Sender).CustomerLocation)).ToList();
            foreach (IDAL.DO.Parcel currentParcel in parcels)//Tests whether the drone can perform the shipping route (battery test)
            {
                if (BatteryCheckingForDroneAndParcel(currentParcel, drone))
                {
                    DroneListBL[index].DroneStatus = DroneStatus.Delivery;
                    dal.AssignParcelToDrone(currentParcel.ParcelID, drone.DroneID);
                    return;
                }
            }
            throw new ItemNotExistException("There is no parcel to assign with the drone");
        }

        public void CollectionOfParcelByDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.DroneID == ID);//find the index of the dron
            Drone drone = DisplayDrone(ID);//drone from IDAL.DO
            if (drone.DroneStatus != DroneStatus.Delivery)
                throw new WorngStatusException("The drone is not in delivery mode");
            Parcel parcel = ParcelDisplay(drone.MyParcel.ParcelID);//the wanted parcel
            if (parcel.PickUp != DateTime.MinValue)
                throw new WorngStatusException("The parcel has allready been picked up ");
            double distance = DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(parcel.SenderCustomer.CustomerID).CustomerLocation);
            DroneListBL[index].Battery -= (int)(distance * vacant);//update the battery
            DroneListBL[index].MyCurrentLocation = CustomerDisplay(parcel.SenderCustomer.CustomerID).CustomerLocation;
            dal.CollectionOfParcelByDrone(parcel.ParcelID, drone.DroneID);//send to the function
        }

        public void DeliveryParcelByDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.DroneID == ID);
            Drone drone = DisplayDrone(ID);
            Parcel parcel = ParcelDisplay(drone.MyParcel.ParcelID);
            if (drone.DroneStatus == DroneStatus.Delivery && parcel.PickUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
            {
                double distance = DistanceCalculation(drone.MyCurrentLocation, CustomerDisplay(parcel.TargetidCustomer.CustomerID).CustomerLocation);
                switch ((int)drone.Weight)
                {
                    case (int)WeightCategories.Light:
                        DroneListBL[index].Battery -= (int)(distance * carriesLightWeight);
                        break;
                    case (int)WeightCategories.Midium:
                        DroneListBL[index].Battery -= (int)(distance * carriesMediumWeight);
                        break;
                    case (int)WeightCategories.Heavy:
                        DroneListBL[index].Battery -= (int)(distance * carriesHeavyWeight);
                        break;
                }
                DroneListBL[index].MyCurrentLocation = CustomerDisplay(parcel.TargetidCustomer.CustomerID).CustomerLocation;
                dal.DeliveryParcelToCustomer(parcel.ParcelID);
            }
            else throw new WorngStatusException("The parcel couldnt be delivered");
        }

        public Drone DisplayDrone(int droneID)
        {
            Drone droneBO = new();
            try
            {
                IDAL.DO.Drone droneDO = dal.DroneDisplay(droneID);
                droneDO.CopyPropertiesTo(droneBO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //if (!DroneListBL.Exists(item => item.DroneID == droneID))
            //    throw new ItemNotExistException("Drone not found");
            DroneToList tmp = DroneListBL.Find(item => item.DroneID == droneID);
            droneBO.MyParcel = new();
            droneBO.DroneStatus = tmp.DroneStatus;
            droneBO.Battery = tmp.Battery;
            droneBO.MyCurrentLocation = tmp.MyCurrentLocation;

            if (droneBO.DroneStatus == DroneStatus.Delivery)//if not in a delivery mode= doesnt have any parcel
            {
                IDAL.DO.Parcel parcel = dal.ParcelDisplay(droneID);
                droneBO.MyParcel.SenderCustomer = new();
                droneBO.MyParcel.ReceiverCustomer = new();
                parcel.CopyPropertiesTo(droneBO.MyParcel);

                if (parcel.PickUp == DateTime.MinValue)//is not already picked
                    droneBO.MyParcel.Status = false;//waiting
                else droneBO.MyParcel.Status = true;//on the way

                IDAL.DO.Customer senderCustomer = dal.CustomerDisplay(parcel.Sender);
                IDAL.DO.Customer receiverCustomer = dal.CustomerDisplay(parcel.Targetid);
                senderCustomer.CopyPropertiesTo(droneBO.MyParcel.SenderCustomer);
                receiverCustomer.CopyPropertiesTo(droneBO.MyParcel.ReceiverCustomer);

                droneBO.MyParcel.Collection = new() { Longitude = senderCustomer.Longitude, Latitude = senderCustomer.Latitude };
                droneBO.MyParcel.Delivery = new() { Longitude = receiverCustomer.Longitude, Latitude = receiverCustomer.Latitude };
                droneBO.MyParcel.TransportDistance = DistanceCalculation(droneBO.MyParcel.Collection, droneBO.MyParcel.Delivery);
            }
            return droneBO;
        }

        public IEnumerable<DroneToList> ListDroneDisplay()
        {
            return DroneListBL;
        }
    }
}