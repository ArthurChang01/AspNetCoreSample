using AspNetCore.IdentityServer.Models.Member;
using AspNetCore.IdentityServer.Models.Member.Repository;
using AspNetCore.Infra.Rest;
using AspNetCore.Proxy.Interfaces;
using AspNetCore.Proxy.Options;
using AspNetCore.Proxy.Proxies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using System.IO;

namespace AspNetCore.IdentityServer
{
    public partial class Startup
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

            services.AddOptions();
            services.Configure<AutOptions>(Configuration);

            // Add framework services.
            services.AddMvc();

            var pathToDoc = $"{Directory.GetCurrentDirectory()}{ Configuration["Swagger:Path"]}";

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(opt =>
            {
                opt.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "AspNetCoreSample",
                    Description = "A simple ASP.Net Core WebAPI smaple",
                    TermsOfService = "ASP.Net Core"
                });
                opt.IncludeXmlComments(pathToDoc);
                opt.DescribeAllEnumsAsStrings();
            });

            services.AddTransient<IRestContext, RestContext>();
            services.AddTransient<IAuthProxy, AuthProxy>();
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

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}