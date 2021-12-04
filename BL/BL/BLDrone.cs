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
        public void AddDrone(Drone drone, int stationID)
        {
            if (CheckNumOfDigits(drone.Id) != 3)
                throw new WrongInputException("Bad Drone ID");
            if (drone.Model == null || drone.Model == "")
                throw new WrongInputException("Missing drone model");
            try
            {
                DroneToList listDrone = new();
                IDAL.DO.Station wantedStation = dal.GetStation(stationID);
                drone.CopyPropertiesTo(listDrone);
                listDrone.Location = new() { Longitude = wantedStation.Longitude, Latitude = wantedStation.Latitude };
                listDrone.Battery = rand.Next(20, 41);
                listDrone.Status = DroneStatus.Maintenance;
                object obj = new IDAL.DO.Drone();
                drone.CopyPropertiesTo(obj);
                dal.Add((IDAL.DO.Drone)obj); //calls the function from DALOBJECT
                DroneListBL.Add(listDrone);
            }
            catch (ItemNotExistException ex)
            {
                throw new ItemNotExistException("Station does not exist", ex);
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw new ItemAlreadyExistsException("Trying to add an existing drone", ex);
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

        public void UpdateDrone(Drone drone)
        {
            if (drone.Model == null || drone.Model == "")
                throw new WrongInputException("Missing drone model");
            
            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            drone.CopyPropertiesTo(listDrone);

            object obj = new IDAL.DO.Drone();
            drone.CopyPropertiesTo(obj);
            try
            {
                dal.UpdateDrone((IDAL.DO.Drone)obj);//calls the function from DALOBJECT
            }
            catch (IDAL.DO.ItemNotExistException ex)
            {
                throw new ItemNotExistException("Drone does not exist", ex);
            }
        }


        public void SendDroneToCharge(Drone drone)
        {
            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            if (listDrone.Status != DroneStatus.Available)//if the drone does not exist or the drone is not available
                throw new ItemNotExistException("Drone is not available");
            //drone.CopyPropertiesTo(listDrone);
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
            double distance = MinDistanceLocation(BaseStationListBL, listDrone.Location).Item2;
            if (distance * vacant > listDrone.Battery)
                throw new NotEnoughBatteryException("The drone cant go to a baseStation");
            try
            {
                dal.SendingDroneToChargingBaseStation(
                    listDrone.Id, MinDistanceLocation(BaseStationListBL, listDrone.Location).Item3);//calls the function from DALOBJECT
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //if the drone *can* go to a chargin Drog station:
            listDrone.Battery -= (int)(distance * vacant);
            listDrone.Status = DroneStatus.Maintenance;
            listDrone.Location = MinDistanceLocation(BaseStationListBL, listDrone.Location).Item1;
            int index = DroneListBL.FindIndex(item => item.Id == listDrone.Id);//finds the drone with the wanted ID
            DroneListBL[index] = listDrone;
        }

        public void ReleasingDroneFromBaseStation(Drone drone, int minuteInCharge)
        {
            DroneToList listDrone = DroneListBL.FirstOrDefault(d => d.Id == drone.Id);
            if (listDrone == null)
                throw new ItemNotExistException("Drone does not exist");
            if (listDrone.Battery + (int)(minuteInCharge * droneLoadingRate) <= 50)//the drone is half charged and can be used
                throw new NotEnoughBatteryException("The drone needs to be charged");
            try
            {//we brought the station that has the right location
                IDAL.DO.Station station = dal.ListStationDisplay(item => item.Latitude == listDrone.Location.Latitude
                     && item.Longitude == listDrone.Location.Longitude).First();
               //update
                dal.ReleasingDroneFromChargingBaseStation(listDrone.Id, station.StationID);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            //update the drone

            listDrone.Battery += (int)(minuteInCharge * droneLoadingRate);
            if (listDrone.Battery > 100)//cant be more than 100
                listDrone.Battery = 100;
            listDrone.Status = DroneStatus.Available;
            int index = DroneListBL.FindIndex(item => item.Id == listDrone.Id);
            DroneListBL[index] = listDrone;
        }

        public void AssignParcelToDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.Id == ID);//find index of the wanted drone
            Drone drone = GetDrone(ID);//drone from IDAL.DO
            if (drone.Status != DroneStatus.Available)
                throw new WorngStatusException("The drone is not available");
            //Brings the list of parcels sorted by order of urgency
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcelDisplay(i => i.MyDroneID == 0 && (int)i.Weight <= (int)drone.Weight).
                OrderByDescending(currentParcel => currentParcel.Priority).ThenByDescending(currentParcel =>
                 currentParcel.Weight).ThenByDescending(currentParcel =>
                     DistanceCalculation(drone.Location, GetCustomer(currentParcel.Sender).CustomerLocation)).ToList();
            foreach (IDAL.DO.Parcel currentParcel in parcels)//Tests whether the drone can perform the shipping route (battery test)
            {
                if (BatteryCheckingForDroneAndParcel(currentParcel, drone))
                {
                    DroneListBL[index].Status = DroneStatus.Delivery;
                    dal.AssignParcelToDrone(currentParcel.ParcelID, drone.Id);
                    return;
                }
            }
            throw new ItemNotExistException("There is no parcel to assign with the drone");
        }

        public void CollectionOfParcelByDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.Id == ID);//find the index of the dron
            Drone drone = GetDrone(ID);//drone from IDAL.DO
            if (drone.Status != DroneStatus.Delivery)
                throw new WorngStatusException("The drone is not in delivery mode");
            Parcel parcel = GetParcel(drone.Parcel.ParcelID);//the wanted parcel
            if (parcel.PickUp != null)
                throw new WorngStatusException("The parcel has allready been picked up ");
            double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.SenderCustomer.CustomerID).CustomerLocation);
            DroneListBL[index].Battery -= (int)(distance * vacant);//update the battery
            DroneListBL[index].Location = GetCustomer(parcel.SenderCustomer.CustomerID).CustomerLocation;
            dal.CollectionOfParcelByDrone(parcel.ParcelID, drone.Id);//send to the function
        }

        public void DeliveryParcelByDrone(int ID)
        {
            int index = DroneListBL.FindIndex(i => i.Id == ID);
            Drone drone = GetDrone(ID);
            Parcel parcel = GetParcel(drone.Parcel.ParcelID);
            if (drone.Status == DroneStatus.Delivery && parcel.PickUp != null && parcel.Delivered == null)
            {
                double distance = DistanceCalculation(drone.Location, GetCustomer(parcel.TargetidCustomer.CustomerID).CustomerLocation);
                switch ((int)drone.Weight)
                {
                    case (int)WeightCategories.Light:
                        DroneListBL[index].Battery -= (int)(distance * carriesLightWeight);
                        break;
                    case (int)WeightCategories.Medium:
                        DroneListBL[index].Battery -= (int)(distance * carriesMediumWeight);
                        break;
                    case (int)WeightCategories.Heavy:
                        DroneListBL[index].Battery -= (int)(distance * carriesHeavyWeight);
                        break;
                }
                DroneListBL[index].Location = GetCustomer(parcel.TargetidCustomer.CustomerID).CustomerLocation;
                DroneListBL[index].Status = DroneStatus.Available;
                dal.DeliveryParcelToCustomer(parcel.ParcelID);
            }
            else throw new WorngStatusException("The parcel couldnt be delivered");
        }

        public Drone GetDrone(int id)
        {
            Drone droneBO = new();
            try
            {
                IDAL.DO.Drone droneDO = dal.DroneDisplay(id);
                droneDO.CopyPropertiesTo(droneBO);
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }
            DroneToList tmp = DroneListBL.Find(item => item.Id == id);
            droneBO.Parcel = new();
            droneBO.Status = tmp.Status;
            droneBO.Battery = tmp.Battery;
            droneBO.Location = tmp.Location;

            if (droneBO.Status == DroneStatus.Delivery)//if not in a delivery mode= doesnt have any parcel
            {
                IDAL.DO.Parcel parcel = dal.ListParcelDisplay(i=>i.MyDroneID==id).First();
                droneBO.Parcel.SenderCustomer = new();
                droneBO.Parcel.ReceiverCustomer = new();
                parcel.CopyPropertiesTo(droneBO.Parcel);

                if (parcel.PickUp == null)//is not already picked
                    droneBO.Parcel.Status = false;//waiting
                else droneBO.Parcel.Status = true;//on the way

                IDAL.DO.Customer senderCustomer = dal.CustomerDisplay(parcel.Sender);
                IDAL.DO.Customer receiverCustomer = dal.CustomerDisplay(parcel.Targetid);
                senderCustomer.CopyPropertiesTo(droneBO.Parcel.SenderCustomer);
                receiverCustomer.CopyPropertiesTo(droneBO.Parcel.ReceiverCustomer);

                droneBO.Parcel.Collection = new() { Longitude = senderCustomer.Longitude, Latitude = senderCustomer.Latitude };
                droneBO.Parcel.Delivery = new() { Longitude = receiverCustomer.Longitude, Latitude = receiverCustomer.Latitude };
                droneBO.Parcel.TransportDistance = DistanceCalculation(droneBO.Parcel.Collection, droneBO.Parcel.Delivery);
            }
            return droneBO;
        }

        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DroneListBL.FindAll(i => predicate == null ? true : predicate(i));
        }
    }
}