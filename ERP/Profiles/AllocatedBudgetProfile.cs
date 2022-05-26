using AutoMapper;
using ERP.DTOs;
using ERP.Models;


namespace ERP.Profiles
{
    public class AllocatedBudgetProfile: Profile
    {
        public AllocatedBudgetProfile()
        {
            CreateMap<AllocatedBudget, AllocatedBudgetReadDto>();
            CreateMap<AllocatedBudgetCreateDto, AllocatedBudget>();
        }
    }
}
