using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class ConsultantProfile:Profile
    {
        public ConsultantProfile()
        {
            CreateMap<Consultant, ConsultantReadDto>();
            CreateMap<ConsultantCreateDto, Consultant>();
        }

       
    }


    public class DeclinedWorkListProfile : Profile
    {
        public DeclinedWorkListProfile() {
            CreateMap<DeclinedWorkList, DeclinedWorkListReadDto>();
            CreateMap<DeclinedWorkListCreateDto, DeclinedWorkList>();
        }
    }

    public class ApprovedWorkListProfile : Profile
    {
        public ApprovedWorkListProfile()
        {
            CreateMap<ApprovedWorkList, ApprovedWorkListReadDto>();
            CreateMap<ApprovedWorkListCreateDto, ApprovedWorkList>();
        }
    }

    public class DefectsCorrectionlistProfile : Profile
    {
        public DefectsCorrectionlistProfile()
        {
            CreateMap<DefectsCorrectionlist, DefectsCorrectionlistReadDto>();
            CreateMap<DefectsCorrectionlistCreateDto, DefectsCorrectionlist>();
        }
    }

}
