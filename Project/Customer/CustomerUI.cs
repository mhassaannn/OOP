using Project.Customer;
using Project.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CustomerUI
    {
        private CustomerService service;

        public CustomerUI(OwnerRepo repository)
        {
            service = new CustomerService(repository);
        }
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== CUSTOMER MENU ===\n");
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. Customer Login");
                Console.WriteLine("3. View Registered Customers");
                Console.WriteLine("4. Browse Games");
                Console.WriteLine("5. Search Games");
                Console.WriteLine("6. Filter Games by Category");
                Console.WriteLine("7. Add Game to Cart");
                Console.WriteLine("8. View Shopping Cart");
                Console.WriteLine("9. Remove Game from Cart");
                Console.WriteLine("10. Update Cart Quantity");
                Console.WriteLine("11. Purchase Games");
                Console.WriteLine("12. Purchase History");
                Console.WriteLine("13. Exit / Logout");

                Console.Write("\nEnter your choice: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                    Pause();
                    continue;
                }

                if (choice == 1)
                {
                    RegisterCustomer();
                    Pause();
                }
                else if (choice == 2)
                {
                    CustomerLogin();
                    Pause();
                }
                else if (choice == 3)
                {
                    ViewCustomers();
                    Pause();
                }
                else if (choice == 4)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        BrowseGames();
                        Pause();
                    }
                }
                else if (choice == 5)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        SearchGames();
                        Pause();
                    }
                }
                else if (choice == 6)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        FilterGames();
                        Pause();
                    }
                }
                else if (choice == 7)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        AddGameToCart();
                        Pause();
                    }
                }
                else if (choice == 8)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        ViewCart();
                        Pause();
                    }
                }
                else if (choice == 9)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        RemoveGameFromCart();
                        Pause();
                    }
                }
                else if (choice == 10)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        UpdateCartItem();
                        Pause();
                    }
                }
                else if (choice == 11)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        PurchaseGames();
                        Pause();
                    }
                }
                else if (choice == 12)
                {
                    if (!isCustomerLoggedIn)
                    {
                        Console.WriteLine("\nPlease login first to access shopping features.");
                        Pause();
                    }
                    else
                    {
                        ViewPurchaseHistory();
                        Pause();
                    }
                }
                else if (choice == 13)
                {
                    if (isCustomerLoggedIn)
                    {
                        service.ClearActiveCustomer();
                        loggedInCustomer = null;
                        isCustomerLoggedIn = false;

                        Console.WriteLine("\nYou have been logged out successfully.");
                    }

                    Console.WriteLine("\nExiting customer menu...");
                    return;
                }

            }
        }

        private void RegisterCustomer()
        {
            Console.Clear();

            Console.WriteLine("===== CUSTOMER REGISTRATION =====\n");

            CustomerModel customer = new CustomerModel();

            Console.Write("Enter Customer ID: ");

            int customerId;

            if (!int.TryParse(Console.ReadLine(), out customerId) ||
                customerId <= 0)
            {
                Console.WriteLine("\nPlease enter a valid positive Customer ID.");
                return;
            }

            customer.CustomerId = customerId;

            Console.Write("Enter Name: ");
            customer.Name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                Console.WriteLine("\nName cannot be empty.");
                return;
            }

            Console.Write("Enter Username: ");
            customer.Username = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(customer.Username))
            {
                Console.WriteLine("\nUsername cannot be empty.");
                return;
            }

            Console.Write("Enter Password: ");
            customer.Password = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                Console.WriteLine("\nPassword cannot be empty.");
                return;
            }

            service.AddCustomer(customer);
        }
        private bool isCustomerLoggedIn = false;
        private CustomerModel loggedInCustomer = new CustomerModel();
        private void CustomerLogin()
        {
            Console.Clear();

            if (isCustomerLoggedIn)
            {
                Console.WriteLine("A customer is already logged in.");
                Console.WriteLine("Please logout before logging into another account.");
                return;
            }

            Console.WriteLine("===== CUSTOMER LOGIN =====\n");

            Console.Write("Enter Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Enter Password: ");
            string password = Console.ReadLine() ?? "";

            bool isLoggedIn = service.Login(username, password);

            if (isLoggedIn)
            {
                loggedInCustomer = service.GetCustomerByUsername(username);

                isCustomerLoggedIn = true;

                service.SetActiveCustomer(loggedInCustomer.CustomerId);

                Console.WriteLine("\nLogin successful!");
                Console.WriteLine("Welcome, " + loggedInCustomer.Name + "!");
            }
            else
            {
                Console.WriteLine("\nInvalid username or password.");
            }
        }

        private void BrowseGames()
        {
            Console.Clear();

            Console.WriteLine("===== AVAILABLE GAMES =====");

            List<OwnerModel> games = service.GetAvailableGames();

            if (games.Count == 0)
            {
                Console.WriteLine("\nNo games are currently available.");
                return;
            }

            foreach (OwnerModel game in games)
            {
                Console.WriteLine("\nGame Name: " + game.Name);
                Console.WriteLine("Price: PKR " + game.Price);
                Console.WriteLine("Available Stock: " + game.Stock);
                Console.WriteLine("Category: " + game.Category);
                Console.WriteLine("-------------------------");
            }
        }
        private void SearchGames()
        {
            Console.Clear();

            Console.WriteLine("===== SEARCH GAMES =====");

            Console.Write("Enter Game Name: ");
            string name = Console.ReadLine() ?? "";

            OwnerModel game = service.SearchGame(name);

            if (game == null)
            {
                Console.WriteLine("\nGame not found!");
            }
            else
            {
                Console.WriteLine("\nGame Found!");
                Console.WriteLine("Game Name: " + game.Name);
                Console.WriteLine("Price: PKR " + game.Price);
                Console.WriteLine("Available Stock: " + game.Stock);
                Console.WriteLine("Category: " + game.Category);
            }
        }
        private void ViewCustomers()
        {
            Console.Clear();

            Console.WriteLine("===REGISTERED CUSTOMERS===\n");

            List<CustomerModel> customers = service.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("\nNo customers registered.");
                return;
            }

            foreach (CustomerModel customer in customers)
            {
                Console.WriteLine("\nCustomer ID: " + customer.CustomerId);
                Console.WriteLine("Name: " + customer.Name);
                Console.WriteLine("Username: " + customer.Username);
                Console.WriteLine("-------------------------");
            }
        }
        private void FilterGames()
        {
            Console.Clear();

            Console.WriteLine("===== FILTER GAMES BY CATEGORY =====");

            Console.Write("Enter Category: ");
            string category = Console.ReadLine() ?? "";

            List<OwnerModel> games = service.FilterGamesByCategory(category);

            if (games.Count == 0)
            {
                Console.WriteLine("\nNo games found in this category.");
                return;
            }

            foreach (OwnerModel game in games)
            {
                Console.WriteLine("\nGame Name: " + game.Name);
                Console.WriteLine("Price: PKR " + game.Price);
                Console.WriteLine("Available Stock: " + game.Stock);
                Console.WriteLine("Category: " + game.Category);
                Console.WriteLine("-------------------------");
            }
        }

        private void AddGameToCart()
        {
            Console.Clear();

            Console.WriteLine("===== ADD GAME TO CART =====\n");

            Console.Write("Enter Game Name: ");
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

            Console.WriteLine("\nGame Found: " + game.Name);
            Console.WriteLine("Price: PKR " + game.Price);
            Console.WriteLine("Available Stock: " + game.Stock);

            Console.Write("\nEnter Quantity: ");

            int quantity;

            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("\nPlease enter a valid quantity greater than zero.");
                return;
            }

            bool isAdded = service.AddToCart(game, quantity);

            if (isAdded)
            {
                Console.WriteLine("\nGame added to cart successfully!");
            }
            else
            {
                Console.WriteLine("\nUnable to add game to cart.");
            }
        }

        private void ViewCart()
        {
            Console.Clear();

            Console.WriteLine("===== SHOPPING CART =====");

            List<CartModel> cartItems = service.GetCartItems();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("\nYour shopping cart is empty.");
                return;
            }

            foreach (CartModel item in cartItems)
            {
                Console.WriteLine("\nGame Name: " + item.Game.Name);
                Console.WriteLine("Price: PKR " + item.Game.Price);
                Console.WriteLine("Quantity: " + item.Quantity);
                Console.WriteLine("Subtotal: PKR " +
                    (item.Game.Price * item.Quantity));

                Console.WriteLine("-------------------------");
            }
            float total = service.GetCartTotal();

            Console.WriteLine("\n=========================");
            Console.WriteLine("Total Cart Amount: PKR " + total);
            Console.WriteLine("=========================");
        }
        private void RemoveGameFromCart()
        {
            Console.Clear();

            Console.WriteLine("===== REMOVE GAME FROM CART =====");

            Console.Write("Enter Game Name: ");
            string gameName = Console.ReadLine() ?? "";

            bool isRemoved = service.RemoveFromCart(gameName);

            if (isRemoved)
            {
                Console.WriteLine("\nGame removed from your cart successfully!");
            }
            else
            {
                Console.WriteLine("\nGame not found in your cart.");
            }
        }
        private void UpdateCartItem()
        {
            Console.Clear();

            Console.WriteLine("===== UPDATE CART QUANTITY =====");

            Console.Write("\nEnter Game Name: ");
            string gameName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(gameName))
            {
                Console.WriteLine("\nGame name cannot be empty.");
                return;
            }

            Console.Write("Enter New Quantity: ");

            int newQuantity;

            if (!int.TryParse(Console.ReadLine(), out newQuantity) ||
                newQuantity <= 0)
            {
                Console.WriteLine("\nPlease enter a valid quantity greater than zero.");
                return;
            }

            bool isUpdated = service.UpdateCartQuantity(gameName, newQuantity);

            if (isUpdated)
            {
                Console.WriteLine("\nCart quantity updated successfully!");
            }
            else
            {
                Console.WriteLine("\nUnable to update cart quantity.");
            }
        }
        private void PurchaseGames()
        {
            Console.Clear();

            Console.WriteLine("===== CHECKOUT =====");

            Console.Write("Are you sure you want to complete your purchase? (Y/N): ");
            string confirmation = Console.ReadLine() ?? "";

            if (confirmation.ToLower() != "y")
            {
                Console.WriteLine("\nPurchase cancelled.");
                return;
            }

            bool isPurchased = service.Checkout(loggedInCustomer);

            if (isPurchased)
            {
                Console.WriteLine("\nPurchase completed successfully!");
                Console.WriteLine("Thank you for shopping with us.");
            }
            else
            {
                Console.WriteLine("\nPurchase could not be completed.");
            }
        }
        private void ViewPurchaseHistory()
        {
            Console.Clear();

            Console.WriteLine("===== MY PURCHASE HISTORY =====");

            List<Sale> sales = service.GetCustomerPurchaseHistory(
                loggedInCustomer.CustomerId
            );

            if (sales.Count == 0)
            {
                Console.WriteLine("\nYou have no previous purchases.");
                return;
            }

            foreach (Sale sale in sales)
            {
                Console.WriteLine("\nGame Name: " + sale.GameName);
                Console.WriteLine("Quantity Purchased: " + sale.QuantitySold);
                Console.WriteLine("Price Per Game: PKR " + sale.SalePrice);
                Console.WriteLine("Total Amount: PKR " + sale.TotalAmount);
                Console.WriteLine("-------------------------");
            }
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}