namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }
        FstoreContext _context = new FstoreContext();


        public void AddOrderDetail(OrderDetail orderDetail)
        {
            ArgumentNullException.ThrowIfNull(orderDetail, nameof(orderDetail));
            try
            {
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the orderdetail.", ex);
            }
        }

        public List<OrderDetail> GetAllOrders()
        {
            var result = _context.OrderDetails
            .Include(o => o.Order)
            .Include(p => p.Product)
            .OrderBy(p => p.OrderId)
            .ToList();
            return result;
        }

        public List<OrderDetail> GetAllOrdersDetailByMeber()
        {
            var result = _context.OrderDetails
            .Include(o => o.Order)
            .Include(p => p.Product)
            .OrderBy(p => p.OrderId)
            .ToList();
            return result;
        }
    }
}
