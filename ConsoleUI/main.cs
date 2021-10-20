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
                "\nTo find a distance between a point and a station or between a point and the customer, type 5." +
                "\nTo exit, type 6.");
            int UserAnswer = 0;
            string Input;
            int IDFromUser1, IDFromUser2;
            while (UserAnswer != 6)
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
                                    IDAL.DO.MainFuncAdd.AddDrone();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Station:
                                    IDAL.DO.MainFuncAdd.AddStation();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Parcel:
                                    IDAL.DO.MainFuncAdd.AddParcel();
                                    break;

                                case (int)IDAL.DO.@enum.AddOptions.Customer:
                                    IDAL.DO.MainFuncAdd.AddCustomer();
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
                            UserAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
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
                                        IDAL.DO.Drone droneWanted = IDAL.DO.DalObject.DroneDisplay(IDFromUser1);
                                        Console.WriteLine(droneWanted);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplyStation:
                                    {
                                        Console.WriteLine("Enter station ID (4 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.Station stationWanted = IDAL.DO.DalObject.StationDisplay(IDFromUser1);
                                        Console.WriteLine(stationWanted);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplayParcel:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.Parcel parcelWanted = IDAL.DO.DalObject.ParcelDisplay(IDFromUser1);
                                        Console.WriteLine(parcelWanted);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayIndividualOptions.DisplayCustomer:
                                    {
                                        Console.WriteLine("Enter customer ID (9 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        IDAL.DO.Customer custumerWanted = IDAL.DO.DalObject.CustomerDisplay(IDFromUser1);
                                        Console.WriteLine(custumerWanted);
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
                                    {
                                        IDAL.DO.Drone[] ListOfDrones = IDAL.DO.DalObject.ListDroneDisplay();
                                        for (int i = 0; i < ListOfDrones.Length; i++)
                                            Console.WriteLine(ListOfDrones[i]);
                                    }
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplyStationList:
                                    {
                                        IDAL.DO.Station[] ListOfStation = IDAL.DO.DalObject.ListStationDisplay();
                                        for (int i = 0; i < ListOfStation.Length; i++)
                                            Console.WriteLine(ListOfStation[i]);
                                    }
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplayParcelList:
                                    {
                                        IDAL.DO.Parcel[] ListOfParcel = IDAL.DO.DalObject.ListParcelDisplay();
                                        for (int i = 0; i < ListOfParcel.Length; i++)
                                            Console.WriteLine(ListOfParcel[i]);
                                    }
                                    break;

                                case (int)IDAL.DO.@enum.DisplayListOptions.DisplayCustomerList:
                                    {
                                        IDAL.DO.Customer[] ListOfCustomers = IDAL.DO.DalObject.ListCustomerDisplay();
                                        for (int i = 0; i < ListOfCustomers.Length; i++)
                                            Console.WriteLine(ListOfCustomers[i]);
                                    }
                                    break;
                                case (int)IDAL.DO.@enum.DisplayListOptions.ListOfUnassignedParcels:
                                    {
                                        IDAL.DO.Parcel[] ListOfParcel = IDAL.DO.DalObject.ListOfUnassignedParcels();
                                        for (int i = 0; i < ListOfParcel.Length; i++)
                                            Console.WriteLine(ListOfParcel[i]);
                                        break;
                                    }

                                case (int)IDAL.DO.@enum.DisplayListOptions.ListOfAvailableChargingStations:
                                    {
                                        IDAL.DO.Station[] ListOfStation = IDAL.DO.DalObject.ListOfAvailableChargingStations();
                                        for (int i = 0; i < ListOfStation.Length; i++)
                                            Console.WriteLine(ListOfStation[i]);
                                        break;
                                    }

                            }
                            UserAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                            break;
                        }
                    case (int)IDAL.DO.@enum.options.FindTheDistance:
                        {
                            double lat1, lon1;
                            int ID;
                            Console.WriteLine("Enter the latitude of the point from which you want to calculate distance");
                            Input = Console.ReadLine();
                            double.TryParse(Input, out lat1);
                            Console.WriteLine("Enter the longitude of the point from which you want to calculate distance");
                            Input = Console.ReadLine();
                            double.TryParse(Input, out lon1);
                            Console.WriteLine("Enter the ID number of the customer or station from which you would like to measure distance. (For a 9-digit customer and for a 6-digit station). ");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out ID);
                            Console.WriteLine(IDAL.DO.DistanceCalculation.Calculate(lat1, lon1, ID));
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
                "\nTo find a distance between a point and a station or between a point and the customer, type 5." +
                "\nTo exit, type 6.");
            }
        }
    }
}


