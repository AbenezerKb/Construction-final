using ERP.Models;


namespace ERP.Services
{
    public interface IWeeklyRequirementRepo
    {
        bool SaveChanges();

        IEnumerable<WeeklyRequirement> GetAllWeeklyRequirement();
        WeeklyRequirement GetWeeklyRequirements(string id);
        void CreateWeeklyRequirement(WeeklyRequirement weeklyRequirement);
        void DeleteWeeklyRequirements(string id);
        void UpdateWeeklyRequirement(string id,WeeklyRequirement updatedWeeklyRequirement);
    }
}
