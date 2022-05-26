
using Microsoft.EntityFrameworkCore;
using ERP.Exceptions;
using ERP.Context;
using ERP.Models;
using ERP.DTOs.ProjectTask;

namespace ERP.Services.ProjectTaskService
{

    public class ProjectTaskService : IProjectTaskService
    {
        private readonly DataContext DbContext;
        public ProjectTaskService(DataContext appDbContext)
        {
            DbContext = appDbContext;

        }
        public async Task<ProjectTask> Add(ProjectTaskDto taskDto)
        {
            var project = await DbContext.Projects.FindAsync(taskDto.ProjectId);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project not found with projectId={taskDto.ProjectId}");
            }
            var task = new ProjectTask
            {
                Name = taskDto.Name,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                ProjectId = taskDto.ProjectId,
                IsSubContractorWork = taskDto.IsSubContractorWork

            };
            if (taskDto.IsSubContractorWork)
            {
                task.SubTasks.Add(new SubTask
                {
                    Name = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = TaskConstant.MAXPRIORITYVALUE,
                    Budget = taskDto.Budget,
                    Progress = 0,
                });
            }
            project.Tasks.Add(task);
            await DbContext.SaveChangesAsync();
            return task;
        }



        public async Task<List<ProjectTask>> GetAll()
        {
            return await DbContext.Tasks.Include(t => t.SubTasks).ToListAsync();
        }

        public async Task<ProjectTask> GetById(int id)
        {
            var task = (await DbContext.Tasks.Where(t => t.Id == id).Include(t => t.SubTasks).ToListAsync()).FirstOrDefault();
            if (task == null) throw new ItemNotFoundException($"Task not found with id={id}");
            return task;
        }

        public async Task<List<ProjectTask>> GetByName(string name)
        {
            var tasks = await DbContext.Tasks.Where(t => t.Name.ToLower().Contains(name.ToLower())).Include(t => t.SubTasks).ToListAsync();

            if (!tasks.Any())
            {
                throw new ItemNotFoundException($"Tasks not found with name={name}");
            }
            return tasks;
        }

        public async Task<List<ProjectTask>> GetByNameAndProjectId(string name, int projectId)
        {
            var tasks = await DbContext.Tasks.Where(t => t.ProjectId == projectId && t.Name.ToLower().Contains(name.ToLower())).Include(t => t.SubTasks).ToListAsync();

            if (!tasks.Any())
            {
                throw new ItemNotFoundException($"Tasks not found with name={name} and projectId={projectId}");
            }
            return tasks;
        }

        public async Task<List<ProjectTask>> GetByProjectId(int projectId)
        {
            var tasks = await DbContext.Tasks.Where(t => t.ProjectId == projectId).Include(t => t.SubTasks).ToListAsync();

            if (!tasks.Any())
            {
                throw new ItemNotFoundException($"Tasks not found with projectId={projectId}");
            }
            return tasks;

        }

        public async Task<List<ProjectTask>> GetSubContractorWorks(int projectId)
        {
            var project = await DbContext.Projects.Where(p => p.Id == projectId)
                                                  .Include(p => p.Tasks)
                                                  .ThenInclude(t => t.SubTasks)
                                                  .FirstOrDefaultAsync();
            if (project == null) throw new ItemNotFoundException($"Project not found with ProjectId={projectId}");
            return project.Tasks.Where(t => t.IsSubContractorWork).ToList();

        }


        public async Task<ProjectTask> Remove(int id)
        {
            var task = await DbContext.Tasks.FindAsync(id);
            if (task == null) throw new ItemNotFoundException($"Task not found with id={id}");
            DbContext.Tasks.Remove(task);
            await DbContext.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectTask> Update(int id, ProjectTaskDto taskDto)
        {
            var task = await DbContext.Tasks.FindAsync(id);
            if (task == null) throw new ItemNotFoundException($"Task not found with id={id}");

            task.Name = taskDto.Name;
            task.StartDate = taskDto.StartDate;
            task.EndDate = taskDto.EndDate;
            task.ProjectId = taskDto.ProjectId;
            DbContext.Update(task);
            await DbContext.SaveChangesAsync();

            return task;
        }
    }
}