using AutoMapper;
using ERP.DTOs;
using ERP.Models;



namespace ERP.Profiles
{
    public class AssignedWorkForceProfile : Profile
    {
        public AssignedWorkForceProfile()
        {
            CreateMap<AssignedWorkForce, AssignedWorkForceReadDto>();
            CreateMap<AssignedWorkForceCreateDto, AssignedWorkForce>();
        }
    }


    public class WorkForceProfile : Profile
    {
        public WorkForceProfile()
        {
            CreateMap<WorkForce, WorkForceReadDto>();
            CreateMap<WorkForceCreateDto, WorkForce>();
        }
    }
}
