using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IAgreementRepository
    {
        Agreement CreateAgreement(DemandProposal demand, OfferProposal offer, DateTime validTo);
        void UpdateAgreementState(string agreementId, AgreementState state);
        Agreement GetAgreement(string agreementId);

    }
}
