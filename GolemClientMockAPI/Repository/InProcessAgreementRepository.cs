using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Repository
{
    public class InProcessAgreementRepository : IAgreementRepository
    {
        public IDictionary<string, Agreement> Agreements { get; set; } = new ConcurrentDictionary<string, Agreement>();

        public Agreement CreateAgreement(DemandProposal demandProposal, OfferProposal offerProposal, DateTime validTo)
        {
            if(this.Agreements.ContainsKey(offerProposal.Id))
            {
                throw new Exception($"Agreement Id {offerProposal.Id} already exists!");
            }

            var agreement = new Agreement()
            {
                Id = offerProposal.Id,
                OfferProposal = offerProposal,
                DemandProposal = demandProposal,
                State = AgreementState.Proposal,
                ValidTo = validTo
            };

            this.Agreements[agreement.Id] = agreement;

            return agreement;
        }

        public Agreement GetAgreement(string agreementId)
        {
            if (this.Agreements.ContainsKey(agreementId))
            {
                return this.Agreements[agreementId];
            }
            else
            {
                return null;
                //throw new Exception($"Agreement Id {agreementId} does not exist!");
            }
        }

        public void UpdateAgreementState(string agreementId, AgreementState state)
        {
            if (this.Agreements.ContainsKey(agreementId))
            {
                this.Agreements[agreementId].State = state;
            }
            else
            {
                throw new Exception($"Agreement Id {agreementId} does not exist!");
            }
        }
    }
}
