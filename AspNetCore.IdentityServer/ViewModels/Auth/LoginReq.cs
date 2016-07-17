using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.ViewModels.Auth
{
    /// <summary>
    /// Request - LogIn
    /// </summary>
    public class LogInReq
    {
        /// <summary>
        /// 使用者帳戶
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
    }
}