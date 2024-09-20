using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        Member CurrentMember { get; }
        bool IsLoggedIn();
        Task<bool> Login(string email, string password);
        bool LoginAdmin(string username, string password);
        bool IsAdmin();
        void Logout();
        void UpdateMember(Member member);
        void AddMemeber(Member member);
    }
}