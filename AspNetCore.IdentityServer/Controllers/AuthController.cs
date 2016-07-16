using AspNetCore.Proxy.Interfaces;
using AspNetCore.Proxy.Responses.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCore.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private IAuthProxy _proxy = null;

        public AuthController(IAuthProxy proxy)
        {
            this._proxy = proxy;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="username">帳戶(Email)</param>
        /// <param name="password">密碼</param>
        /// <returns>登入結果</returns>
        [HttpGet("LogIn/{username}/{password}")]
        [Produces(typeof(LogInResp))]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            LogInResp resp = null;

            try
            {
                resp = await this._proxy.LogIn(username, password);
            }
            catch (Exception ex)
            {
                ex = ex.InnerException ?? ex;
                return this.StatusCode(500, ex);
            }

            return Ok(resp);
        }
    }
}