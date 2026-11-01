using Project;
using Project.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Owner
{
    public class OwnerService
    {
        private OwnerRepo repo;

        public OwnerService(OwnerRepo repository)
        {
            repo = repository;
        }
        public void AddGame(OwnerModel game)
        {
            if (game == null)
            {
                Console.WriteLine("\nInvalid game information.");
                return;
            }

            if (string.IsNullOrWhiteSpace(game.Name))
            {
                Console.WriteLine("\nGame name is required.");
                return;
            }

            if (game.Price <= 0)
            {
                Console.WriteLine("\nPrice must be greater than zero.");
                return;
            }

            if (game.Stock < 0)
            {
                Console.WriteLine("\nStock cannot be negative.");
                return;
            }

            repo.AddGame(game);

            SaveGamesToFile(); //file Handling
        }
        public List<OwnerModel> ViewGame()
        {
            return repo.GetAllGames();
        }

        public OwnerModel SearchGame(string name)
        {
            return repo.SearchGame(name);
        }

        public bool UpdateGame(string name, float newPrice, int newStock)
        {
            if (newPrice <= 0)
            {
                Console.WriteLine("\nPrice must be greater than zero.");
                return false;
            }

            if (newStock < 0)
            {
                Console.WriteLine("\nStock cannot be negative.");
                return false;
            }

            bool isUpdated = repo.UpdateGame(name, newPrice, newStock);

            if (isUpdated)
            {
                SaveGamesToFile(); //file handling
            }

            return isUpdated;
        }
        public bool UpdateGameStock(string gameName, int quantity)
        {
            return repo.UpdateGameStock(gameName, quantity);
        }
        public bool RestoreGameStock(string gameName, int quantity)
        {
            return repo.RestoreGameStock(gameName, quantity);
        }

        public bool DeleteGame(string name)
        {
            bool isDeleted = repo.DeleteGame(name);

            if (isDeleted)
            {
                SaveGamesToFile(); //file handling
            }

            return isDeleted;
        }

        public List<OwnerModel> GetLowStockGames()
        {
            return repo.GetLowStockGames();
        }
        public List<OwnerModel> SortGamesByPrice()
        {
            return repo.SortGamesByPrice();
        }
        public void AddSale(Sale sale)
        {
            repo.AddSale(sale);
        }
        public float GetTotalSales()
        {
            return repo.GetTotalSales();
        }
        public List<Sale> GetSalesHistory()
        {
            return repo.GetSalesHistory();
        }

        //File Handling
        public void SaveGamesToFile()
        {
            repo.SaveGamesToFile();
        }
        public void LoadGamesFromFile()
        {
            repo.LoadGamesFromFile();
        }
        public void SaveSalesToFile()
        {
            repo.SaveSalesToFile();
        }
        public void LoadSalesFromFile()
        {
            repo.LoadSalesFromFile();
        }
    }
}