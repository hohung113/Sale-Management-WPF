using System.Linq.Expressions;

namespace DataAccess
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }
        FstoreContext _context = new FstoreContext();

        public async Task<Member> GetMemberByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            //var member = _context.Members.Include(item => item.Orders).ThenInclude(item => item.OrderDetails).SingleOrDefault(m => m.Email == email) ?? throw new KeyNotFoundException($"No member found with email: {email}");
            var member = await _context.Members.SingleOrDefaultAsync(m => m.Email == email);

            return member;
        }

        public IEnumerable<Member> GetMembers()
        {
            var listMembers = _context.Members.ToList();
            return listMembers.Any() ? listMembers : Enumerable.Empty<Member>();
        }

        public void AddMember(Member member)
        {
            ArgumentNullException.ThrowIfNull(member, nameof(member));
            try
            {
                _context.Members.Add(member);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the member.", ex);
            }
        }

        public async void UpdateMember(Member member)
        {
            ArgumentNullException.ThrowIfNull(member, nameof(member));
            try
            {
                // context.Entry<Member>>(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while update the member.", ex);
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
