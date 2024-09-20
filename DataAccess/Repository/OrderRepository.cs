using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public List<CartItem> CartItems {  get; set; } = new List<CartItem>();

        public async Task AddNewOrder(Order order) => await OrderDAO.Instance.AddNewOrder(order);

        public void AddToCart(CartItem item) => CartItems.Add(item);

        public void ClearCartItem() => CartItems.Clear();
    }
}