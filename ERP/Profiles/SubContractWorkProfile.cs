using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class SubContractWorkProfile: Profile
    {
        public SubContractWorkProfile()
        {
            CreateMap<SubContractWork, SubContractWorkReadDto>();
            CreateMap<SubContractWorkCreateDto, SubContractWork>();
        }
    }
}
