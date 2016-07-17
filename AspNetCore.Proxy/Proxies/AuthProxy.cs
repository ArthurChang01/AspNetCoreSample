using AspNetCore.Infra.Rest;
using AspNetCore.Proxy.Interfaces;
using AspNetCore.Proxy.Options;
using AspNetCore.Proxy.Responses.Auth;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Proxy.Proxies
{
    public class AuthProxy : IAuthProxy
    {
        private IRestContext _client = null;

        public AuthProxy(IRestContext client, IOptions<AutOptions> option)
        {
            this._client = client;
            this._client.SetBaseAddress(option.Value.IdentityServerUrl);
        }

        public async Task<LogInOut> LogIn(string username, string password)
        {
            LogInOut resp = null;

            try
            {
                resp = await this._client
                    .SetQueryString("username", username)
                    .SetQueryString("password", password)
                    .Get<LogInOut>("api/token");
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }
    }
}