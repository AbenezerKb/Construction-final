using ERP.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ProjectManagementAnalyticsService
{
    public class ProjectManagementAnalyticsService : IProjectManagementAnalyticsService
    {
        DataContext dbContext;
        public ProjectManagementAnalyticsService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<object> GetAnalytics(int projectId)
        {
            if (!await dbContext.Projects.AnyAsync(p => p.Id == projectId))
            {
                throw new Exceptions.ItemNotFoundException($"Project not found with id={projectId}");
            }
            var projectTasks = await dbContext.Tasks.Where(t => t.ProjectId == projectId)
                                                    .Include(t => t.SubTasks)
                                                    .ToListAsync();
            var tasksCompletedCount = projectTasks.Count(t => t.IsCompleted());
            return new
            {
                tasks = new
                {
                    completed = tasksCompletedCount,
                    pending = projectTasks.Count - tasksCompletedCount
                },
                professionals = new
                {
                    planned = 0,
                    used = 0
                },
                labors = new
                {
                    planned = 0,
                    used = 0
                },
                subContractors = new
                {
                    completed = 0,
                    pending = 0
                },
                equipments = new
                {
                    planned = 0,
                    used = 0,
                    damaged = 0
                },
                materials = new
                {
                    planned = 0,
                    used = 0,
                    damaged = 0
                }
            };
        }
    }
}