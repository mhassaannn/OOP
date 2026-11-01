using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
    }
}
