using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public async Task AddNewOrderDetail(OrderDetail orderDetail)
        {
            OrderDetailDAO.Instance.AddOrderDetail(orderDetail);
        }

        public List<OrderDetail> GetAllOrderDetailByMember()
        {
            return OrderDetailDAO.Instance.GetAllOrdersDetailByMeber();
        }


        public List<OrderDetail> GetAllOrdersDetail()
        {
            return OrderDetailDAO.Instance.GetAllOrders();
        }
    }
}
