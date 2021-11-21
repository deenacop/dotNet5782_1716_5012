using System;
using IBL.BO;
using BL;

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
            AssignParcelToDrone = 1, CollectParcelByDrone, DelivereParcelToCustomer,
            SendDroneToChargingBaseStation, ReleaseDroneFromChargingBaseStation
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
            int IDFromUser1, IDFromUser2;
            while (userAnswer != 6)
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
                                        newDrone.DroneID = Console.Read();
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newDrone.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the ID station which in you whant to charge the new drone (4 digits)");
                                        int chosenStation = Console.Read();
                                        bl.Add
                                        break;

                                    case (int)AddOptions.Station:
                                        BaseStation newStation = new();
                                        Console.WriteLine("Enter the station ID (3 digits)");
                                        newStation.StationID = Console.Read();
                                        Console.WriteLine("Enter the station name");
                                        newStation.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude and latitude");
                                        newStation.StationLocation.Longitude = Console.Read();
                                        newStation.StationLocation.Latitude = Console.Read();
                                        Console.WriteLine("Enter the number of available slots");
                                        newStation.NumOfAvailableChargingSlots = Console.Read();
                                        bl.Add
                                        break;

                                    case (int)AddOptions.Parcel:
                                        Parcel newParcel = new();
                                        Console.WriteLine("Enter the parcel sender ID (3 digits)");
                                        newParcel.Sender.CustomerID = Console.Read();
                                        Console.WriteLine("Enter the parcel targetid ID (3 digits)");
                                        newParcel.Targetid.CustomerID = Console.Read();
                                        Console.WriteLine("Choose the parcel Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newParcel.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the parcel priority");
                                        newParcel.Priority = (@enum.Priorities)Console.Read();
                                        bl.Add
                                        break;

                                    case (int)AddOptions.Customer:
                                        Customer newCustomer = new();
                                        Console.WriteLine("Enter the customer ID (3 digits)");
                                        newCustomer.CustomerID = Console.Read();
                                        Console.WriteLine("Enter the customer name");
                                        newCustomer.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the customer phone number");
                                        newCustomer.PhoneNumber = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude and latitude");
                                        newCustomer.CustomerLocation.Longitude = Console.Read();
                                        newCustomer.CustomerLocation.Latitude = Console.Read();
                                        bl.Add
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
                                    case (int)UpdateOptions.AssignParcelToDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            bl.??? (Console.ReadLine());
                                            break;
                                        }

                                    case (int)UpdateOptions.CollectParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            bl.??? (Console.ReadLine());
                                            break;
                                        }

                                    case (int)UpdateOptions.DelivereParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            bl.??? (Console.ReadLine());
                                            break;
                                        }
                                    case (int)UpdateOptions.SendDroneToChargingBaseStation:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            bl.??? (Console.ReadLine());
                                            break;
                                        }

                                    case (int)UpdateOptions.ReleaseDroneFromChargingBaseStation:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int id = Console.Read();
                                            Console.WriteLine("Enter the amount of time(by minute) that the drone was in the chaging base station");
                                            int numMinute = Console.Read();
                                            bl.???
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
                                            bl.??
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplyStation:
                                        {
                                            Console.WriteLine("Enter base station ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            bl.??
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayParcel:
                                        {
                                            Console.WriteLine("Enter parcel ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            bl.??
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayCustomer:
                                        {
                                            Console.WriteLine("Enter customer ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            bl.??
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
                                input = Console.ReadLine();
                                int.TryParse(input, out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)DisplayListOptions.DisplyDroneList:
                                        {
                                            List<Drone> ListOfDrones = bl.ListDroneDisplay();
                                            foreach (Drone tmp in ListOfDrones) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplyStationList:
                                        {
                                            List<Station> ListOfStation = bl.ListStationDisplay();
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayParcelList:
                                        {
                                            List<Parcel> ListOfParcel = bl.ListParcelDisplay();
                                            foreach (Parcel tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayCustomerList:
                                        {
                                            List<Customer> ListOfCustomer = bl.ListCustomerDisplay();
                                            foreach (Customer tmp in ListOfCustomer) { Console.WriteLine(tmp); }
                                        }
                                        break;
                                    case (int)DisplayListOptions.ListOfUnassignedParcels:
                                        {
                                            List<Parcel> ListOfParcel = bl.ListParcelDisplay();
                                            foreach (Parcel tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                    case (int)DisplayListOptions.ListOfAvailableChargingStations:
                                        {
                                            List<Station> ListOfStation = bl.ListStationDisplay();
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
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
