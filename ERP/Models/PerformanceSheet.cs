using ERP.Models.Others;
namespace ERP.Models
{
    public class PerformanceSheet : IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? EmployeeId { get; set; }
        public int? SubContractorId { get; set; }
        public float PerformancePoint { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}