using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{

    public class SubTask : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Budget { get; set; }
        public double Progress { get; set; }
        public string Remark { get; set; } = string.Empty;

        public int TaskId { get; set; }

        [JsonIgnore]
        public ProjectTask? ProjectTask { get; set; }
        [JsonIgnore]
        public List<WeeklyResultValue>? WeeklyResultValues { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool isCompleted() => Progress == 100;

    }
}