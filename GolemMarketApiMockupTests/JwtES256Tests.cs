using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Repository;
using GolemClientMockAPI.Security;
using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class JwtES256Tests
    {

        [TestMethod]
        public async Task Jwt256Builder_TokenCreateAndVerifySuccessful()
        {
            const string privateKey = "c711e5080f2b58260fe19741a7913e8301c1128ec8e80b8009406e5047e6e1ef";
            const string publicKey = "04e33993f0210a4973a94c26667007d1b56fe886e8b3c2afdd66aa9e4937478ad20acfbdc666e3cec3510ce85d40365fc2045e5adb7e675198cf57c6638efa1bdb";

            var builder = new JwtES256Builder();

            var jwt = builder.CreateToken(new Dictionary<string, string>() { { "iss", "TestNodeId" }, { "aud", "GolemNetHub" }, { "sub", "TestNodeId" } }, 
                privateKey, 
                publicKey);

            var isValid = builder.ValidateToken(jwt, new Dictionary<string, string>() { { "iss", "TestNodeId" }, { "aud", "GolemNetHub" }, { "sub", "TestNodeId" } }, 
                publicKey);

            Assert.IsTrue(isValid);

        }

    }
}
