using Project.Owner;
using Project.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CartModel
    {
        public OwnerModel Game { get; set; }
        public int Quantity { get; set; }
    }
}