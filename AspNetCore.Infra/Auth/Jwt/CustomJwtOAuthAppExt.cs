﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Auth.Jwt
{
    public static class CustomJwtOAuthAppExt
    {
        /// <summary>
        /// Adds the <see cref="TokenProviderMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables token generation capabilities.
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A  <see cref="CustomJwtTokenOption"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSimpleTokenProvider(this IApplicationBuilder app, CustomJwtTokenOption options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<CustomJwtOAuthProviderMiddleware>(Options.Create(options));
        }
    }
}