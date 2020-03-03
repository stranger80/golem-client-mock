using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Repository;
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
    public class NetHubIntegrationTests
    {
        public INetHubProcessor NetHubProcessor { get; set; }

        [TestInitialize]
        public void InitializeTests()
        {
            this.NetHubProcessor = new InMemoryNetHubProcessor();
        }


        [TestMethod]
        public async Task InMemoryNetHubProcessor_Integration_RegistrationPositiveFlow()
        {
            var nodeId = "TestNodeId";

            Assert.IsFalse(this.NetHubProcessor.IsNodeRegistered(nodeId));

            this.NetHubProcessor.RegisterNode(nodeId);

            Assert.IsTrue(this.NetHubProcessor.IsNodeRegistered(nodeId));

            // prevent double registering (FOR CONSIDERATION)
            try
            {
                this.NetHubProcessor.RegisterNode(nodeId);

                Assert.Fail("Shouldn't allow to register the same node twice!");
            }
            catch(Exception exc)
            {

            }

            this.NetHubProcessor.DeregisterNode(nodeId);

            Assert.IsFalse(this.NetHubProcessor.IsNodeRegistered(nodeId));
        }

        [TestMethod]
        public async Task InMemoryNetHubProcessor_Integration_SendReceivePositiveFlow()
        {
            var nodeId = "TestNodeId";
            var nodeId2 = "TestNodeId2";

            this.NetHubProcessor.RegisterNode(nodeId);
            this.NetHubProcessor.RegisterNode(nodeId2);

            var message = new NetMessage()
            {
                RequestId = "RequestId",
                SenderNodeId = nodeId,
                Payload = "Payload",
                DestinationAddress = $"//net/{nodeId2}"
            };

            this.NetHubProcessor.SendMessage(message);


            var receivedMessages = await this.NetHubProcessor.CollectMessagesAsync(nodeId2, 1.0f);

            Assert.IsNotNull(receivedMessages);
            Assert.IsTrue(receivedMessages.Count == 1);
            Assert.AreEqual(message.RequestId, receivedMessages.First().RequestId);


            this.NetHubProcessor.DeregisterNode(nodeId);
            this.NetHubProcessor.DeregisterNode(nodeId2);
        }



    }
}
