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
        public enum Options { Add = 1, Update, DisplayIndividual, DisplayList, Exit };
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
                        case (int)Options.Add:
                            {
                                Console.WriteLine("To add a drone, type 1.\nTo add a Base station, type 2.\n" +
                                                  "To add a parcel, type 3.\nTo add a customer, type 4.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)AddOptions.Drone:
                                        Drone newDrone = new();
                                        newDrone.Location = new();
                                        Console.WriteLine("Enter the drone ID (3 digits)");
                                        int.TryParse(Console.ReadLine(), out int ID);
                                        newDrone.Id = ID;
                                        Console.WriteLine("Enter the drone model");
                                        newDrone.Model = Console.ReadLine();
                                        Console.WriteLine("Choose the drone Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        int.TryParse(Console.ReadLine(), out int status);
                                        newDrone.Weight = (WeightCategories)status;
                                        Console.WriteLine("Enter the ID station which in you want to charge the new drone (4 digits) -from the list:");
                                        IEnumerable<BaseStationToList> ListOfStation = bl.GetBaseStationList();
                                        foreach (BaseStationToList tmp in ListOfStation) { Console.WriteLine("\t" + tmp.Id + "\t" + tmp.Name); }
                                        int chosenStation;
                                        int.TryParse(Console.ReadLine(), out chosenStation);
                                        bl.AddDrone(newDrone, chosenStation);
                                        break;

                                    case (int)AddOptions.Station:
                                        BaseStation newStation = new();
                                        newStation.DronesInCharging = new();
                                        newStation.Location = new();
                                        Console.WriteLine("Enter the station ID (4 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newStation.Id = ID;
                                        Console.WriteLine("Enter the station name");
                                        newStation.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude (range is (35,36)");
                                        double.TryParse(Console.ReadLine(), out double lon);
                                        newStation.Location.Longitude = lon;
                                        Console.WriteLine("Enter the location- latitude(range is (31,32))");
                                        double.TryParse(Console.ReadLine(), out double lat);
                                        newStation.Location.Latitude = lat;
                                        Console.WriteLine("Enter the number of available slots");
                                        int.TryParse(Console.ReadLine(), out int num);
                                        newStation.NumOfAvailableChargingSlots = num;
                                        bl.AddBaseStation(newStation);
                                        break;

                                    case (int)AddOptions.Parcel:
                                        Parcel newParcel = new();
                                        newParcel.SenderCustomer = new();
                                        newParcel.TargetidCustomer = new();
                                        Console.WriteLine("Enter the parcel sender ID (9 digits) from the customer list");
                                        IEnumerable<CustomerToList> ListOfCustomer = bl.GetListCustomer();
                                        foreach (CustomerToList tmp in ListOfCustomer) { Console.WriteLine("\t" + tmp.Name + "\t" + tmp.Id); }
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newParcel.SenderCustomer.Id = ID;
                                        Console.WriteLine("Enter the parcel targetid ID (9 digits)from the customer list");

                                        foreach (CustomerToList tmp in ListOfCustomer) { Console.WriteLine("\t" + tmp.Name + "\t" + tmp.Id); }
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newParcel.TargetidCustomer.Id = ID;
                                        Console.WriteLine("Choose the parcel Weight: 1 for light, 2 for midium, and 3 for heavy");
                                        int.TryParse(Console.ReadLine(), out status);
                                        newParcel.Weight = (WeightCategories)status;
                                        Console.WriteLine("Enter the parcel priority");
                                        int.TryParse(Console.ReadLine(), out status);
                                        newParcel.Priority = (Priorities)status;
                                        bl.AddParcel(newParcel);
                                        break;

                                    case (int)AddOptions.Customer:
                                        Customer newCustomer = new();
                                        newCustomer.Location = new();
                                        newCustomer.FromCustomer = new();
                                        newCustomer.ToCustomer = new();
                                        Console.WriteLine("Enter the customer ID (9 digits)");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        newCustomer.Id = ID;
                                        Console.WriteLine("Enter the customer name");
                                        newCustomer.Name = Console.ReadLine();
                                        Console.WriteLine("Enter the customer phone number");
                                        newCustomer.PhoneNumber = Console.ReadLine();
                                        Console.WriteLine("Enter the location- longitude (range is (35,36)");
                                        double.TryParse(Console.ReadLine(), out lon);
                                        newCustomer.Location.Longitude = lon;
                                        Console.WriteLine("Enter the location- latitude range is (31,32)");
                                        double.TryParse(Console.ReadLine(), out lat);
                                        newCustomer.Location.Latitude = lat;

                                        bl.AddCustomer(newCustomer);
                                        break;
                                }
                                break;
                            }

                        case (int)Options.Update:
                            {
                                Console.WriteLine("To update the drone model, type 1.\nTo update the base station details, type 2.\n" +
                                    "To update the customer details, type 3.\nTo send a drone to charge, type 4.\n" +
                                    "To release a drone frome the charging, type 5.\nTo assign parcel to drone, type 6.\n" +
                                    "To collect a parcel by a drone, type 7.\nTo deliver parcel by a drone, type 8.");
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)UpdateOptions.UpdateDroneModel:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits) ");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("Enter the wanted model");
                                            string model = Console.ReadLine();
                                            Drone tmp = bl.GetDrone(ID);
                                          
                                            tmp.Model = model;
                                            bl.UpdateDrone(tmp);
                                            break;
                                        }

                                    case (int)UpdateOptions.UpdateBaseStationDetails:
                                        {
                                            Console.WriteLine("Enter The station ID (4 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("if you want to change the name, enter the new name else continue");
                                            string ansName = Console.ReadLine();
                                            if (ansName == "")
                                                ansName = null;
                                            Console.WriteLine("if you want to change the number of slots in the station, enter the new number of slots else continue");
                                            if (!int.TryParse(Console.ReadLine(), out int ansNum))
                                                ansNum = 0;
                                            BaseStation tmp = bl.GetBaseStation(ID);
                                            if(ansName!=null)
                                               tmp.Name = ansName;
                                            if (ansNum != 0)
                                                tmp.NumOfAvailableChargingSlots = ansNum;
                                            bl.UpdateStation(tmp);
                                            break;
                                        }

                                    case (int)UpdateOptions.UpdateCustomerDetails:
                                        {
                                            Console.WriteLine("Enter The customer ID (9 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("if you want to change the name, enter the new name else continue");
                                            string ansName = Console.ReadLine();
                                            if (ansName == "")
                                                ansName = null;
                                            Console.WriteLine("if you want to change the phone number of the customer, enter the new phone number else continue");
                                            string ansPhone = Console.ReadLine();
                                            if (ansPhone == "")
                                                ansName = null;
                                            Customer tmp = bl.GetCustomer(ID);
                                            if(ansName!=null)
                                                tmp.Name = ansName;
                                            if (ansPhone != null)
                                                tmp.PhoneNumber = ansPhone;
                                            bl.UpdateCustomer(tmp);
                                            break;
                                        }
                                    case (int)UpdateOptions.SendDroneToCharge:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Drone drone = bl.GetDrone(ID);
                                            bl.SendDroneToCharge(drone);
                                            break;
                                        }

                                    case (int)UpdateOptions.ReleaseDroneFromeCharging:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits)");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Console.WriteLine("and the the amount of time(by minute) that the drone was in the chaging base station");
                                            int.TryParse(Console.ReadLine(), out int num);
                                            Drone drone = bl.GetDrone(ID);
                                            bl.ReleasingDroneFromBaseStation(drone, num);
                                            break;
                                        }
                                    case (int)UpdateOptions.AssignParcelToDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Drone drone = bl.GetDrone(ID);
                                            bl.AssignParcelToDrone(drone);
                                            break;
                                        }
                                    case (int)UpdateOptions.CollectParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Drone drone = bl.GetDrone(ID);
                                            bl.CollectionParcelByDrone(drone);
                                            break;
                                        }
                                    case (int)UpdateOptions.DeliverParcelByDrone:
                                        {
                                            Console.WriteLine("Enter The drone ID (3 digits).");
                                            int.TryParse(Console.ReadLine(), out int ID);
                                            Drone drone = bl.GetDrone(ID);
                                            bl.DeliveryParcelByDrone(drone);
                                            break;
                                        }
                                }
                                userAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                                break;
                            }

                        case (int)Options.DisplayIndividual:
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
                                            Console.WriteLine(bl.GetDrone(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplyStation:
                                        {
                                            Console.WriteLine("Enter base station ID (4 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.GetBaseStation(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayParcel:
                                        {
                                            Console.WriteLine("Enter parcel ID (6 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.GetParcel(IDFromUser1));
                                            break;
                                        }

                                    case (int)DisplayIndividualOptions.DisplayCustomer:
                                        {
                                            Console.WriteLine("Enter customer ID (9 digits).");
                                            int.TryParse(Console.ReadLine(), out IDFromUser1);
                                            Console.WriteLine(bl.GetCustomer(IDFromUser1));
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
                                int.TryParse(Console.ReadLine(), out userAnswer);
                                switch (userAnswer)
                                {
                                    case (int)DisplayListOptions.DisplyDroneList:
                                        {
                                            IEnumerable<DroneToList> ListOfDrones = bl.GetDroneList();
                                            foreach (DroneToList tmp in ListOfDrones) { Console.WriteLine(tmp); }
                                        }
                                        break;


                                    case (int)DisplayListOptions.DisplyStationList:
                                        {
                                            IEnumerable<BaseStationToList> ListOfStation = bl.GetBaseStationList();
                                            foreach (BaseStationToList tmp in ListOfStation) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayParcelList:
                                        {
                                            IEnumerable<ParcelToList> ListOfParcel = bl.GetListParcel();
                                            foreach (ParcelToList tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                        }
                                        break;

                                    case (int)DisplayListOptions.DisplayCustomerList:
                                        {
                                            IEnumerable<CustomerToList> ListOfCustomer = bl.GetListCustomer();
                                            foreach (CustomerToList tmp in ListOfCustomer) { Console.WriteLine(tmp); }
                                        }
                                        break;
                                    case (int)DisplayListOptions.ListOfUnassignedParcels:
                                        {
                                            IEnumerable<ParcelToList> ListOfParcel = bl.GetListParcel(i => i.Status ==ParcelStatus.Defined);
                                            foreach (ParcelToList tmp in ListOfParcel) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                    case (int)DisplayListOptions.ListOfAvailableChargingStations:
                                        {
                                            IEnumerable<BaseStationToList> ListOfStation = bl.GetBaseStationList(i => i.NumOfAvailableChargingSlots > 0);
                                            foreach (BaseStationToList tmp in ListOfStation) { Console.WriteLine(tmp); }
                                            break;
                                        }

                                }
                                userAnswer = 0;//if UserAnswer will stay 5 the progrom will finish without wanting it to.
                                break;
                            }
                        case (int)Options.Exit:
                            Console.WriteLine("\nThank you for using our drones system, looking forward to see you again!");
                            break;

                        default:
                            Console.WriteLine("Wrong input");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
        }
    }
}



