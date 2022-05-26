namespace ERP.Services.ProjectManagementAnalyticsService
{
    public interface IProjectManagementAnalyticsService
    {
        Task<object> GetAnalytics(int projectId);

    }


}