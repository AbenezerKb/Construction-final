
using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{
    public class ProjectTask : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSubContractorWork { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Progress
        {
            get
            {
                return (float)GetTaskProgress();
            }
        }


        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }
        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsCompleted()
        {
            return GetTaskProgress() == 100;
        }
        public double GetTaskProgress()
        {
            return SubTasks.Select(t =>
                  {
                      return t.Progress;

                  }).DefaultIfEmpty().Average();

        }
        public double GetTotalBudget()
        {
            return SubTasks.Sum(s => s.Budget);

        }
        public object GetSubTaskSummery()
        {
            var summery = SubTasks.Select(s => new
            {
                SubTaskName = s.Name,
                Budget = s.Budget
            }).ToList();
            return summery;
        }

    }
}
