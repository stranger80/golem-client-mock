﻿using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class SubscribeOfferOperation : InMemoryMarketOperationBase
    {
        public SubscribeOfferOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, MarketRequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, MarketProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions)
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public OfferSubscription Run(Offer offer)
        {
            // 1. Persist subscription
            var offerSubscription = this.SubscriptionRepository.CreateOfferSubscription(offer);

            // 2. Construct the pipeline
            var pipeline = new SubscriptionPipeline<OfferSubscription, MarketProviderEvent>()
            {
                Subscription = offerSubscription
            };

            this.ProviderEventPipelines.Add(offerSubscription.Id, pipeline);

            // 3. Resolve the Demand against existing Offer subscriptions and:
            //    - pull the matching ones immediately to pipeline
            //    - ???push the Demand into pipelines of matching Offers???
            //      this is questionable, as this would "duplicate" the message exchange -
            //      - Providers would receive a "market proposal" from matching, and immediately respond with a counter proposal,
            //        thus the Requestors would almost certainly receive a "market proposal" and a subsequent "direct proposal"
            //        from most Providers (as there are few Requestors and many willing Providers)
            //      - So it is tempting to 'break the symmetry' and not forward the Demand automatically to Provider subscription pipelines.

            foreach (var demandSubscription in this.RequestorEventPipelines.Values)
            {
                var demand = demandSubscription.Subscription.Demand;

                var matchingResult = this.MarketResolver.MatchDemandOffer(demand.Properties.Select(prop => prop.Key + ((prop.Value == null) ? "" : ("=" + JsonConvert.SerializeObject(prop.Value)))).ToArray(), demand.Constraints,
                                                                          offer.Properties.Select(prop => prop.Key + ((prop.Value == null) ? "" : ("=" + JsonConvert.SerializeObject(prop.Value)))).ToArray(), offer.Constraints);

                switch (matchingResult)
                {
                    case GolemMarketResolver.ResultEnum.True:
                        // Create offer proposal
                        var offerProposal = this.ProposalRepository.SaveOfferProposal(demandSubscription.Subscription.Id, offerSubscription.Id, offerSubscription.Offer); // no "previous proposal id" as we are mathcing the "on market" demands/offers
                        this.OfferSubscriptions.Add(offerProposal.Id, offerSubscription.Id);

                        // build Requestor OfferProposal event and put in Requestor pipeline
                        var requestorEvent = new MarketRequestorEvent()
                        {
                            EventDate = DateTime.Now,
                            EventType = MarketRequestorEvent.MarketRequestorEventType.Proposal,
                            OfferProposal = offerProposal,
                            ProviderId = offer.NodeId
                        };

                        if (!demandSubscription.PipelineQueue.TryAdd(requestorEvent))
                        {
                            // TODO what do we do if unable to Add (eg. due to collection full)?
                            // Log warning?
                            // Should iteration be stopped now (to allow the Requestor to collect results?)
                            // What about other possible Offers that were skipped? they will not have a chance of being matched again...
                        }
                        break;
                    default:
                        break;
                }
            }

            return offerSubscription;
        }
    }
}
