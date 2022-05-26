using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class BIDProfile: Profile
    {
        public BIDProfile()
        {
            CreateMap<BID, BIDReadDto>();
            CreateMap<BIDCreateDto, BID>();
        }
        
    }
}




