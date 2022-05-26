using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class LaborDetailProfile: Profile { 


         public LaborDetailProfile()
    {
        CreateMap<LaborDetail, LaborDetailReadDto>();
        CreateMap<LaborDetailCreateDto, LaborDetail>();
    }
}
}
