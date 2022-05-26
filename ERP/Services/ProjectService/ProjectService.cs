using ERP.Context;
using ERP.Models;
using ERP.Exceptions;
using ERP.DTOs.Project;
using ERP.Models.Others;
using Microsoft.EntityFrameworkCore;
using ERP.DTOs.TaskProgressSheet;
using ERP.DTOs.TaskSchedule;

namespace ERP.Services.ProjectService
{

    public class ProjectService : IProjectService
    {
        private readonly DataContext dbContext;
        public ProjectService(DataContext appDbContext)
        {
            dbContext = appDbContext;

        }

        public async Task<List<TaskScheduleDto>> GetActualSchedule(int projectId)
        {
            List<TaskScheduleDto> actualSchedule = new();
            var weeklyPlans = await dbContext.WeeklyPlans.AsNoTracking().Where(wp => wp.ProjectId == projectId)
                                                   .Include(wp => wp.PlanValues)
                                                   .ThenInclude(wpv => wpv.SubTask)
                                                   .ToListAsync();
            weeklyPlans.ForEach(wp =>
            {
                var weekEndDate = wp.WeekStartDate.AddDays(7);
                var weeklySchedule = wp.PlanValues.Select(wpv => new TaskScheduleDto
                {
                    TaskName = wpv.SubTask!.Name,
                    Priority = wpv.SubTask.Priority,
                    StartDate = wp.WeekStartDate,
                    EndDate = weekEndDate
                });

                actualSchedule.AddRange(weeklySchedule);

            });


            return actualSchedule;
        }

        public async Task<List<Project>> GetByName(string name)
        {
            var projects = await dbContext.Projects.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (!projects.Any())
            {
                throw new ItemNotFoundException($"Projects not found with name={name}");
            }
            return projects;

        }

        public async Task<List<Project>> GetByNameAndSiteId(string name, int siteId)
        {
            var projects = await dbContext.Projects.Where(p => p.SiteId == siteId && p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (!projects.Any())
            {
                throw new ItemNotFoundException($"Projects not found with name={name} and siteId={siteId}");
            }
            return projects;
        }

        public async Task<List<Project>> GetBySiteId(int siteId)
        {
            var projects = await dbContext.Projects.Where(p => p.SiteId == siteId).ToListAsync();
            if (!projects.Any())
            {
                throw new ItemNotFoundException($"Projects not found with siteId={siteId}");
            }
            return projects;
        }

        public async Task<List<TaskScheduleDto>> GetCrashSchedule(int projectId)
        {
            List<SubTask> subTasks = await dbContext.SubTasks.AsNoTracking()
                                                             .Include(st => st.ProjectTask)
                                                             .Where(st => st.ProjectTask!.ProjectId == projectId)
                                                             .OrderBy(st => st.StartDate)
                                                             .ToListAsync();

            List<TaskScheduleDto> crashSchedule = subTasks.Select(st =>
              new TaskScheduleDto
              {
                  TaskName = st.Name,
                  Priority = st.Priority,
                  StartDate = st.StartDate,
                  EndDate = st.EndDate
              }).ToList();

            return crashSchedule;

        }

        public async Task<List<TaskProgressSheetDto>> GetTaskProgressSheet(int projectId)
        {
            var project = await dbContext.Projects.Where(p => p.Id == projectId)
                                                .Include(p => p.Tasks)
                                                .ThenInclude(p => p.SubTasks)
                                                .FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ItemNotFoundException($"Project not found with ProjectId={projectId}");
            }
            List<TaskProgressSheetDto> taskProgressSheets = project.Tasks.Select(t =>
            new TaskProgressSheetDto
            {
                MainTaskId = t.Id,
                MainTaskName = t.Name,
                Progress = (float)t.Progress

            }).ToList();

            return taskProgressSheets;
        }

        async Task<Project> IProjectService.Add(ProjectDto projectDto)
        {

            var project = new Project
            {
                Name = projectDto.Name,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                SiteId = projectDto.SiteId,
                ManagerId = projectDto.ManagerId,
                CoordinatorId = projectDto.CoordinatorId,
                Status = Status.New
            };
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return project;

        }

        async Task<List<Project>> IProjectService.GetAll()
        {
            return await dbContext.Projects.ToListAsync();
        }

        async Task<Project> IProjectService.GetById(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project not found with id={id}");
            }
            return project;
        }

        async Task<Project> IProjectService.Remove(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project not found with id={id}");
            }

            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();

            return project;
        }

        async Task<Project> IProjectService.Update(int id, ProjectDto projectDto)
        {
            var project = await dbContext.Projects.FindAsync(id);

            if (project == null)
            {
                throw new ItemNotFoundException($"Project not found with id={id}");
            }

            project.Name = projectDto.Name;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.ManagerId = projectDto.ManagerId;
            project.CoordinatorId = projectDto.CoordinatorId;
            project.SiteId = projectDto.SiteId;

            dbContext.Update(project);
            await dbContext.SaveChangesAsync();

            return project;

        }
    }
}
