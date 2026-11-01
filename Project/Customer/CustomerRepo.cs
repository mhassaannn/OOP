using Project.Owner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CustomerRepo
    {
        public List<CustomerModel> Customers = new List<CustomerModel>();

        private string filePath = "customers.txt";

        public void AddCustomer(CustomerModel customer)
        {
            Customers.Add(customer);
        }
        public bool CustomerIdExists(int customerId)
        {
            foreach (CustomerModel customer in Customers)
            {
                if (customer.CustomerId == customerId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UsernameExists(string username)
        {
            foreach (CustomerModel customer in Customers)
            {
                if (string.Equals(
                    customer.Username,
                    username,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public List<CustomerModel> GetAllCustomers()
        {
            return Customers;
        }

        public CustomerModel FindCustomerByUsername(string username)
        {
            foreach (CustomerModel customer in Customers)
            {
                if (customer.Username.ToLower() == username.ToLower())
                {
                    return customer;
                }
            }

            return null;
        }

        public void SaveCustomersToFile()
        {
            using (StreamWriter writer = new StreamWriter("customers.txt"))
            {
                foreach (CustomerModel customer in Customers)
                {
                    writer.WriteLine(
                        customer.CustomerId + "|" +
                        customer.Name + "|" +
                        customer.Username + "|" +
                        customer.Password
                    );
                }
            }

            Console.WriteLine("\nCustomers saved successfully!");
        }

        public void LoadCustomersFromFile()
        {
            if (!File.Exists("customers.txt"))
            {
                Console.WriteLine("No saved customer file found.");
                return;
            }

            List<CustomerModel> loadedCustomers = new List<CustomerModel>();

            using (StreamReader reader = new StreamReader("customers.txt"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split('|');

                    if (data.Length != 4)
                    {
                        Console.WriteLine("Skipping invalid customer record.");
                        continue;
                    }

                    int customerId;

                    if (!int.TryParse(data[0], out customerId) ||
                        customerId <= 0 ||
                        string.IsNullOrWhiteSpace(data[1]) ||
                        string.IsNullOrWhiteSpace(data[2]) ||
                        string.IsNullOrWhiteSpace(data[3]))
                    {
                        Console.WriteLine("Skipping invalid customer record.");
                        continue;
                    }

                    CustomerModel customer = new CustomerModel();

                    customer.CustomerId = customerId;
                    customer.Name = data[1];
                    customer.Username = data[2];
                    customer.Password = data[3];

                    loadedCustomers.Add(customer);
                }
            }

            Customers.Clear();
            Customers.AddRange(loadedCustomers);

            Console.WriteLine("Customers loaded successfully!");
        }
    }
}