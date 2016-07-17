using AspNetCore.Proxy.Responses.Auth;
using System.Threading.Tasks;

namespace AspNetCore.Proxy.Interfaces
{
    public interface IAuthProxy
    {
        Task<LogInOut> LogIn(string username, string password);
    }
}