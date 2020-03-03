using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GolemClientMockAPI.Entities.MarketProviderEvent;
using static GolemClientMockAPI.Entities.MarketRequestorEvent;

namespace GolemClientMockAPI.Mappers
{
    public static class EnumMappers
    {
        public static string TranslateRequestorEventType(MarketRequestorEventType eventType)
        {
            switch(eventType)
            {
                case MarketRequestorEventType.Proposal:
                    return "ProposalEvent";
                case MarketRequestorEventType.PropertyQuery:
                    return "PropertyQueryEvent";
                default:
                    throw new Exception($"Unknown RequestorEventType {eventType}");
            }
        }
        public static string TranslateProviderEventType(MarketProviderEventType eventType)
        {
            switch (eventType)
            {
                case MarketProviderEventType.Proposal:
                    return "ProposalEvent";
                case MarketProviderEventType.PropertyQuery:
                    return "PropertyQueryEvent";
                case MarketProviderEventType.AgreementProposal:
                    return "AgreementEvent";
                default:
                    throw new Exception($"Unknown ProviderEventType {eventType}");
            }
        }

    }
}
