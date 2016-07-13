using AspNetCore.IdentityServer.Controllers;
using AspNetCore.IdentityServer.Models.Member;
using AspNetCore.Infra.Auth.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IMemberRepository Repository { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string conString = this.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MemberDbContext>(opt => opt.UseSqlServer(conString));

            // Add framework services.
            services.AddMvc();

            services.AddScoped<IMemberRepository, MemberRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemberRepository rpt)
        {
            this.Repository = rpt;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            ConfigureAuth(app);

            app.UseMvc();
        }

        // The secret key every token will be signed with.
        // Keep this safe on the server!
        private static readonly string secretKey = "mysupersecret_secretkey!123";

        private void ConfigureAuth(IApplicationBuilder app)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            app.UseSimpleTokenProvider(new CustomJwtTokenOption
            {
                Path = "/api/token",
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                Expiration = TimeSpan.FromMinutes(30),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            ClaimsIdentity ci = new ClaimsIdentity("Jwt");

            Account act = await this.Repository.LogIn(username, password);
            if (act == null)
                return null;

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, act.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, act.User.Name));
            ci.AddClaim(new Claim("sub", "Sheng618"));

            return ci;
        }
    }
}