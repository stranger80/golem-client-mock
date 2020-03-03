using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface IRequestorMarketProcessor
    {
        Task<ICollection<DemandSubscription>> GetDemandsAsync(string nodeId);

        DemandSubscription SubscribeDemand(Demand demand);

        Task<ICollection<MarketRequestorEvent>> CollectRequestorEventsAsync(string subscriptionId, float? timeout, int? maxEvents);

        DemandProposal CreateDemandProposal(string subscriptionId, string offerProposalId, Demand demand);

        Agreement CreateAgreement(String proposal, DateTime validTo);

        Task<AgreementResultEnum> ConfirmAgreementAsync(string agreementId, float? timeout);
        void SendConfirmAgreement(string agreementId);

        Task<AgreementResultEnum> WaitConfirmAgreementResponseAsync(string agreementId, float? timeout);

        Task<bool> CancelAgreement(string agreementId);

        void UnsubscribeDemand(string subscriptionId);
    }
}
