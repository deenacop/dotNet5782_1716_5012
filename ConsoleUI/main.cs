using System;

namespace ConsoleUI
{
    class main
    {
        enum options { Add = 1, Update, DisplayIndividual, DisplayList, Exit };
        enum AddOptions { Drone = 1, Station, Parcel, Customer };
        enum UpdateOptions { AssignParcelToDrone = 1, CollectParcelByDrone, DelivereParcelToCustomer, SendDroneToChargingBaseStation, ReleaseDroneFromChargingBaseStation };
        enum DisplayIndividualOptions { DisplyDrone = 1, DisplyStation, DisplayParcel, DisplayCustomer };
        enum DisplayListOptions { DisplyDroneList = 1, DisplyStationList, DisplayParcelList, DisplayCustomerList, ListOfUnassignedParcels, ListOfAvailableChargingStations };
        static void Main(string[] args)
        {
            IDAL.DO.DataSource.Initialize();
            Console.WriteLine("Welcome to our drone delivery system.\n" +
                "To insert, type 1." +
                "\nTo update, type 2." +
                "\nTo view a single item, type 3." +
                "\nTo view a list, type 4." +
                "\nTo exit, type 5.");
            int UserAnswer = 0;
            while (UserAnswer != 5)
            {
                String input = Console.ReadLine();
                if (int.TryParse(input, out UserAnswer))
                {
                    switch (UserAnswer)
                    {
                        case (int)options.Add:
                            {
                                Console.WriteLine("To add a drone, type 1.\n To add a station, type 2.\n"+
                                                  "To add a parcel, type 3.\n To add a customer, type 4." );
                                input = Console.ReadLine();
                                if (int.TryParse(input, out UserAnswer))
                                {
                                    switch (UserAnswer)
                                    {
                                        case (int)AddOptions.Drone:
                                            {
                                                
                                            }
                                            break;

                                        case (int)AddOptions.Station:
                                            {
                                                
                                                IDAL.DO.mainFuncAdd.AddStation();
                                            }
                                            break;

                                        case (int)AddOptions.Parcel:
                                            {
                                                IDAL.DO.Parcel p = new IDAL.DO.Parcel();
                                                IDAL.DO.DalObject.Add(p);
                                            }
                                            break;

                                        case (int)AddOptions.Customer:
                                            {
                                                IDAL.DO.Customer c = new IDAL.DO.Customer();
                                                IDAL.DO.DalObject.Add(c);
                                            }
                                            break;
                                    }
                                }
                                break;
                            }


                        case (int)options.Update:
                            {
                                input = Console.ReadLine();
                                if (int.TryParse(input, out UserAnswer))
                                {
                                    switch (UserAnswer)
                                    {
                                        case (int)UpdateOptions.AssignParcelToDrone:
                                            IDAL.DO.DalObject.AssignParcelToDrone(345678);
                                            break;

                                        case (int)UpdateOptions.CollectParcelByDrone:
                                            IDAL.DO.DalObject.CollectionOfParcelByDrone(123456);
                                            break;

                                        case (int)UpdateOptions.DelivereParcelToCustomer:
                                            IDAL.DO.DalObject.DeliveryParcelToCustomer(123456);
                                            break;

                                        case (int)UpdateOptions.SendDroneToChargingBaseStation:
                                            IDAL.DO.DalObject.SendingDroneToChargingBaseStation(123, 1234);
                                            break;

                                        case (int)UpdateOptions.ReleaseDroneFromChargingBaseStation:
                                            IDAL.DO.DalObject.ReleasingDroneFromChargingBaseStation(123, 1234);
                                            break;
                                    }
                                }
                                break;
                            }

                        case (int)options.DisplayIndividual:
                            {
                                input = Console.ReadLine();
                                if (int.TryParse(input, out UserAnswer))
                                {
                                    switch (UserAnswer)
                                    {
                                        case (int)DisplayIndividualOptions.DisplyDrone:
                                            IDAL.DO.DalObject.DroneDisplay(123);
                                            break;

                                        case (int)DisplayIndividualOptions.DisplyStation:
                                            IDAL.DO.DalObject.StationDisplay(1234);
                                            break;

                                        case (int)DisplayIndividualOptions.DisplayParcel:
                                            IDAL.DO.DalObject.ParcelDisplay(123456);
                                            break;

                                        case (int)DisplayIndividualOptions.DisplayCustomer:
                                            IDAL.DO.DalObject.CustomerDisplay(324281716);
                                            break;
                                    }
                                }
                                break;
                            }

                        case (int)options.DisplayList:
                            {
                                input = Console.ReadLine();
                                if (int.TryParse(input, out UserAnswer))
                                {
                                    switch (UserAnswer)
                                    {
                                        case (int)DisplayListOptions.DisplyDroneList:
                                            IDAL.DO.DalObject.ListDroneDisplay();
                                            break;

                                        case (int)DisplayListOptions.DisplyStationList:
                                            IDAL.DO.DalObject.ListStationDisplay();
                                            break;

                                        case (int)DisplayListOptions.DisplayParcelList:
                                            IDAL.DO.DalObject.ListParcelDisplay();
                                            break;

                                        case (int)DisplayListOptions.DisplayCustomerList:
                                            IDAL.DO.DalObject.ListCustomerDisplay();
                                            break;

                                        case (int)DisplayListOptions.ListOfUnassignedParcels:
                                            IDAL.DO.DalObject.ListOfUnassignedParcels();
                                            break;

                                        case (int)DisplayListOptions.ListOfAvailableChargingStations:
                                            IDAL.DO.DalObject.ListOfAvailableChargingStations();
                                            break;
                                    }
                                }
                                break;
                            }

                        case (int)options.Exit:
                            Console.WriteLine("bey");
                            break;

                        default:
                            Console.WriteLine("Rong input");
                            break;

                    }
                }
            }
        }
    }
}
          
          