using AspNetCore.IdentityServer.Models.Member.Entities;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.Models.Member.Repository
{
    public interface IMemberRepository
    {
        Task<Account> LogIn(string account, string password);
    }
}