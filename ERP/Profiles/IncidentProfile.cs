using AutoMapper;
using ERP.DTOs;
using ERP.Models;

namespace ERP.Profiles
{
    public class IncidentProfile: Profile
    {
        public IncidentProfile()
        {
            CreateMap<Incident, IncidentReadDto>();
            CreateMap<IncidentCreateDto, Incident>();
        }
    }
}
