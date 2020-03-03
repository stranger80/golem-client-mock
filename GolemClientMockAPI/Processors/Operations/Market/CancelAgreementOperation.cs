using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CancelAgreementOperation : ConfirmAgreementBase
    {

        public CancelAgreementOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IAgreementRepository agreementRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, MarketRequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, MarketProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions,
                                        IDictionary<string, BlockingCollection<AgreementResultEnum>> agreementResultPipelines) 
            : base(subscriptionRepo, proposalRepo, agreementRepo, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions, agreementResultPipelines)
        {
        }

        public async Task<bool> Run(string agreementId)
        {
            // OK, so

            var result = this.SendCancelAgreement(agreementId);

            return result;
        }

        /// <summary>
        /// - check Agreement state 
        ///   - if Proposed it means the Agreement hasn't been approved yet
        ///     - Set Agreement to Cancelled
        ///     - send Cancelled AgreementResult to Requestor side AgreementResultPipeline (to break any hanging ConfirmAgreement calls)
        ///     - return true
        ///   - if not Proposed - it means the Provider has already approved the agreement - cannot cancel
        ///     - return false
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public bool SendCancelAgreement(string agreementId)
        {
            // 0. Validate the agreement
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if (agreement != null)
            {
                switch(agreement.State)
                {
                    case AgreementState.Pending:
                        this.AgreementRepository.UpdateAgreementState(agreementId, AgreementState.Cancelled);

                        // if there already is an awaiting response pipeline - put Cancel event in it
                        if (this.AgreementResultPipelines.ContainsKey(agreementId))
                        {
                            this.AgreementResultPipelines[agreementId].Add(AgreementResultEnum.Cancelled);
                        }

                        return true;
                    case AgreementState.Cancelled:
                    case AgreementState.Rejected: // already rejected - cancel successful

                        return true;

                    default:
                        return false;
                }

            }
            else
            {
                throw new Exception($"Agreement Id {agreementId} does not exist...");
            }
        }

    }
}
