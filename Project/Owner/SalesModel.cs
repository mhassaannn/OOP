using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Owner
{
    public class Sale
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";


        public string GameName { get; set; } = "";

        public int QuantitySold { get; set; }

        public float SalePrice { get; set; }

        public float TotalAmount { get; set; }

    }
}
