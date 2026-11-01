using Project.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Owner
{
    public class OwnerUI
    {
        private OwnerService service;

        public OwnerUI(OwnerRepo repository)
        {
            service = new OwnerService(repository);
        }
        public bool Login()
        {
            Console.Clear();

            Console.WriteLine("===== OWNER LOGIN =====\n");

            Console.Write("Enter Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Enter Password: ");
            string password = Console.ReadLine() ?? "";

            if (username == "admin" && password == "admin")
            {
                Console.WriteLine("\nLogin successful!");
                return true;
            }
            else
            {
                Console.WriteLine("\nInvalid username or password.");
                return false;
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("===OWNER MENU===\n");
                Console.WriteLine("1. Add Game");
                Console.WriteLine("2. View Game");
                Console.WriteLine("3. Search Game");
                Console.WriteLine("4. Update Game");
                Console.WriteLine("5. Delete Game");
                Console.WriteLine("6. Low Stock");
                Console.WriteLine("7. Sort By Price");
                Console.WriteLine("8. Total Sales");
                Console.WriteLine("9. Sales History");
                Console.WriteLine("10. Save Games to File"); //file handling option
                Console.WriteLine("11. Load Games from File"); //file handling option
                Console.WriteLine("12. Save Sales History to File");
                Console.WriteLine("13. Load Sales History from File");
                Console.WriteLine("14. Exit");
                Console.Write("\nEnter your choice = ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    AddGame();
                    Pause();
                }
                else if (choice == "2")
                {
                    ViewGame();
                    Pause();
                }
                else if (choice == "3")
                {
                    SearchGame();
                    Pause();
                }
                else if (choice == "4")
                {
                    UpdateGame();
                    Pause();
                }
                else if (choice == "5")
                {
                    DeleteGame();
                    Pause();
                }
                else if (choice == "6")
                {
                    LowStock();
                    Pause();
                }
                else if (choice == "7")
                {
                    SortByPrice();
                    Pause();
                }
                else if (choice == "8")
                {
                    TotalSales();
                    Pause();
                }
                else if (choice == "9")
                {
                    SalesHistory();
                    Pause();
                }
                else if (choice == "10") //File Handling option
                {
                    SaveGames();
                }
                else if (choice == "11") //file handling option
                {
                    LoadGames();
                }
                else if (choice == "12")
                {
                    SaveSalesHistory();
                }
                else if (choice == "13")
                {
                    LoadSalesHistory();
                }

                else if (choice == "14")
                {
                    Console.WriteLine("\nExiting Owner Menu");
                    break;
                }
                else
                {
                    Console.WriteLine("\nInvalid Choice.");
                    Pause();
                }
            }
        }

        private void AddGame()
        {
            Console.Clear();

            Console.WriteLine("===== ADD NEW GAME =====\n");

            Console.Write("Enter game name: ");
            string name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\nGame name cannot be empty.");
                return;
            }

            Console.Write("Enter game price: ");
            float price;

            if (!float.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.WriteLine("\nPlease enter a valid price greater than zero.");
                return;
            }

            Console.Write("Enter available stock: ");
            int stock;

            if (!int.TryParse(Console.ReadLine(), out stock) || stock < 0)
            {
                Console.WriteLine("\nPlease enter a valid non-negative stock quantity.");
                return;
            }

            Console.Write("Enter game category: ");
            string category = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("\nGame category cannot be empty.");
                return;
            }

            OwnerModel game = new OwnerModel();

            game.Name = name;
            game.Price = price;
            game.Stock = stock;
            game.Category = category;

            service.AddGame(game);

            Console.WriteLine("\nGame added successfully!");
        }

        private void ViewGame()
        {
            Console.Clear();

            Console.WriteLine("==============================================");
            Console.WriteLine("              AVAILABLE GAMES");
            Console.WriteLine("==============================================\n");

            var games = service.ViewGame();

            if (games.Count == 0)
            {
                Console.WriteLine("No games available.");
                return;
            }

            Console.WriteLine(
                "{0,-25} {1,-12} {2,-10} {3,-20}",
                "GAME NAME",
                "PRICE",
                "STOCK",
                "CATEGORY"
            );

            Console.WriteLine(
                "--------------------------------------------------------------------------"
            );

            foreach (OwnerModel game in games)
            {
                Console.WriteLine(
                    "{0,-25} {1,-12} {2,-10} {3,-20}",
                    game.Name,
                    game.Price,
                    game.Stock,
                    game.Category
                );
            }

            Console.WriteLine(
                "\n--------------------------------------------------------------------------"
            );

            Console.WriteLine("Total Games: " + games.Count);
        }

        private void SearchGame()
        {
            Console.Clear();
            Console.WriteLine("===OWNER MENU===\n");

            Console.Write("Enter game name: ");
            string name = Console.ReadLine() ?? "";

            OwnerModel game = service.SearchGame(name);

            if (game != null)
            {
                Console.WriteLine("\nGame Found!");

                Console.WriteLine("Name: " + game.Name);
                Console.WriteLine("Price: " + game.Price);
                Console.WriteLine("Stock: " + game.Stock);
                Console.WriteLine("Category: " + game.Category);
            }
            else
            {
                Console.WriteLine("\nGame not found!");
            }
        }

        private void UpdateGame()
        {
            Console.Clear();

            Console.WriteLine("===== UPDATE GAME =====\n");

            Console.Write("Enter game name: ");
            string name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\nGame name cannot be empty.");
                return;
            }

            OwnerModel game = service.SearchGame(name);

            if (game == null)
            {
                Console.WriteLine("\nGame not found!");
                return;
            }

            Console.WriteLine("\nGame Found!");

            Console.WriteLine("Current Price: " + game.Price);
            Console.Write("Enter new price: ");

            float newPrice;

            if (!float.TryParse(Console.ReadLine(), out newPrice) || newPrice <= 0)
            {
                Console.WriteLine("\nPlease enter a valid price greater than zero.");
                return;
            }

            Console.WriteLine("Current Stock: " + game.Stock);
            Console.Write("Enter new stock: ");

            int newStock;

            if (!int.TryParse(Console.ReadLine(), out newStock) || newStock < 0)
            {
                Console.WriteLine("\nPlease enter a valid non-negative stock quantity.");
                return;
            }

            bool updated = service.UpdateGame(name, newPrice, newStock);

            if (updated)
            {
                Console.WriteLine("\nGame updated successfully!");
            }
            else
            {
                Console.WriteLine("\nGame update failed!");
            }
        }

        private void DeleteGame()
        {
            Console.Clear();
            Console.WriteLine("===OWNER MENU===\n");

            Console.Write("Enter game name to delete: ");
            string name = Console.ReadLine() ?? "";

            OwnerModel game = service.SearchGame(name);

            if (game == null)
            {
                Console.WriteLine("\nGame not found!");
                return;
            }

            Console.WriteLine("\nGame Found!");
            Console.WriteLine("Name: " + game.Name);
            Console.WriteLine("Price: " + game.Price);
            Console.WriteLine("Stock: " + game.Stock);

            Console.Write("\nAre you sure you want to delete this game? (Y/N): ");
            string confirmation = Console.ReadLine() ?? "";

            if (confirmation.ToUpper() == "Y")
            {
                bool deleted = service.DeleteGame(name);

                if (deleted)
                {
                    Console.WriteLine("\nGame deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nGame deletion failed!");
                }
            }
            else
            {
                Console.WriteLine("\nDeletion cancelled.");
            }
        }
        private void LowStock()
        {
            Console.Clear();

            Console.WriteLine("===OWNER MENU===\n");

            List<OwnerModel> lowStockGames = service.GetLowStockGames();

            if (lowStockGames.Count == 0)
            {
                Console.WriteLine("\nNo low-stock games found.");
                return;
            }

            foreach (OwnerModel game in lowStockGames)
            {
                Console.WriteLine("\nGame Name: " + game.Name);
                Console.WriteLine("Available Stock: " + game.Stock);
                Console.WriteLine("-------------------------");
            }
        }
        private void SortByPrice()
        {
            Console.Clear();

            Console.WriteLine("===OWNER MENU===\n");

            List<OwnerModel> sortedGames = service.SortGamesByPrice();

            if (sortedGames.Count == 0)
            {
                Console.WriteLine("\nNo games available to sort.");
                return;
            }

            foreach (OwnerModel game in sortedGames)
            {
                Console.WriteLine("\nGame Name: " + game.Name);
                Console.WriteLine("Price: " + game.Price);
                Console.WriteLine("Stock: " + game.Stock);
                Console.WriteLine("Category: " + game.Category);
                Console.WriteLine("-------------------------");
            }
        }
        private void TotalSales()
        {
            Console.Clear();

            Console.WriteLine("===OWNER MENU===\n");

            float totalSales = service.GetTotalSales();

            Console.WriteLine("\nTotal Revenue: PKR " + totalSales);
        }
        private void SalesHistory()
        {
            Console.Clear();

            Console.WriteLine("===OWNER MENU===\n");

            List<Sale> sales = service.GetSalesHistory();

            if (sales.Count == 0)
            {
                Console.WriteLine("\nNo sales records found.");
                return;
            }

            foreach (Sale sale in sales)
            {
                Console.WriteLine("\nGame Name: " + sale.GameName);
                Console.WriteLine("Quantity Sold: " + sale.QuantitySold);
                Console.WriteLine("Sale Price: " + sale.SalePrice);
                Console.WriteLine("Total Amount: " + sale.TotalAmount);
                Console.WriteLine("-------------------------");
            }
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to go back to menu...");
            Console.ReadKey();
        }

        //File Handling
        private void SaveGames()
        {
            Console.Clear();

            Console.WriteLine("===== SAVE GAMES =====");

            service.SaveGamesToFile();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        private void LoadGames()
        {
            Console.Clear();

            Console.WriteLine("===== LOAD GAMES =====");

            service.LoadGamesFromFile();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        private void SaveSalesHistory()
        {
            Console.Clear();

            Console.WriteLine("===== SAVE SALES HISTORY =====");

            service.SaveSalesToFile();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        private void LoadSalesHistory()
        {
            Console.Clear();

            Console.WriteLine("===== LOAD SALES HISTORY =====");

            service.LoadSalesFromFile();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}