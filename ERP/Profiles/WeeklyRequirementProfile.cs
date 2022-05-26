using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class WeeklyRequirementProfile: Profile
    {
        public WeeklyRequirementProfile()
        {
            CreateMap<WeeklyRequirement, WeeklyRequirementReadDto>();
            CreateMap<WeeklyRequirementCreateDto, WeeklyRequirement>();
        }
    }


    public class WeeklyMaterialProfile : Profile
    {
        public WeeklyMaterialProfile()
        {
            CreateMap<WeeklyMaterial, WeeklyMaterialReadDto>();
            CreateMap<WeeklyMaterialCreateDto, WeeklyMaterial>();
        }
            
    }



    public class WeeklyEquipmentProfile : Profile
    {
        public WeeklyEquipmentProfile()
        {
            CreateMap<WeeklyEquipment, WeeklyEquipmentReadDto>();
            CreateMap<WeeklyEquipmentCreateDto, WeeklyEquipment>();
        }
    }



    public class WeeklyLaborProfile : Profile
    {
        public WeeklyLaborProfile()
        {
            CreateMap<WeeklyLabor, WeeklyLaborReadDto>();
            CreateMap<WeeklyLaborCreateDto, WeeklyLabor>();
        }
    }
}
