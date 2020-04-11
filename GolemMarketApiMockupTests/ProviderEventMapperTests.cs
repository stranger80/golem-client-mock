using AutoMapper;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Mappers;
using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class ProviderEventMapperTests
    {

        public IMapper Mapper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.Mapper = config.CreateMapper();
        }

        [TestMethod]
        public void Map_ProviderEvent_ToDemandEvent()
        {

            var mapper = new MarketProviderEventMapper(this.Mapper);

            var entity = new MarketProviderEvent()
            {
                EventDate = DateTime.Now,
                EventType = MarketProviderEvent.MarketProviderEventType.Proposal,
                DemandProposal = new DemandProposal()
                {
                    OfferId = "PreviousOfferId",
                    InternalId = 2,
                    Demand = new Demand()
                    {
                        NodeId = "RequestorNodeId",
                        Constraints = "()",
                        Properties = new Dictionary<string, JToken>() { }
                    }
                }
            };

            var result = mapper.Map(entity) as GolemMarketMockAPI.MarketAPI.Models.ProposalEvent;

            Assert.AreEqual(entity.DemandProposal.Demand.Constraints, result.Proposal.Constraints);
            Assert.AreEqual(entity.DemandProposal.OfferId, result.Proposal.PrevProposalId);
            Assert.AreEqual(entity.DemandProposal.Id, result.Proposal.ProposalId);
            Assert.AreEqual(entity.DemandProposal.Demand.NodeId, result.Proposal.IssuerId);

        }



    }
}
