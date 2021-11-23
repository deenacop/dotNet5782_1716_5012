using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace ConsoleUI_BL
{

    public class ProgramBL
    {
        static IBL.IBL bl = new BL.BL();
        /// <summary>
        /// Add , Update, DisplayIndividual, DisplayList, FindTheDistance,Exit
        /// </summary>
        public enum options { Add = 1, Update, DisplayIndividual, DisplayList, FindTheDistance, Exit };
        /// <summary>
        /// Which item to add
        /// </summary>
        public enum AddOptions { Drone = 1, Station, Parcel, Customer };
        /// <summary>
        ///  What kind of update is needed
        /// </summary>
        public enum UpdateOptions
        {
            UpdateDroneModel = 1, UpdateBaseStationDetails, UpdateCustomerDetails, SendDroneToCharge,
            ReleaseDroneFromeCharging, AssignParcelToDrone, CollectParcelByDrone, DeliverParcelByDrone
        };
        /// <summary>
        /// What item to print
        /// </summary>
        public enum DisplayIndividualOptions { DisplyDrone = 1, DisplyStation, DisplayParcel, DisplayCustomer };
        /// <summary>
        /// What list of items to print
        /// </summary>
        public enum DisplayListOptions
        {
            DisplyDroneList = 1, DisplyStationList, DisplayParcelList,
            DisplayCustomerList, ListOfUnassignedParcels, ListOfAvailableChargingStations
        };

        static public void Main(string[] args)
        {

            int userAnswer = 0;
            int IDFromUser1;
            while (userAnswer != 5)
            {
                try
                {
                    Console.WriteLine("To insert, type 1." +
                 "\nTo update, type 2." +
                 "\nTo view a single item, type 3." +
                 "\nTo view a list, type 4." +
                 "\nTo exit, type 5.");
                    int.TryParse(Console.ReadLine(), out userAnswer);
                    switch (userAnswer)
                    {
                        case (int)options.Add:
                            {
                                Console.WriteLine("To add a drone, type 1.\nTo add a Base station, type 2.\n" +
                                                  "To add a parcel, type 3.\nTo add a customer, type 4.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)AddOptions.Drone:
                                        Drone newDrone = new();
                                        Console.WriteLine("Enter the drone ID (3 digits)");
                                        int.TryParse(Console.ReadLine(), out int ID);
                                        newDrone.DroneID = ID;
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newDrone.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the ID station which in you want to charge the new drone (4 digits)");
                                        int chosenStation;
                                        int.TryParse(Console.ReadLine(), out chosenStation);
                                        bl.AddDrone(newDrone, chosenStation);
                                        break;

                                    case (int)AddOptions.Station:
                                        BaseStation newStation = new();
                                        Console.WriteLine("Enter the station ID (3 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newStation.StationID = ID;
                                        Console.WriteLine("Enter the station name");
                                        newStation.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude and latitude");
                                        double.TryParse(Console.ReadLine(), out double lon);
                                        newStation.StationLocation.Longitude = lon;
                                        double.TryParse(Console.ReadLine(), out double lat);
                                        newStation.StationLocation.Latitude = lat;
                                        Console.WriteLine("Enter the number of available slots");
                                        int.TryParse(Console.ReadLine(), out int num);
                                        newStation.NumOfAvailableChargingSlots = num;
                                        bl.AddBaseStation(newStation);
                                        break;

                                    case (int)AddOptions.Parcel:
                                        Parcel newParcel = new();
                                        Console.WriteLine("Enter the parcel sender ID (9 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newParcel.SenderCustomer.CustomerID = ID;
                                        Console.WriteLine("Enter the parcel targetid ID (9 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newParcel.TargetidCustomer.CustomerID = ID;
                                        Console.WriteLine("Choose the parcel Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newParcel.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the parcel priority");
                                        newParcel.Priority = (@enum.Priorities)Console.Read();
                                        bl.AddParcel(newParcel);
                                        break;

                                    case (int)AddOptions.Customer:
                                        Customer newCustomer = new();
                                        Console.WriteLine("Enter the customer ID (3 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newCustomer.CustomerID = ID;
                                        Console.WriteLine("Enter the customer name");
                                        newCustomer.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the customer phone number");
                                        newCustomer.PhoneNumber = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude and latitude");
                                        double.TryParse(Console.ReadLine(), out lon);
                                        newCustomer.CustomerLocation.Longitude = lon;
                                        double.TryParse(Console.ReadLine(), out lat);
                                        newCustomer.CustomerLocation.Latitude = lat;
                                        newCustomer.CustomerLocation.Latitude = Console.Read();
                                        bl.AddCustomer(newCustomer);
                                        break;
                                }
                                break;
                            }

                        case (int)options.Update:
                            {
                                Console.WriteLine("To update the drone model, type 1.\nTo update the base station details, type 2.\n" +
                                    "To update the customer details, type 3.\nTo send a drone to charge, type 4.\n" +
                                    "To release a drone frome the charging, type 5.\nTo assign parcel to drone, type6.\n" +
                                    "To collect a parcel by a drone, type 7.\nTo deliver parcel by a drone, type 8.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)UpdateOptions.UpdateDroneModel:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits) ");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            int droneID = ID;
                                            Console.WriteLine("Enter the wanted model");
                                            string model = Console.ReadLine();
                                            bl.UpdateDroneModel(droneID, model);
                                            break;
                                        }

                                    case (int)UpdateOptions.UpdateBaseStationDetails:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("if you want to change the name, enter the new name");
                                            string ansName = Console.ReadLine();
                                            if (ansName == "\n")
                                                ansName = null;
                                            Console.WriteLine("if you want to change the number of slots in the station, enter the new number of slots");
                                            if (!int.TryParse(Console.ReadLine(), out int ansNum))
                                                ansNum = 0;
                                            bl.UpdateStation(ID, ansName, ansNum == 0 ? ansNum : null);
                                            break;
                                        }

                                    case (int)UpdateOptions.UpdateCustomerDetails:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("if you want to change the name, enter the new name");
                                            string ansName = Console.ReadLine();
                                            if (ansName == "\n")
                                                ansName = null;
                                            Console.WriteLine("if you want to change the phone number of the customer, enter the new phone number");
                                            string ansPhone = Console.ReadLine();
                                            if (ansPhone == "\n")
                                                ansName = null;
                                            bl.UpdateCustomer(ID, ansName, ansPhone);
                                            break;
                                        }
                                    case (int)UpdateOptions.SendDroneToCharge:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            bl.SendDroneToCharge(ID);
                                            break;
                                        }

                                    case (int)UpdateOptions.ReleaseDroneFromeCharging:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits)");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("and the the amount of time(by minute) that the drone was in the chaging base station");
                                            int.TryParse(Console.ReadLine(), out int num);
                                            bl.ReleasingDroneFromBaseStation(ID, num);
                                            break;
                                        }
                                    case (int)UpdateOptions.AssignParcelToDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            bl.AssignParcelToDrone(ID);
                                            break;
                                        }
                                    case (int)UpdateOptions.CollectParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            bl.CollectionOfParcelByDrone(ID);
                                            break;
                                        }
                                    case (int)UpdateOptions.DeliverParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            bl.DeliveryParcelByDrone(ID);
                                            break;
                                        }
                                }
                                userAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                                break;
                            }

                        case (int)options.DisplayIndividual:
                            {
                                Console.WriteLine("To to print a drone, type 1.\nTo print a base station, type 2.\n" +
                                                 "To print a parcel, type 3.\nTo print a customer, type 4.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)DisplayIndividualOptions.DisplyDrone:
                                        {
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.DisplayDrone(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplyStation:
                                        {
                                            Console.WriteLine("Enter base station ID (4 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.BaseStationDisplay(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayParcel:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.ParcelDisplay(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayCustomer:
                                        {
                                            Console.WriteLine("Enter customer ID (9 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.CustomerDisplay(IDFromUser1));
                                            break;
                                        }
                                }
                                break;
                            }

                        case (int)options.DisplayList:
                            {
                                Console.WriteLine("To to print all drones, type 1.\nTo print all stations, type 2.\n" +
                                                "To print all parcels, type 3.\nTo print all customers, type 4.\n" +
                                                "To print all unassign parcels, type 5.\nTo print all available charge station, type 6.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)DisplayListOptions.DisplyDroneList:
                                        {
                                            IEnumerable<DroneToList> ListOfDrones = bl.ListDroneDisplay();
                                            foreach (DroneToList tmp in ListOfDrones) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplyStationList:
                                        {
                                            IEnumerable<BaseStationToList> ListOfStation = bl.ListBaseStationlDisplay();
                                            foreach (BaseStationToList tmp in ListOfStation) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayParcelList:
                                        {
                                            IEnumerable<ParcelToList> ListOfParcel = bl.ListParcelDisplay();
                                            foreach (ParcelToList tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayCustomerList:
                                        {
                                            IEnumerable<CustomerToList> ListOfCustomer = bl.ListCustomerDisplay();
                                            foreach (CustomerToList tmp in ListOfCustomer) { Console.WriteLine(tmp); }
                                        }
                                        break;
                                    case (int)DisplayListOptions.ListOfUnassignedParcels:
                                        {
                                            IEnumerable<ParcelToList> ListOfParcel = bl.ListOfUnassignedParcelDisplay();
                                            foreach (ParcelToList tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                    case (int)DisplayListOptions.ListOfAvailableChargingStations:
                                        {
                                            IEnumerable<BaseStationToList> ListOfStation = bl.ListOfAvailableSlotsBaseStationlDisplay();
                                            foreach (BaseStationToList tmp in ListOfStation) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                }
                                userAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                                break;
                            }
                        case (int)options.Exit:
                            Console.WriteLine("\nThank you for using our drones system, looking forward to see you again!");
                            break;

                        default:
                            Console.WriteLine("Wrong input");
                            break;
                    }
                }
                catch (ItemNotExistException ex)
                {
                    Console.WriteLine(ex);

                }
                catch (AlreadyExistedItemException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}



