using AspNetCore.Infra.Rest;
using AspNetCore.Proxy.Options;
using AspNetCore.Proxy.Proxies;
using AspNetCore.Proxy.Responses.Auth;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.Proxy.UT
{
    [TestFixture]
    public class AuthProxyUT
    {
        private Mock<IRestContext> mockRest = null;
        private IOptions<AutOptions> authOption = null;

        [SetUp]
        public void Init()
        {
            this.mockRest = new Mock<IRestContext>();
            this.mockRest.Setup(o => o.SetQueryString(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockRest.Object);

            var mockAuthOption = new Mock<IOptions<AutOptions>>();
            mockAuthOption.Setup(o => o.Value).Returns(new AutOptions() { IdentityServerUrl = "" });
            this.authOption = mockAuthOption.Object;
        }

        [Test]
        public async Task LogIn()
        {
            //Arrange
            string username = "a@b.c",
                     password = "1234",
                     access_token = Guid.NewGuid().ToString();
            LogInResp expect = new LogInResp { AccessToken = access_token };

            this.mockRest
                .Setup(o => o.Get<LogInResp>(It.IsAny<string>()))
                .Returns(Task.FromResult<LogInResp>(new LogInResp { AccessToken = access_token }));
            AuthProxy proxy = new AuthProxy(this.mockRest.Object, authOption);

            //Act
            LogInResp actual = await proxy.LogIn(username, password);

            //Assert
            Assert.AreEqual(expect.AccessToken, actual.AccessToken);
        }
    }
}