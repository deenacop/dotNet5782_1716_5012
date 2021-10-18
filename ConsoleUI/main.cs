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
                   
                    
            }
        }
    }
}
