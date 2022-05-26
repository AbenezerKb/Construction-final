
namespace ERP.Services.ProjectManagementReportService
{
    public interface IProjectManagementReportService
    {
        Task<object> GetReportWith(DateTime StartDate, DateTime EndDate, int projectId);

    }

}