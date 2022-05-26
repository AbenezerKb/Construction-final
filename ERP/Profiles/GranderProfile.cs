using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class GranderProfile:Profile
    {
        public GranderProfile()
        {
            CreateMap<Grander, GranderReadDto>();
            CreateMap<GranderCreateDto, Grander>();
        }

    }
       

    public class ResourcePlanProfile : Profile
    {        
        public ResourcePlanProfile()
        {
            CreateMap<ResourcePlan, ResourcePlanReadDto>();
            CreateMap<ResourcePlanCreateDto, ResourcePlan>();
        }

    }




    public class SubcontractingPlanProfile : Profile
    {
        public SubcontractingPlanProfile()
        {
            CreateMap<SubcontractingPlan, SubcontractingPlanReadDto>();
            CreateMap<SubcontractingPlanCreateDto, SubcontractingPlan>();
        }

    }




    public class WorkForcePlanProfile : Profile
    {
        public WorkForcePlanProfile()
        {
            CreateMap<WorkForcePlan, WorkForcePlanReadDto>();
            CreateMap<WorkForcePlanCreateDto, WorkForcePlan>();
        }

    }


}
