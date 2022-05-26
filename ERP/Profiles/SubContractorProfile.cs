using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class SubContractorProfile : Profile
    {
        public SubContractorProfile()
        {
            CreateMap<SubContractor, SubContractorReadDto>();
            CreateMap<SubContractorCreateDto, SubContractor>();
        }
    }
}
