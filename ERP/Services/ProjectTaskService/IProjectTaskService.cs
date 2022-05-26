using ERP.Models;
using ERP.DTOs.ProjectTask;

namespace ERP.Services.ProjectTaskService
{
    public interface IProjectTaskService
    {
        Task<ProjectTask> Add(ProjectTaskDto projectDto);
        Task<ProjectTask> Remove(int id);
        Task<ProjectTask> Update(int id, ProjectTaskDto projectDto);
        Task<List<ProjectTask>> GetAll();
        Task<List<ProjectTask>> GetByProjectId(int projectId);
        Task<ProjectTask> GetById(int id);
        Task<List<ProjectTask>> GetByName(string name);
        Task<List<ProjectTask>> GetByNameAndProjectId(string name, int projectId);
        Task<List<ProjectTask>> GetSubContractorWorks(int projectId);


    }
}