using ERP.Models;
using ERP.DTOs.SubTask;
namespace ERP.Services.SubTaskService
{
    public interface ISubTaskService
    {
        Task<SubTask> Add(SubTaskDto subTaskDto);
        Task<SubTask> Remove(int id);
        Task<SubTask> Update(int id, SubTaskDto subTaskDto);
        Task<List<SubTask>> GetAll();
        Task<List<SubTask>> GetByTaskId(int taskId);
        Task<SubTask> GetById(int id);

        Task<List<SubTask>> GetUpComming(int deadlineNotificationDay);
    }
}