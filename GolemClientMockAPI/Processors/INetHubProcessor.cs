using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface INetHubProcessor
    {
        void RegisterNode(string nodeId);

        void DeregisterNode(string nodeId);

        bool IsNodeRegistered(string nodeId);

        void SendMessage(NetMessage message);

        Task<ICollection<NetMessage>> CollectMessagesAsync(string collectingNodeId, int timeout);
    }
}
