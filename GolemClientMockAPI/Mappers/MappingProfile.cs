using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ...from Entities

            CreateMap<Entities.MarketRequestorEvent, GolemMarketMockAPI.MarketAPI.Models.Event>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateRequestorEventType(src.EventType)));

            CreateMap<Entities.MarketRequestorEvent, GolemMarketMockAPI.MarketAPI.Models.ProposalEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateRequestorEventType(src.EventType)))
                    .ForMember(dest => dest.Proposal, opt => opt.MapFrom(src => src.OfferProposal));

            CreateMap<Entities.MarketProviderEvent, GolemMarketMockAPI.MarketAPI.Models.Event>()
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)));
            CreateMap<Entities.MarketProviderEvent, GolemMarketMockAPI.MarketAPI.Models.ProposalEvent>()
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)))
                .ForMember(dest => dest.Proposal, opt => opt.MapFrom(src => src.DemandProposal));
            CreateMap<Entities.MarketProviderEvent, GolemMarketMockAPI.MarketAPI.Models.AgreementEvent>()
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)))
                .ForMember(dest => dest.Agreement, opt => opt.MapFrom(src => src.Agreement));

            CreateMap<Entities.Agreement, GolemMarketMockAPI.MarketAPI.Models.Agreement>()
                .ForMember(dest => dest.Demand, opt => opt.MapFrom(src => src.DemandProposal))
                .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.OfferProposal))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.AgreementId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Entities.Demand, GolemMarketMockAPI.MarketAPI.Models.Demand>()
                .ForMember(dest => dest.RequestorId, opt => opt.MapFrom(src => src.NodeId))
                //.ForMember(dest => dest.DemandId, opt => opt.MapFrom(src => src.Id))
                ;

            // special map to enable mapping of the Agreement
            CreateMap<Entities.DemandProposal, GolemMarketMockAPI.MarketAPI.Models.Demand>()
                .ForMember(dest => dest.RequestorId, opt => opt.MapFrom(src => src.Demand.NodeId))
                .ForMember(dest => dest.DemandId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Demand.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Demand.Properties))
                ;

            CreateMap<Entities.DemandProposal, GolemMarketMockAPI.MarketAPI.Models.Proposal>()
                .ForMember(dest => dest.PrevProposalId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.ProposalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IssuerId, opt => opt.MapFrom(src => src.Demand.NodeId))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Demand.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Demand.Properties));

            CreateMap<Entities.Offer, GolemMarketMockAPI.MarketAPI.Models.Offer>()
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.NodeId))
                //.ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Id))
                ;

            // special map to enable mapping of the Agreement
            CreateMap<Entities.OfferProposal, GolemMarketMockAPI.MarketAPI.Models.Offer>()
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.Offer.NodeId))
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Offer.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Offer.Properties))
                ;

            CreateMap<Entities.OfferProposal, GolemMarketMockAPI.MarketAPI.Models.Proposal>()
                .ForMember(dest => dest.PrevProposalId, opt => opt.MapFrom(src => src.DemandId))
                .ForMember(dest => dest.ProposalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IssuerId, opt => opt.MapFrom(src => src.Offer.NodeId))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Offer.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Offer.Properties));

            CreateMap<Entities.ActivityExecResult, ActivityAPI.Models.ExeScriptCommandResult>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                ;


            // ActivityProviderEvent
            CreateMap<Entities.ActivityProviderEvent, ActivityAPI.Models.CreateActivityProviderEvent>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.AgreementId, opt => opt.MapFrom(src => src.AgreementId))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
                ;

            CreateMap<Entities.ActivityProviderEvent, ActivityAPI.Models.ProviderEvent>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
                ;

            CreateMap<Entities.ActivityProviderEvent, ActivityAPI.Models.ExecProviderEvent>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
                .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.ExeScript.BatchId))
                // NOTE: We are not implementing the ExeScript text parsing here! This must be implemented in dedicated mapper
                .ForMember(dest => dest.ExeScript, opt => opt.Ignore())
                ;


            // ActivityRequestorEvent
            CreateMap<Entities.ActivityRequestorEvent, ActivityAPI.Models.ExeScriptCommandResult>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.ExecResult.Index))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.ExecResult.Result))
                .ForMember(dest => dest.IsBatchFinished, opt => opt.MapFrom(src => src.ExecResult.IsBatchFinished))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.ExecResult.Message))
                ;


            // ...to Entities

            CreateMap<GolemMarketMockAPI.MarketAPI.Models.Demand, Entities.Demand>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => PropertyMappers.MapFromJsonString(src.Properties.ToString())));

            CreateMap<GolemMarketMockAPI.MarketAPI.Models.Offer, Entities.Offer>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => PropertyMappers.MapFromJsonString(src.Properties.ToString())));

            CreateMap<ActivityAPI.Models.ExeScriptCommandResult, Entities.ActivityExecResult>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result));


        }
    }
}
