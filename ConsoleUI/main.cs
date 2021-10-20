using System;

namespace ConsoleUI
{
    class main
    {
        static void Main(string[] args)
        {
            IDAL.DO.DalObject boot = new IDAL.DO.DalObject();
            Console.WriteLine("Welcome to our drone delivery system.\n" +
                "To insert, type 1." +
                "\nTo update, type 2." +
                "\nTo view a single item, type 3." +
                "\nTo view a list, type 4." +
                "\nTo exit, type 5.");
            int UserAnswer = 0;
            string Input;
            int IDFromUser1, IDFromUser2;
            while (UserAnswer != 5)
            {
                Input = Console.ReadLine();
                int.TryParse(Input, out UserAnswer);
                switch (UserAnswer)
                {
                    case (int)IDAL.DO.@enum.options.Add:
                        {
                            Console.WriteLine("To add a drone, type 1.\nTo add a station, type 2.\n" +
                                              "To add a parcel, type 3.\nTo add a customer, type 4.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {
                                case (int)IDAL.DO.@enum.AddOptions.Drone:
                                    IDAL.DO.mainFuncAdd.AddDrone();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Station:
                                    IDAL.DO.mainFuncAdd.AddStation();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Parcel:
                                    IDAL.DO.mainFuncAdd.AddParcel();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Customer:
                                    IDAL.DO.mainFuncAdd.AddCustomer();
                                    break;
                            }
                            break;
                        }
                       
                    case (int)IDAL.DO.@enum.options.Update:
                        {
                            Console.WriteLine("To assign pacel to drone, type 1.\nTo collect parcel by a drone, type 2.\n" +
                                          "To deliver parcel to a customer, type 3.\nTo send drone to a charging base station, type 4.\n" +
                                          "To release drone from charging station, type 5.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {

                                case (int)IDAL.DO.@enum.UpdateOptions.AssignParcelToDrone:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.AssignParcelToDrone(IDFromUser1);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.UpdateOptions.CollectParcelByDrone:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.CollectionOfParcelByDrone(IDFromUser1);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.UpdateOptions.DelivereParcelToCustomer:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.DeliveryParcelToCustomer(IDFromUser1);
                                        break;
                                    }
                                case (int)IDAL.DO.@enum.UpdateOptions.SendDroneToChargingBaseStation:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Console.WriteLine("Choose from the following available stations. Enter the chosen ID (4 digits).");
                                        IDAL.DO.DalObject.ListOfAvailableChargingStations();
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser2);
                                        IDAL.DO.DalObject.SendingDroneToChargingBaseStation(IDFromUser1, IDFromUser2);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.UpdateOptions.ReleaseDroneFromChargingBaseStation:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Console.WriteLine("Enter station ID (4 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser2);
                                        IDAL.DO.DalObject.ReleasingDroneFromChargingBaseStation(IDFromUser1, IDFromUser2);
                                        break;
                                    }

                            }
                            UserAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                            break;
                        }



                    case (int)IDAL.DO.@enum.options.DisplayIndividual:
                        {
                            Console.WriteLine("To to print a drone, type 1.\nTo print a station, type 2.\n" +
                                             "To print a parcel, type 3.\nTo print a customer, type 4.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplyDrone:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.DroneDisplay(IDFromUser1);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplyStation:
                                    {
                                        Console.WriteLine("Enter station ID (4 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.StationDisplay(IDFromUser1);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplayParcel:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.ParcelDisplay(IDFromUser1);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplayCustomer:
                                    {
                                        Console.WriteLine("Enter customer ID (9 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.DalObject.CustomerDisplay(IDFromUser1);
                                        break;
                                    }
                            }
                            break;
                        }



                    case (int)IDAL.DO.@enum.options.DisplayList:
                        {
                            Console.WriteLine("To to print all drones, type 1.\nTo print all stations, type 2.\n" +
                                            "To print all parcels, type 3.\nTo print all customers, type 4.\n" +
                                            "To print all unassign parcels, type 5.\nTo print all available charge station, type 6.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {
                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplyDroneList:

                                    IDAL.DO.DalObject.ListDroneDisplay();
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplyStationList:
                                    //Console.WriteLine((IDAL.DO.DalObject.ListOfStation[0].ToString()));

                                    IDAL.DO.DalObject.ListStationDisplay();
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplayParcelList:
                                    IDAL.DO.DalObject.ListParcelDisplay();
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplayCustomerList:
                                    IDAL.DO.DalObject.ListCustomerDisplay();
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.ListOfUnassignedParcels:
                                    IDAL.DO.DalObject.ListOfUnassignedParcels();
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.ListOfAvailableChargingStations:
                                    IDAL.DO.DalObject.ListOfAvailableChargingStations();
                                    break;
                            }
                            UserAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                            break;
                        }



                    case (int)IDAL.DO.@enum.options.Exit:
                        Console.WriteLine("\nThank you for using our drones system, looking forward to see you again!");
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;

                }

                Console.WriteLine("To insert, type 1." +
                "\nTo update, type 2." +
                "\nTo view a single item, type 3." +
                "\nTo view a list, type 4." +
                "\nTo exit, type 5.");
            }
        }
    }
}


