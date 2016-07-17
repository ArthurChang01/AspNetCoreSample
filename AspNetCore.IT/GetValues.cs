using AspNetCore.IdentityServer;
using AspNetCore.IdentityServer.ViewModels.Auth;
using AspNetCore.Proxy.Responses.Auth;
using AspNetCore.WebAPI;
using AspNetCore.WebAPI.Controllers;
using AspNetCore.WebAPI.ViewModels.Values;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.IT
{
    [TestFixture]
    public class GetValues
    {
        private TestServer _IdServer;
        private TestServer _ApiServer;
        private HttpClient _IdClient;
        private HttpClient _ApiClient;

        [SetUp]
        public void Init()
        {
            _IdServer = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<IdentityServer.Startup>());

            _ApiServer = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<WebAPI.Startup>());

            _IdClient = _IdServer.CreateClient();
            _ApiClient = _ApiServer.CreateClient();
        }

        [Test]
        public async Task IT_GetValues()
        {
            var resp = await _IdClient.GetAsync("/api/token?username=a@b.c&password=1234");
            resp.EnsureSuccessStatusCode();

            LogInOut result = await resp.Content.ReadAsStringAsync()
                .ContinueWith<LogInOut>(o => JsonConvert.DeserializeObject<LogInOut>(o.Result));
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "api/Values");
            req.Headers.Add("Authorization", "Bearer " + result.AccessToken);

            resp = await _ApiClient.SendAsync(req);
            Dto result2 = await resp.Content.ReadAsStringAsync()
                .ContinueWith<Dto>(o => JsonConvert.DeserializeObject<Dto>(o.Result));
        }
    }
}