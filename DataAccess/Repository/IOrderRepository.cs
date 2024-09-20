using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        List<CartItem> CartItems { get; set; }
        public Task AddNewOrder(Order order);
        public void AddToCart(CartItem item);
        public void ClearCartItem();
    }
}
