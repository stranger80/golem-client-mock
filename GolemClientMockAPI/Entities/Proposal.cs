using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GolemClientMockAPI.Entities
{
    public class Proposal
    {
        /// <summary>
        /// Proposal Id (as perceived by Proposal receiver)
        /// </summary>
        public string Id { get; set; }

        public int InternalId { get; set; }

        /// <summary>
        /// SubscriptionId of the sender (required to identify the sending pipeline)
        /// </summary>
        public string SendingSubscriptionId { get; set; }

        /// <summary>
        /// SubscriptionId of the receiver (required to identify the receiving pipeline)
        /// </summary>
        public string ReceivingSubscriptionId { get; set; }

        public StateEnum State { get; set; }

        /// <summary>
        /// * `Initial` - proposal arrived from the market as response to subscription
        /// * `Draft` - bespoke counter-proposal issued by one party directly to other party (negotiation phase)
        /// * `Rejected` by other party
        /// * `Accepted` - promoted into the Agreement draft
        /// * `Expired` - not accepted nor rejected before validity period 
        /// </summary>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum StateEnum
        {
            [EnumMember(Value = "Initial")]
            InitialEnum = 0,
            [EnumMember(Value = "Draft")]
            DraftEnum = 1,
            [EnumMember(Value = "Rejected")]
            RejectedEnum = 2,
            [EnumMember(Value = "Accepted")]
            AcceptedEnum = 3,
            [EnumMember(Value = "Expired")]
            ExpiredEnum = 4
        }
    }
}
