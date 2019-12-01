using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class NetMessage
    {
        public string SenderNodeId { get; set; }

        public string DestinationAddress { get; set; }

        public string RequestId { get; set; }

        public string Payload { get; set; }
    }
}
