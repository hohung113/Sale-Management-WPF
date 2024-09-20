namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        FstoreContext _context = new FstoreContext();

        //public Member GetMemberByEmail(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //    {
        //        throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        //    }
        //    var member = _context.Members.SingleOrDefault(m => m.Email == email) ?? throw new KeyNotFoundException($"No member found with email: {email}");
        //    return member;
        //}

        // For Admin Managed
        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                var listAllOrders = _context.Orders.ToList();
                if (listAllOrders.Any())
                {
                    listAllOrders.OrderBy(m => m.OrderId).ToList();
                }
                return listAllOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }

        public async Task AddNewOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            try
            {
               _context.Orders.Add(order);
               await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while add new order.", ex);
            }
        }

        public void UpdateOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            try
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while update the member.", ex);
            }
        }

        public IEnumerable<Order> GetAllOrderByMemberId(int id)
        {
            try
            {
                var listAllOrdersByMemberId = _context.Orders
                    .Where(o => o.OrderId.Equals(id))
                    .ToList();
                if (listAllOrdersByMemberId.Any())
                {
                    listAllOrdersByMemberId.OrderBy(m => m.OrderDate).ToList();
                }
                return listAllOrdersByMemberId;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }
        public void DeleteMember(Member member)
        {
            ArgumentNullException.ThrowIfNull(member, nameof(member));
            try
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while delete the member.", ex);
            }
        }
    }
}
