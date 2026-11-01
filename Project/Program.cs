using Project.Owner;
using Project.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OwnerRepo ownerRepo = new OwnerRepo();

            Console.WriteLine("\nLoading application data...");

            ownerRepo.LoadGamesFromFile();
            ownerRepo.LoadSalesFromFile();

            Console.WriteLine("\nApplication ready!");
            Console.ReadKey();

            OwnerUI ownerUI = new OwnerUI(ownerRepo);
            CustomerUI customerUI = new CustomerUI(ownerRepo);

            while (true)
            {
                Console.Clear();

                Console.WriteLine("==============================");
                Console.WriteLine("     GAMING STORE SYSTEM");
                Console.WriteLine("==============================\n");

                Console.WriteLine("1. Owner Login");
                Console.WriteLine("2. Customer Menu");
                Console.WriteLine("3. Exit");

                Console.Write("\nEnter your choice: ");

                int choice = int.Parse(Console.ReadLine() ?? "0");

                if (choice == 1)
                {
                    bool isLoggedIn = ownerUI.Login();

                    if (isLoggedIn)
                    {
                        ownerUI.ShowMenu();
                    }
                }
                else if (choice == 2)
                {
                    customerUI.ShowMenu();
                }
                else if (choice == 3)
                {
                    Console.WriteLine("\nApplication closed.");
                    break;
                }
                else
                {
                    Console.WriteLine("\nInvalid choice!");
                    Console.ReadKey();
                }
            }
        }
    }
}   