using ERP.DTOs.WeeklyResult;
using ERP.Models;
namespace ERP.Services.WeeklyResultService
{
    public interface IWeeklyResultService
    {
        Task<WeeklyResult> Add(WeeklyResultDto weeklyResultDto);
        Task<WeeklyResultValue> UpdateResult(int weeklyResultValueId, int newValue);
        Task<List<WeeklyResult>> GetByProjectId(int projectId);
        Task<WeeklyResult> GetByWeeklyPlanId(int weeklyPlanId);
        Task<WeeklyResult> GetById(int weeklyResultId);
        Task<List<WeeklyResult>> GetAll();
        // Task<WeeklyResult> UpdateWeeklyResultApproval(int weeklyResultId, int employeeId, bool isApproved);
        Task<WeeklyResult> Remove(int weeklyResultId);
    }

}