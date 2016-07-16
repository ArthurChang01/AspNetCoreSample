using AspNetCore.Proxy.Responses.Auth;
using System.Threading.Tasks;

namespace AspNetCore.Proxy.Interfaces
{
    public interface IAuthProxy
    {
        Task<LogInResp> LogIn(string username, string password);
    }
}