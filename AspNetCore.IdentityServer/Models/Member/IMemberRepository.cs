using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.Models.Member
{
    public interface IMemberRepository
    {
        Task<Account> LogIn(string account, string password);
    }
}