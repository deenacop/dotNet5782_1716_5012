using System;

namespace ConsoleUI
{
    class main
    {
        //public enum options { Add=1, Update, DisplayIndividual, DisplayList, Exit };
        static void Main(string[] args)
        {
            IDAL.DO.DataSource.Initialize();
            Console.WriteLine("Welcome to our drone delivery system.\n" +
                "To insert, type 1." +
                "\nTo update, type 2." +
                "\nTo view a single item, type 3." +
                "\nTo view a list, type 4." +
                "\nTo exit, type 5.\n");
            int UserAnswer = Console.Read();
            switch(UserAnswer)
            {
                case 1:
                    UserChooseAddItem();
                    Console.WriteLine("*station name");
                    NewStation.Name = Console.ReadLine(); Console.WriteLine("For adding a base station Type 1." +
                        "\nTo add a drone, type 2." +
                        "\nTo receive a new customer, type 3." +
                        "\nTo receive a new sabotage, type 4.");
                    UserAnswer = Console.Read();
                    switch (UserAnswer)
                    {
                        case 1:
                            {
                                IDAL.DO.Station NewStation = new IDAL.DO.Station();
                                Console.WriteLine("Enter the new station details: *station ID (4 digits)");
                                NewStation.ID = Console.Read();
                                Console.WriteLine("*station name");
                                NewStation.Name = Console.ReadLine();
                                Console.WriteLine("*station number of available charge slots");
                                NewStation.NumOfAvailableChargeSlots = Console.Read();
                                Console.WriteLine("*station longitude");
                                NewStation.Longitude = Console.Read();
                                Console.WriteLine("*station latitude");
                                NewStation.Latitude = Console.Read();
                                IDAL.DO.DalObject.Add(NewStation);
                                break;
                            }
                        case 2:
                            {
                                IDAL.DO.Station NewStation = new IDAL.DO.Station();
                                Console.WriteLine("Enter the new station details: *station ID (4 digits)");
                                NewStation.ID = Console.Read();
                                Console.WriteLine("*station name");
                                NewStation.Name = Console.ReadLine();
                                Console.WriteLine("*station number of available charge slots");
                                NewStation.NumOfAvailableChargeSlots = Console.Read();
                                Console.WriteLine("*station longitude");
                                NewStation.Longitude = Console.Read();
                                Console.WriteLine("*station latitude");
                                NewStation.Latitude = Console.Read();
                                IDAL.DO.DalObject.Add(NewStation);
                                break;
                            }
                    }
                    break;


            }
        }
    }
}
