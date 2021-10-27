using System;
using IDAL.DO;
namespace ConsoleUI
{
    class main
    {
        static void Main(string[] args)
        {
            DalObject boot = new DalObject();
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
                    case (int)@enum.options.Add:
                        {
                            Console.WriteLine("To add a drone, type 1.\nTo add a station, type 2.\n" +
                                              "To add a parcel, type 3.\nTo add a customer, type 4.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {
                                case (int)@enum.AddOptions.Drone:
                                    MainFuncAdd.AddDrone();
                                    break;

                                case (int)@enum.AddOptions.Station:
                                    MainFuncAdd.AddStation();
                                    break;

                                case (int) @enum.AddOptions.Parcel:
                                    MainFuncAdd.AddParcel();
                                    break;

                                case (int)@enum.AddOptions.Customer:
                                    MainFuncAdd.AddCustomer();
                                    break;
                            }
                            break;
                        }

                    case (int)@enum.options.Update:
                        {
                            Console.WriteLine("To assign pacel to drone, type 1.\nTo collect parcel by a drone, type 2.\n" +
                                          "To deliver parcel to a customer, type 3.\nTo send drone to a charging base station, type 4.\n" +
                                          "To release drone from charging station, type 5.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {

                                case (int)@enum.UpdateOptions.AssignParcelToDrone:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        DalObject.AssignParcelToDrone(IDFromUser1);
                                        break;
                                    }

                                case (int)@enum.UpdateOptions.CollectParcelByDrone:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        DalObject.CollectionOfParcelByDrone(IDFromUser1);
                                        break;
                                    }

                                case (int)@enum.UpdateOptions.DelivereParcelToCustomer:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        DalObject.DeliveryParcelToCustomer(IDFromUser1);
                                        break;
                                    }
                                case (int)@enum.UpdateOptions.SendDroneToChargingBaseStation:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Console.WriteLine("Choose from the following available stations. Enter the chosen ID (4 digits).");
                                        Station[] ListOfStation = DalObject.ListOfAvailableChargingStations();
                                        for (int i = 0; i < ListOfStation.Length; i++)
                                            Console.WriteLine(ListOfStation[i]);
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser2);
                                        DalObject.SendingDroneToChargingBaseStation(IDFromUser1, IDFromUser2);
                                        break;
                                    }

                                case (int)@enum.UpdateOptions.ReleaseDroneFromChargingBaseStation:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Console.WriteLine("Enter station ID (4 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser2);
                                        DalObject.ReleasingDroneFromChargingBaseStation(IDFromUser1, IDFromUser2);
                                        break;
                                    }

                            }
                            UserAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                            break;
                        }



                    case (int)@enum.options.DisplayIndividual:
                        {
                            Console.WriteLine("To to print a drone, type 1.\nTo print a station, type 2.\n" +
                                             "To print a parcel, type 3.\nTo print a customer, type 4.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {

                                case (int)@enum.DisplayIndividualOptions.DisplyDrone:
                                    {
                                        Console.WriteLine("Enter drone ID (3 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Drone droneWanted = DalObject.DroneDisplay(IDFromUser1);
                                        Console.WriteLine(droneWanted);
                                        break;
                                    }

                                case (int)@enum.DisplayIndividualOptions.DisplyStation:
                                    {
                                        Console.WriteLine("Enter station ID (4 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Station stationWanted = DalObject.StationDisplay(IDFromUser1);
                                        Console.WriteLine(stationWanted);
                                        break;
                                    }

                                case (int)@enum.DisplayIndividualOptions.DisplayParcel:
                                    {
                                        Console.WriteLine("Enter parcel ID (6 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Parcel parcelWanted = DalObject.ParcelDisplay(IDFromUser1);
                                        Console.WriteLine(parcelWanted);
                                        break;
                                    }

                                case (int)@enum.DisplayIndividualOptions.DisplayCustomer:
                                    {
                                        Console.WriteLine("Enter customer ID (9 digits).");
                                        Input = Console.ReadLine();
                                        int.TryParse(Input, out IDFromUser1);
                                        Customer custumerWanted = DalObject.CustomerDisplay(IDFromUser1);
                                        Console.WriteLine(custumerWanted);
                                        break;
                                    }
                            }
                            break;
                        }



                    case (int)@enum.options.DisplayList:
                        {
                            Console.WriteLine("To to print all drones, type 1.\nTo print all stations, type 2.\n" +
                                            "To print all parcels, type 3.\nTo print all customers, type 4.\n" +
                                            "To print all unassign parcels, type 5.\nTo print all available charge station, type 6.");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out UserAnswer);
                            switch (UserAnswer)
                            {
                                case (int)@enum.DisplayListOptions.DisplyDroneList:
                                    {
                                        Drone[] ListOfDrones = DalObject.ListDroneDisplay();
                                        for (int i = 0; i < ListOfDrones.Length; i++)
                                            Console.WriteLine(ListOfDrones[i]);
                                    }
                                    break;

                                case (int)@enum.DisplayListOptions.DisplyStationList:
                                    {
                                        Station[] ListOfStation = DalObject.ListStationDisplay();
                                        for (int i = 0; i < ListOfStation.Length; i++)
                                            Console.WriteLine(ListOfStation[i]);
                                    }
                                    break;

                                case (int)@enum.DisplayListOptions.DisplayParcelList:
                                    {
                                        Parcel[] ListOfParcel = DalObject.ListParcelDisplay();
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
                                case (int)@enum.DisplayListOptions.ListOfUnassignedParcels:
                                    {
                                        Parcel[] ListOfParcel = DalObject.ListOfUnassignedParcels();
                                        for (int i = 0; i < ListOfParcel.Length; i++)
                                            Console.WriteLine(ListOfParcel[i]);
                                        break;
                                    }

                                case (int)@enum.DisplayListOptions.ListOfAvailableChargingStations:
                                    {
                                        Station[] ListOfStation = DalObject.ListOfAvailableChargingStations();
                                        for (int i = 0; i < ListOfStation.Length; i++)
                                            Console.WriteLine(ListOfStation[i]);
                                        break;
                                    }

                            }
                            UserAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                            break;
                        }
                    case (int)@enum.options.FindTheDistance:
                        {
                            double lat1, lon1;
                            int ID,answer;
                            Console.WriteLine("Enter the latitude of the point from which you want to calculate distance");
                            Input = Console.ReadLine();
                            double.TryParse(Input, out lat1);
                            Console.WriteLine("Enter the longitude of the point from which you want to calculate distance");
                            Input = Console.ReadLine();
                            double.TryParse(Input, out lon1);
                            Console.WriteLine("To find the distance from the point you entered to a station, type 1\nTo find the distance from the point you entered to a customer type 0");
                            Input = Console.ReadLine();
                            int.TryParse(Input, out answer);
                            while (answer!=0 || answer!=1)
                            {
                                Console.WriteLine("To find the distance from the point you entered to a station, type 1\nTo find the distance from the point you entered to a customer type 0");
                                Input = Console.ReadLine();
                                int.TryParse(Input, out answer);
                            }
                            if (answer==1)//station
                            {
                                Console.WriteLine("Enter the ID number of the station from which you would like to measure distance ( 6-digit). ");
                                Input = Console.ReadLine();
                                int.TryParse(Input, out ID);
                                Console.WriteLine(DistanceCalculation.Calculation(lon1, lat1, DistanceCalculation.FindTheStationCoordinates(ID)));
                            }
                            if (answer == 0)//customer
                            {
                                Console.WriteLine("Enter the ID number of the customer from which you would like to measure distance (9-digit). ");
                                Input = Console.ReadLine();
                                int.TryParse(Input, out ID);
                                Console.WriteLine(DistanceCalculation.Calculation(lon1,lat1,DistanceCalculation.FindTheCustomerCoordinates(ID)));

                            }
                            break;
                        }


                    case (int)@enum.options.Exit:
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


