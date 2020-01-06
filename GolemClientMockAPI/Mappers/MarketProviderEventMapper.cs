using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class MarketProviderEventMapper
    {
        public IMapper Mapper { get; set; }

        public MarketProviderEventMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public GolemMarketMockAPI.MarketAPI.Models.Event Map(Entities.MarketProviderEvent providerEventEntity)
        {
            switch (providerEventEntity.EventType)
            {
                case Entities.MarketProviderEvent.MarketProviderEventType.Proposal:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.ProposalEvent>(providerEventEntity);
                case Entities.MarketProviderEvent.MarketProviderEventType.PropertyQuery:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.Event>(providerEventEntity);
                case Entities.MarketProviderEvent.MarketProviderEventType.AgreementProposal:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.AgreementEvent>(providerEventEntity);
                default:
                    throw new Exception($"Unknown ProviderEventType {providerEventEntity.EventType} ");
            }
        }
    }
}
