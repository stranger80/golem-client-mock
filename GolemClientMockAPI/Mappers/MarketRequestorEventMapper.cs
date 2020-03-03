using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class MarketRequestorEventMapper
    {
        public IMapper Mapper { get; set; }

        public MarketRequestorEventMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public GolemMarketMockAPI.MarketAPI.Models.Event Map(Entities.MarketRequestorEvent requestorEventEntity)
        {
            switch(requestorEventEntity.EventType)
            {
                case Entities.MarketRequestorEvent.MarketRequestorEventType.Proposal:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.ProposalEvent>(requestorEventEntity);
                case Entities.MarketRequestorEvent.MarketRequestorEventType.PropertyQuery:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.Event>(requestorEventEntity);
                default:
                    throw new Exception($"Unknown RequestorEventType {requestorEventEntity.EventType}");
            }

            
        }
    }
}
