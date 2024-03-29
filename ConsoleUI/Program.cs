﻿using System.Collections.Generic;
using System;
using DO;

namespace ConsoleUI
{
    public class Program
    {
        /// <summary>
        /// Add , Update, DisplayIndividual, DisplayList, FindTheDistance,Exit 
        /// </summary>
        public enum Options { Add = 1, Update, DisplayIndividual, DisplayList/*, FindTheDistance*/, Exit };
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
        public static readonly DalApi.IDal Dal = DalApi.DalFactory.GetDal();
        public static void Main(string[] args)
        {

            int userAnswer = 0;
            string input;
            int IDFromUser1, IDFromUser2;
            while (userAnswer != 6)
            {
                try
                {
                    Console.WriteLine("To insert, type 1." +
                 "\nTo update, type 2." +
                 "\nTo view a single item, type 3." +
                 "\nTo view a list, type 4." +
                 //"\nTo find a distance between a point and a station or between a point and the customer, type 5." +
                 "\nTo exit, type 6.");
                    input = Console.ReadLine();
                    int.TryParse(input, out userAnswer);
                    switch (userAnswer)
                    {
                        case (int)Options.Add:
                            {
                                Console.WriteLine("To add a drone, type 1.\nTo add a station, type 2.\n" +
                                                  "To add a parcel, type 3.\nTo add a customer, type 4.");
                                input = Console.ReadLine();
                                int.TryParse(input, out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)AddOptions.Drone:
                                        MainFuncAdd.AddDrone();
                                        break;

                                    case (int)AddOptions.Station:
                                        MainFuncAdd.AddStation();
                                        break;

                                    case (int)AddOptions.Parcel:
                                        MainFuncAdd.AddParcel();
                                        break;

                                    case (int)AddOptions.Customer:
                                        MainFuncAdd.AddCustomer();
                                        break;
                                }
                                break;
                            }

                        case (int)Options.Update:
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
                                            Dal.AssignParcelToDrone(IDFromUser1, IDFromUser2);
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
                                            Dal.CollectionOfParcelByDrone(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                    case (int)UpdateOptions.DelivereParcelToCustomer:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Dal.DeliveryParcelToCustomer(IDFromUser1);
                                            break;
                                        }
                                    case (int)UpdateOptions.SendDroneToChargingBaseStation:
                                        {
                                            Console.WriteLine("Enter drone ID (3 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Console.WriteLine("Choose from the following available stations. Enter the chosen ID (4 digits).");
                                            IEnumerable<Station> ListOfStation = Dal.GetListStation(i => i.NumOfAvailableChargingSlots > 0);
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser2);
                                            Dal.SendingDroneToChargingBaseStation(IDFromUser1, IDFromUser2);
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
                                            Dal.ReleasingDroneFromChargingBaseStation(IDFromUser1, IDFromUser2);
                                            break;
                                        }

                                }
                                userAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                                break;
                            }

                        case (int)Options.DisplayIndividual:
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
                                            Drone droneWanted = Dal.GetDrone(IDFromUser1);
                                            Console.WriteLine(droneWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplyStation:
                                        {
                                            Console.WriteLine("Enter station ID (4 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Station stationWanted = Dal.GetStation(IDFromUser1);
                                            Console.WriteLine(stationWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayParcel:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            input = Console.ReadLine();
                                            int.TryParse(input, out IDFromUser1);
                                            Parcel parcelWanted = Dal.GetParcel(IDFromUser1);
                                            Console.WriteLine(parcelWanted);
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayCustomer:
                                        {
                                            Console.WriteLine("Enter customer ID (9 digits).");
                                            int.TryParse(Console.ReadLine(), out int Id1);

                                            Customer custumerWanted = Dal.GetCustomer(Id1);
                                            Console.WriteLine(custumerWanted);
                                            break;
                                        }
                                }
                                break;
                            }

                        case (int)Options.DisplayList:
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
                                            IEnumerable<Drone> ListOfDrones = Dal.GetListDrone();
                                            foreach (Drone tmp in ListOfDrones) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplyStationList:
                                        {
                                            IEnumerable<Station> ListOfStation = Dal.GetListStation();
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayParcelList:
                                        {
                                            IEnumerable<Parcel> ListOfParcel = Dal.GetListParcel();
                                            foreach (Parcel tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayCustomerList:
                                        {
                                            IEnumerable<Customer> ListOfCustomer = Dal.GetListCustomer();
                                            foreach (Customer tmp in ListOfCustomer) { Console.WriteLine(tmp); }
                                        }
                                        break;
                                    case (int)DisplayListOptions.ListOfUnassignedParcels:
                                        {
                                            IEnumerable<Parcel> ListOfParcel = Dal.GetListParcel(i=>i.MyDroneID==0);
                                            foreach (Parcel tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                    case (int)DisplayListOptions.ListOfAvailableChargingStations:
                                        {
                                            IEnumerable<Station> ListOfStation = Dal.GetListStation(i=>i.NumOfAvailableChargingSlots>0);
                                            foreach (Station tmp in ListOfStation) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                }
                                userAnswer = 0;//if UserAnswer will stay 6 the progrom will finish without wanting it to.
                                break;
                            }
                        //case (int)options.FindTheDistance:
                        //    {
                        //        double lat1, lon1;
                        //        int ID, answer;
                        //        Console.WriteLine("Enter the latitude of the point from which you want to calculate distance");
                        //        input = Console.ReadLine();
                        //        double.TryParse(input, out lat1);
                        //        Console.WriteLine("Enter the longitude of the point from which you want to calculate distance");
                        //        input = Console.ReadLine();
                        //        double.TryParse(input, out lon1);
                        //        Console.WriteLine("To find the distance from the point you entered to a station, type 1\n" +
                        //            "To find the distance from the point you entered to a customer type 0");
                        //        input = Console.ReadLine();
                        //        int.TryParse(input, out answer);
                        //        if (answer == 1)//station
                        //        {
                        //            Console.WriteLine("Enter the ID number of the station from which you would like to measure distance ( 4-digit). ");
                        //            input = Console.ReadLine();
                        //            int.TryParse(input, out ID);
                        //            Console.WriteLine(DistanceCalculation.Calculation(lon1, lat1, DistanceCalculation.FindTheStationCoordinates(ID)));
                        //        }
                        //        if (answer == 0)//customer
                        //        {
                        //            Console.WriteLine("Enter the ID number of the customer from which you would like to measure distance (9-digit). ");
                        //            input = Console.ReadLine();
                        //            int.TryParse(input, out ID);
                        //            Console.WriteLine(DistanceCalculation.Calculation(lon1, lat1, DistanceCalculation.FindTheCustomerCoordinates(ID)));

                        //        }
                        //        break;
                        //    }

                        case (int)Options.Exit:
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


