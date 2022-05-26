using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class ContractProfile: Profile
    {
        public ContractProfile()
        {
            CreateMap<Contract, ContractReadDto>();
            CreateMap<ContractCreateDto, Contract>();
        }
    }




    public class SubContractingWorkProfile : Profile
    {
        public SubContractingWorkProfile()
        {
            CreateMap<SubContractingWork, SubContractingWorkReadDto>();
            CreateMap<SubContractingWorkCreateDto, SubContractingWork>();
        }
    }

}
