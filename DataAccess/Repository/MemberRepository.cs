
namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private bool _isAdmin;
        public Member CurrentMember { get; private set; }

        public bool LoginAdmin(string username, string password)
        {
            bool result = false;
            if(username.Trim().Equals(HelperAppsettings.GetString("EmailAdmin")) && password.Equals(HelperAppsettings.GetString("PassAdmin"))){
                result = true;
                this._isAdmin = true;
            }
            else
            {
                this._isAdmin = false;
            }
            
            return result;
        }

        public bool IsAdmin()
        {
           return _isAdmin;
        }

        public bool IsLoggedIn() => CurrentMember != null;

        public async Task<bool> Login(string email, string password)
        {
            bool result = false;
            CurrentMember = await MemberDAO.Instance.GetMemberByEmail(email);
            if (CurrentMember != null && CurrentMember.Password.Equals(password))
            {
                result = true;
            }
            return result;
        }

        public void Logout()
        {
            CurrentMember = null;
        }

        public void UpdateMember(Member member)
        {
             MemberDAO.Instance.UpdateMember(member);
        }

        public void AddMemeber(Member member)
        {
            MemberDAO.Instance.AddMember(member);
        }
    }
}