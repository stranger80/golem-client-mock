using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Processors
{
    public class InMemoryNetHubProcessor : INetHubProcessor
    {
        public const string NET_ADDRESS_PREFIX = "//net/";

        public class NodeDescriptor
        {
            public string NodeId { get; set; }
            public DateTime RegistrationDateTime { get; set; }
            public BlockingCollection<NetMessage> IncomingMessageQueue { get; set; }
        }


        /// <summary>
        /// Dictionary of NodeDescriptors indexed by NodeIds
        /// </summary>
        public IDictionary<string, NodeDescriptor> NodeDescriptorsByNodeIds { get; set; } = new Dictionary<string, NodeDescriptor>();


        public Task<ICollection<NetMessage>> CollectMessagesAsync(string collectingNodeId, float timeout)
        {
            if (!this.NodeDescriptorsByNodeIds.ContainsKey(collectingNodeId))
                throw new Exception($"Node [{collectingNodeId}] not registered!");

            var incomingQueue = this.NodeDescriptorsByNodeIds[collectingNodeId].IncomingMessageQueue;


            var resultTask = Task.Run(() =>
            {
                var result = new List<NetMessage>();

                NetMessage message;
                if (incomingQueue.TryTake(out message, (int)(float)(timeout * 1000)))
                {
                    result.Add(message);
                }


                return result as ICollection<NetMessage>;
            });


            return resultTask;
        }

        public void DeregisterNode(string nodeId)
        {
            if (!this.NodeDescriptorsByNodeIds.ContainsKey(nodeId))
                throw new Exception($"Node [{nodeId}] not registered!");

            this.NodeDescriptorsByNodeIds.Remove(nodeId);
        }

        public bool IsNodeRegistered(string nodeId)
        {
            return this.NodeDescriptorsByNodeIds.ContainsKey(nodeId);
        }

        public void RegisterNode(string nodeId)
        {
            if(this.NodeDescriptorsByNodeIds.ContainsKey(nodeId))
                throw new Exception($"Node [{nodeId}] already registered!");

            this.NodeDescriptorsByNodeIds.Add(nodeId,
                new NodeDescriptor()
                {
                    NodeId = nodeId,
                    RegistrationDateTime = DateTime.Now,
                    IncomingMessageQueue = new BlockingCollection<NetMessage>()
                });
        }

        public void SendMessage(NetMessage message)
        {
            var destNodes = GetAddressRouting(message.DestinationAddress);

            foreach(var destNode in destNodes)
            {
                if (this.NodeDescriptorsByNodeIds.ContainsKey(destNode))
                {
                    this.NodeDescriptorsByNodeIds[destNode].IncomingMessageQueue.Add(message);
                }
                else
                {
                    /// TODO decide what to do here - as we don't want to break multicast ending when one node disappeared... 
                }
            }
        }


        /// <summary>
        /// Resolve all destination nodes based on destination address.
        /// NOTE: This would also include multicasting logic, if required.
        /// </summary>
        /// <param name="destinationAddress"></param>
        /// <returns></returns>
        ICollection<string> GetAddressRouting(string destinationAddress)
        {
            var result = new List<string>();

            if(!destinationAddress.StartsWith(NET_ADDRESS_PREFIX))
            {
                throw new Exception($"Destination Address does not start with {NET_ADDRESS_PREFIX}!");
            }

            var addr = destinationAddress.Substring(NET_ADDRESS_PREFIX.Length);
            var nodes = addr.Split("/");

            // TODO implement multicast matching here

            // 

            if (this.NodeDescriptorsByNodeIds.ContainsKey(nodes[0]))
                result.Add(nodes[0]);

            return result;
        }

    }
}
