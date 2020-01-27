using AutoMapper;
using GolemMarketMockAPI.MarketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class AgreementMapper
    {
        public IMapper Mapper { get; set; }

        public AgreementMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public Entities.Agreement MapToEntity(Agreement agreement)
        {
            return this.Mapper.Map<Entities.Agreement>(agreement);
        }

        public Agreement MapEntityToAgreement(Entities.Agreement agreement)
        {
            return this.Mapper.Map<Agreement>(agreement);
        }

    }
}
