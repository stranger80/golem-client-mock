using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public enum AgreementState
    {
        Proposal = 0,
        Pending = 1,
        Cancelled = 2,
        Rejected = 3,
        Approved = 4,
        Expired = 5,
        Terminated = -1,
    }

    public class Agreement
    {
        public string Id { get; set; }

        public AgreementState State { get; set; }

        public DemandProposal DemandProposal { get; set; }

        public OfferProposal OfferProposal { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
