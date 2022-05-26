using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class AllocatedResourcesProfile:Profile
    {
        
        public AllocatedResourcesProfile()
        {
            CreateMap<AllocatedResources, AllocatedResourcesReadDto>();
            CreateMap<AllocatedResourcesCreateDto, AllocatedResources>();
        }
        
    }
}
