using ERP.Models;

namespace ERP.Services.PerformanceSheetService
{
    public interface IPerformanceSheetService
    {
        Task<List<PerformanceSheet>> GetAllEmployeePerformanceSheetsByProjectId(int projectId);
        Task<List<PerformanceSheet>> GetAllSubcontractorPerformanceSheetsByProjectId(int projectId);
        Task<PerformanceSheet> RemoveSheet(int performanceSheetId);
        Task<List<PerformanceSheet>> GetAllByProjectIdAndEmployeeId(int employeeId, int projectId);
        Task<List<PerformanceSheet>> GetAllByProjectIdAndSubContractorId(int subContractorId, int projectId);

    }

}