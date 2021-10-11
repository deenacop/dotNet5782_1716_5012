using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome1716();
            Welcome5012();
            Console.ReadKey();
        }
        static partial void Welcome5012();
        private static void Welcome1716()
        {
            Console.WriteLine("Enter your name: ");

            string useName = Console.ReadLine();

            Console.WriteLine("{0}, welcome to my first consle application", useName);

            
        }
    }
}
