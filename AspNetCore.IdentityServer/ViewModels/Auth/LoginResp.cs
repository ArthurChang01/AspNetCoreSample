using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.ViewModels.Auth
{
    /// <summary>
    /// Response - Login
    /// </summary>
    public class LogInResp
    {
        /// <summary>
        /// 存取令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 過期時間(分)
        /// </summary>
        public int ExpiredMinute { get; set; }
    }
}