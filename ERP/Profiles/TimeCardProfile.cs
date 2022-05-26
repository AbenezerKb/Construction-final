using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class TimeCardProfile: Profile
    {
        public TimeCardProfile()
        {
            CreateMap<TimeCard, TimeCardReadDto>();
            CreateMap<TimeCardCreateDto, TimeCard>();
        }
    }
}
