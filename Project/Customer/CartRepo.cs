using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Customer
{
    public class CartRepo
    {
        private Dictionary<int, List<CartModel>> customerCarts =
            new Dictionary<int, List<CartModel>>();

        private int activeCustomerId = 0;

        public void SetActiveCustomer(int customerId)
        {
            activeCustomerId = customerId;

            if (!customerCarts.ContainsKey(customerId))
            {
                customerCarts.Add(customerId, new List<CartModel>());
            }
        }

        private List<CartModel> GetActiveCart()
        {
            if (activeCustomerId == 0)
            {
                return new List<CartModel>();
            }

            if (!customerCarts.ContainsKey(activeCustomerId))
            {
                customerCarts.Add(
                    activeCustomerId,
                    new List<CartModel>()
                );
            }

            return customerCarts[activeCustomerId];
        }

        public void AddToCart(CartModel item)
        {
            GetActiveCart().Add(item);
        }

        public List<CartModel> GetCartItems()
        {
            return GetActiveCart();
        }

        public void ClearCart()
        {
            GetActiveCart().Clear();
        }

        public bool RemoveFromCart(string gameName)
        {
            List<CartModel> cartItems = GetActiveCart();

            for (int i = 0; i < cartItems.Count; i++)
            {
                if (cartItems[i].Game.Name.ToLower() == gameName.ToLower())
                {
                    cartItems.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool UpdateCartQuantity(string gameName, int newQuantity)
        {
            foreach (CartModel item in GetActiveCart())
            {
                if (item.Game.Name.ToLower() == gameName.ToLower())
                {
                    item.Quantity = newQuantity;
                    return true;
                }
            }

            return false;
        }
    }
}
