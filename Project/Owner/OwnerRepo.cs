using Project;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Owner
{
    public class OwnerRepo
    {
        public List<OwnerModel> games = new List<OwnerModel>();
        public List<Sale> Sales = new List<Sale>();

        public void AddGame(OwnerModel game)
        {
            games.Add(game);
        }
        public List<OwnerModel> GetAllGames()
        {
            return games;
        }


        public OwnerModel SearchGame(string name)
        {
            foreach (OwnerModel game in games)
            {
                if (game.Name.ToLower() == name.ToLower())
                {
                    return game;
                }
            }

            return null;
        }

        public bool UpdateGame(string name, float newPrice, int newStock)
        {
            OwnerModel game = SearchGame(name);

            if (game == null)
            {
                return false;
            }

            game.Price = newPrice;
            game.Stock = newStock;

            return true;
        }
        public bool UpdateGameStock(string gameName, int quantity)
        {
            OwnerModel game = SearchGame(gameName);

            if (game == null)
            {
                return false;
            }

            if (game.Stock < quantity)
            {
                return false;
            }

            game.Stock -= quantity;

            return true;
        }
        public bool RestoreGameStock(string gameName, int quantity)
        {
            OwnerModel game = SearchGame(gameName);

            if (game == null || quantity <= 0)
            {
                return false;
            }

            game.Stock += quantity;

            return true;
        }

        public bool DeleteGame(string name)
        {
            OwnerModel game = SearchGame(name);

            if (game == null)
            {
                return false;
            }

            games.Remove(game);

            return true;
        }

        public List<OwnerModel> GetLowStockGames()
        {
            List<OwnerModel> lowStockGames = new List<OwnerModel>();

            foreach (OwnerModel game in games)
            {
                if (game.Stock <= 5)
                {
                    lowStockGames.Add(game);
                }
            }

            return lowStockGames;
        }
        public List<OwnerModel> SortGamesByPrice()
        {
            List<OwnerModel> sortedGames = new List<OwnerModel>(games);

            sortedGames.Sort((game1, game2) =>
                game1.Price.CompareTo(game2.Price));

            return sortedGames;
        }
        public void AddSale(Sale sale)
        {
            Sales.Add(sale);
        }
        public float GetTotalSales()
        {
            float total = 0;

            foreach (Sale sale in Sales)
            {
                total += sale.TotalAmount;
            }

            return total;
        }
        public List<Sale> GetSalesHistory()
        {
            return Sales;
        }

        //File Handling
        public void SaveGamesToFile()
        {
            using (StreamWriter writer = new StreamWriter("games.txt"))
            {
                foreach (OwnerModel game in games)
                {
                    writer.WriteLine(
                        game.Name + "|" +
                        game.Price.ToString(CultureInfo.InvariantCulture) + "|" +
                        game.Stock + "|" +
                        game.Category
                    );
                }
            }

        }
        public void LoadGamesFromFile()
        {
            if (!File.Exists("games.txt"))
            {
                Console.WriteLine("No saved inventory file found.");
                return;
            }

            List<OwnerModel> loadedGames = new List<OwnerModel>();

            using (StreamReader reader = new StreamReader("games.txt"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split('|');

                    if (data.Length != 4)
                    {
                        Console.WriteLine("Skipping invalid game record.");
                        continue;
                    }

                    float price;
                    int stock;

                    if (string.IsNullOrWhiteSpace(data[0]) ||
                        string.IsNullOrWhiteSpace(data[3]) ||
                        !float.TryParse(
                            data[1],
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out price
                        ) ||
                        !int.TryParse(data[2], out stock) ||
                        price <= 0 ||
                        stock < 0)
                    {
                        Console.WriteLine("Skipping invalid game record: " + data[0]);
                        continue;
                    }

                    OwnerModel game = new OwnerModel();

                    game.Name = data[0];
                    game.Price = price;
                    game.Stock = stock;
                    game.Category = data[3];

                    loadedGames.Add(game);
                }
            }

            games.Clear();
            games.AddRange(loadedGames);

            Console.WriteLine("Games loaded successfully!");
        }
        public void SaveSalesToFile()
        {
            using (StreamWriter writer = new StreamWriter("sales.txt"))
            {
                foreach (Sale sale in Sales)
                {
                    writer.WriteLine(
                        sale.CustomerId + "|" +
                        sale.CustomerName + "|" +
                        sale.GameName + "|" +
                        sale.QuantitySold + "|" +
                        sale.SalePrice.ToString(CultureInfo.InvariantCulture) + "|" +
                        sale.TotalAmount.ToString(CultureInfo.InvariantCulture)
                    );
                }
            }

            Console.WriteLine("Sales history saved successfully!");
        }
        public void LoadSalesFromFile()
        {
            if (!File.Exists("sales.txt"))
            {
                Console.WriteLine("No saved sales history found.");
                return;
            }

            List<Sale> loadedSales = new List<Sale>();

            using (StreamReader reader = new StreamReader("sales.txt"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split('|');

                    if (data.Length != 6)
                    {
                        Console.WriteLine("Skipping invalid sales record.");
                        continue;
                    }

                    int customerId;
                    int quantity;
                    float salePrice;
                    float totalAmount;

                    if (!int.TryParse(data[0], out customerId) ||
                        !int.TryParse(data[3], out quantity) ||
                        !float.TryParse(
                            data[4],
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out salePrice
                        ) ||
                        !float.TryParse(
                            data[5],
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out totalAmount
                        ) ||
                        customerId <= 0 ||
                        quantity <= 0 ||
                        salePrice < 0 ||
                        totalAmount < 0 ||
                        string.IsNullOrWhiteSpace(data[1]) ||
                        string.IsNullOrWhiteSpace(data[2]))
                    {
                        Console.WriteLine("Skipping invalid sales record.");
                        continue;
                    }

                    Sale sale = new Sale();

                    sale.CustomerId = customerId;
                    sale.CustomerName = data[1];
                    sale.GameName = data[2];
                    sale.QuantitySold = quantity;
                    sale.SalePrice = salePrice;
                    sale.TotalAmount = totalAmount;

                    loadedSales.Add(sale);
                }
            }

            Sales.Clear();
            Sales.AddRange(loadedSales);

            Console.WriteLine("Sales history loaded successfully!");
        }
    }
}