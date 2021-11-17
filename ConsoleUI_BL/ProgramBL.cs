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
                 "\nTo find a distance between a point and a station or between a point and the customer, type 5." +
                 "\nTo exit, type 6.");
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
                                        Drone newDrone = new Drone();
                                        Console.WriteLine("Enter the drone ID (3 digits)");
                                        newDrone.DroneID = Console.Read();
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newDrone.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the ID station which in you whant to charge the new drone (4 digits)");
                                        int chosenStation =Console.Read();
                                        break;

                                    case (int)AddOptions.Station:
                                        BaseStation newStation = new BaseStation();
                                        Console.WriteLine("Enter the station ID (3 digits)");
                                        newStation.StationID = Console.Read();
                                        Console.WriteLine("Enter the station name");
                                        newStation.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude and latitude");
                                        newStation.StationLocation.Longitude = Console.Read();
                                        newStation.StationLocation.Latitude= Console.Read();
                                        Console.WriteLine("Enter the number of available slots");
                                        newStation.NumOfAvailableChargingSlots= Console.Read();
                                        break;

                                    case (int)AddOptions.Parcel:
                                        Drone newDrone = new Drone();
                                        Console.WriteLine("Enter the drone ID (3 digits)");
                                        newDrone.DroneID = Console.Read();
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newDrone.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the ID station which in you whant to charge the new drone (4 digits)");
                                        break;

                                    case (int)AddOptions.Customer:
                                        Drone newDrone = new Drone();
                                        Console.WriteLine("Enter the drone ID (3 digits)");
                                        newDrone.DroneID = Console.Read();
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        newDrone.Weight = (@enum.WeightCategories)Console.Read();
                                        Console.WriteLine("Enter the ID station which in you whant to charge the new drone (4 digits)");
                                        break;
                                }
                                break;
                            }

                        case (int)options.Update:
                            {
                                Console.WriteLine("To assign pacel to drone, type 1.\nTo collect parcel by a drone, type 2.\n" +
                                              "To deliver parcel to a customer, type 3.\nTo send drone to a charging base station, type 4.\n" +
                                              "To release drone from charging station, type 5.");
                                input = Console.ReadLine();
                                int.TryParse(input, out userAnswer);
                                switch (userAnswer)
                                {

                                    case (int)UpdateOptions.AssignParcelToDrone:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser2);
                                            bl.AssignParcelToDrone(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                    case (int)UpdateOptions.CollectParcelByDrone:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser2);
                                            bl.CollectionOfParcelByDrone(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                    case (int)UpdateOptions.DelivereParcelToCustomer:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            bl.DeliveryParcelToCustomer(IDFromUser1);
                                            break;
                                        }
                                    case (int)UpdateOptions.SendDroneToChargingBaseStation:
                                        {
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Console.WriteLine("Choose from the following available stations. Enter the chosen ID (4 digits).");
                                            List<Station> ListOfStation = bl.ListOfAvailableChargingStations();
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser2);
                                            bl.SendingDroneToChargingBaseStation(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                    case (int)UpdateOptions.ReleaseDroneFromChargingBaseStation:
                                        {
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Console.WriteLine("Enter station ID (4 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser2);
                                            bl.ReleasingDroneFromChargingBaseStation(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                }
                                userAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                                break;
                            }

                        case (int)options.DisplayIndividual:
                            {
                                Console.WriteLine("To to print a drone, type 1.\nTo print a station, type 2.\n" +
                                                 "To print a parcel, type 3.\nTo print a customer, type 4.");
                                input = Console.ReadLine();
                                int.TryParse(input, out userAnswer);
                                switch (userAnswer)
                                {

                                    case (int)DisplayIndividualOptions.DisplyDrone:
                                        {
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Drone droneWanted = bl.DroneDisplay(IDFromUser1);
                                            Console.WriteLine(droneWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplyStation:
                                        {
                                            Console.WriteLine("Enter station ID (4 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Station stationWanted = bl.StationDisplay(IDFromUser1);
                                            Console.WriteLine(stationWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayParcel:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Parcel parcelWanted = bl.ParcelDisplay(IDFromUser1);
                                            Console.WriteLine(parcelWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayCustomer:
                                        {
                                            Console.WriteLine("Enter customer ID (9 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Customer custumerWanted = bl.CustomerDisplay(IDFromUser1);
                                            Console.WriteLine(custumerWanted);
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
                                userAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                                break;
                            }
                        case (int)options.FindTheDistance:
                            {
                                double lat1, lon1;
                                int ID, answer;
                                Console.WriteLine("Enter the latitude of the point from which you want to calculate distance");
                                input = Console.ReadLine();
                                double.TryParse(input, out lat1);
                                Console.WriteLine("Enter the longitude of the point from which you want to calculate distance");
                                input = Console.ReadLine();
                                double.TryParse(input, out lon1);
                                Console.WriteLine("To find the distance from the point you entered to a station, type 1\n" +
                                    "To find the distance from the point you entered to a customer type 0");
                                input = Console.ReadLine();
                                int.TryParse(input, out answer);
                                if (answer == 1)//station
                                {
                                    Console.WriteLine("Enter the ID number of the station from which you would like to measure distance ( 4-digit). ");
                                    input = Console.ReadLine();
                                    int.TryParse(input, out ID);
                                    Console.WriteLine(DistanceCalculation.Calculation(lon1, lat1, DistanceCalculation.FindTheStationCoordinates(ID)));
                                }
                                if (answer == 0)//customer
                                {
                                    Console.WriteLine("Enter the ID number of the customer from which you would like to measure distance (9-digit). ");
                                    input = Console.ReadLine();
                                    int.TryParse(input, out ID);
                                    Console.WriteLine(DistanceCalculation.Calculation(lon1, lat1, DistanceCalculation.FindTheCustomerCoordinates(ID)));

                                }
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
