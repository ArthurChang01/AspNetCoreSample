using AspNetCore.IdentityServer.ViewModels.Auth;
using AspNetCore.Proxy.Interfaces;
using AspNetCore.Proxy.Responses.Auth;
using AutoMapper;
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

            Mapper.Initialize(
                cfg => cfg.CreateMap<LogInOut, LogInResp>()
            );
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="req">輸入參數</param>
        /// <returns>登入結果</returns>
        [HttpGet("LogIn")]
        [Produces(typeof(LogInResp))]
        public async Task<IActionResult> LogIn([FromQuery]LogInReq req)
        {
            LogInResp resp = null;

            try
            {
                var result = await this._proxy.LogIn(req.UserName, req.Password);
                resp = Mapper.Map<LogInOut, LogInResp>(result);
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