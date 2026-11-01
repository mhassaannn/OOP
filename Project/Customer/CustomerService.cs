using Project.Customer;
using Project.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CustomerService
    {
        CustomerRepo repo = new CustomerRepo();
        private CartRepo cartRepo = new CartRepo();
        public void SetActiveCustomer(int customerId)
        {
            cartRepo.SetActiveCustomer(customerId);
        }

        public void ClearActiveCustomer()
        {
            cartRepo.SetActiveCustomer(0);
        }
        private OwnerRepo ownerRepo;
        public CustomerService(OwnerRepo repository)
        {
            ownerRepo = repository;
            ownerService = new OwnerService(repository);

            repo.LoadCustomersFromFile();
        }
        private OwnerService ownerService;
        public void AddCustomer(CustomerModel customer)
        {
            if (customer == null)
            {
                Console.WriteLine("\nInvalid customer information.");
                return;
            }

            if (customer.CustomerId <= 0)
            {
                Console.WriteLine("\nCustomer ID must be greater than zero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                Console.WriteLine("\nCustomer name cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(customer.Username))
            {
                Console.WriteLine("\nUsername cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                Console.WriteLine("\nPassword cannot be empty.");
                return;
            }

            if (repo.CustomerIdExists(customer.CustomerId))
            {
                Console.WriteLine("\nThis Customer ID is already registered.");
                return;
            }

            if (repo.UsernameExists(customer.Username))
            {
                Console.WriteLine("\nThis username is already taken.");
                return;
            }

            repo.AddCustomer(customer);

            repo.SaveCustomersToFile();
        }

        public List<CustomerModel> GetAllCustomers()
        {
            return repo.GetAllCustomers();
        }
        public List<OwnerModel> GetAvailableGames()
        {
            return ownerRepo.GetAllGames();
        }

        public bool AddToCart(OwnerModel game, int quantity)
        {
            if (game == null)
            {
                Console.WriteLine("Game not found.");
                return false;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero.");
                return false;
            }

            OwnerModel availableGame = ownerRepo.SearchGame(game.Name);

            if (availableGame == null)
            {
                Console.WriteLine("Game is no longer available.");
                return false;
            }

            CartModel existingItem = null;

            foreach (CartModel item in cartRepo.GetCartItems())
            {
                if (item.Game.Name.ToLower() == game.Name.ToLower())
                {
                    existingItem = item;
                    break;
                }
            }

            int existingQuantity = 0;

            if (existingItem != null)
            {
                existingQuantity = existingItem.Quantity;
            }

            if (existingQuantity + quantity > availableGame.Stock)
            {
                Console.WriteLine("Not enough stock available.");
                return false;
            }

            if (existingItem != null)
            {
                cartRepo.UpdateCartQuantity(
                    availableGame.Name,
                    existingQuantity + quantity
                );
            }
            else
            {
                CartModel newItem = new CartModel();

                newItem.Game = availableGame;
                newItem.Quantity = quantity;

                cartRepo.AddToCart(newItem);
            }

            return true;
        }

        public List<CartModel> GetCartItems()
        {
            return cartRepo.GetCartItems();
        }
        public List<Sale> GetCustomerPurchaseHistory(int customerId)
        {
            List<Sale> customerSales = new List<Sale>();

            foreach (Sale sale in ownerRepo.GetSalesHistory())
            {
                if (sale.CustomerId == customerId)
                {
                    customerSales.Add(sale);
                }
            }

            return customerSales;
        }
        public bool RemoveFromCart(string gameName)
        {
            return cartRepo.RemoveFromCart(gameName);
        }
        public bool UpdateCartQuantity(string gameName, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero.");
                return false;
            }

            OwnerModel game = ownerRepo.SearchGame(gameName);

            if (game == null)
            {
                Console.WriteLine("Game not found in store.");
                return false;
            }

            if (newQuantity > game.Stock)
            {
                Console.WriteLine("Not enough stock available.");
                return false;
            }

            return cartRepo.UpdateCartQuantity(gameName, newQuantity);
        }
        public bool UpdateGameStock(string gameName, int quantity)
        {
            return ownerService.UpdateGameStock(gameName, quantity);
        }
        public bool Checkout(CustomerModel customer)
        {
            List<CartModel> cartItems = cartRepo.GetCartItems();

            if (customer == null)
            {
                Console.WriteLine("Invalid customer information.");
                return false;
            }

            if (cartItems.Count == 0)
            {
                Console.WriteLine("Your shopping cart is empty.");
                return false;
            }

            foreach (CartModel item in cartItems)
            {
                if (item.Game == null || item.Quantity <= 0)
                {
                    Console.WriteLine("Invalid item found in shopping cart.");
                    return false;
                }

                OwnerModel game = ownerRepo.SearchGame(item.Game.Name);

                if (game == null || game.Stock < item.Quantity)
                {
                    Console.WriteLine("Insufficient stock for: " + item.Game.Name);
                    return false;
                }
            }

            List<CartModel> updatedItems = new List<CartModel>();

            foreach (CartModel item in cartItems)
            {
                bool stockUpdated = ownerService.UpdateGameStock(
                    item.Game.Name,
                    item.Quantity
                );

                if (!stockUpdated)
                {
                    Console.WriteLine(
                        "Checkout failed. Could not update stock for: " + item.Game.Name
                    );

                    foreach (CartModel updatedItem in updatedItems)
                    {
                        ownerService.RestoreGameStock(
                            updatedItem.Game.Name,
                            updatedItem.Quantity
                        );
                    }

                    return false;
                }

                updatedItems.Add(item);
            }

            foreach (CartModel item in cartItems)
            {
                Sale sale = new Sale();

                sale.CustomerId = customer.CustomerId;
                sale.CustomerName = customer.Name;
                sale.GameName = item.Game.Name;
                sale.QuantitySold = item.Quantity;
                sale.SalePrice = item.Game.Price;
                sale.TotalAmount = item.Game.Price * item.Quantity;

                ownerService.AddSale(sale);
            }

            ownerService.SaveGamesToFile();
            ownerService.SaveSalesToFile();

            cartRepo.ClearCart();

            Console.WriteLine("\nPurchase completed successfully!");

            return true;
        }
        public float GetCartTotal()
        {
            float total = 0;

            foreach (CartModel item in cartRepo.GetCartItems())
            {
                total += item.Game.Price * item.Quantity;
            }

            return total;
        }
        public List<OwnerModel> FilterGamesByCategory(string category)
        {
            List<OwnerModel> filteredGames = new List<OwnerModel>();

            foreach (OwnerModel game in ownerRepo.GetAllGames())
            {
                if (game.Category.ToLower() == category.ToLower())
                {
                    filteredGames.Add(game);
                }
            }

            return filteredGames;
        }
        public OwnerModel SearchGame(string name)
        {
            return ownerRepo.SearchGame(name);
        }
        public bool Login(string username, string password)
        {
            CustomerModel customer = repo.FindCustomerByUsername(username);

            if (customer == null)
            {
                return false;
            }

            if (customer.Password == password)
            {
                return true;
            }

            return false;
        }
        public CustomerModel GetCustomerByUsername(string username)
        {
            return repo.FindCustomerByUsername(username);
        }
    }
}