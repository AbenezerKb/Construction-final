using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{
    public class WeeklyResultValue : IAuditableEntity
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int WeeklyResultId { get; set; }
        [JsonIgnore]
        public WeeklyResult? WeeklyResult { get; set; }
        public int? SubTaskId { get; set; }

        [JsonIgnore]
        public SubTask? SubTask { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}