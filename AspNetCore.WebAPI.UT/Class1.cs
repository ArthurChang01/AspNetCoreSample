﻿using AspNetCore.Infra.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebAPI.UT
{
    public interface IMy
    {
        Task<string> DoAsync(string arg);
    }

    [TestFixture]
    public class Class
    {
        [Test]
        public async Task TestExpectedToPass2()
        {
            string output = "";
            var mock = new Mock<IMy>();
            mock.Setup(o => o.DoAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(output));
            IMy myObj = mock.Object;

            string actual = await myObj.DoAsync("");

            Assert.AreEqual(output, actual);
        }
    }
}