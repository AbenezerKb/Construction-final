using ERP.Models;


namespace ERP.DTOs
{
    public class GranderReadDto
    {
        public string GranderId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManager { get; set; }
        public string Duration { get; set; }
        public string RequestNo { get; set; }
        public DateTime Date { get; set; }
        public IList<WorkForcePlanReadDto> WorkForcePlans { get; set; }
        public IList<ResourcePlanReadDto> ResourcePlans { get; set; }
        public IList<SubcontractingPlanReadDto> SubcontractingPlans { get; set; }
        public string ApprovedBy { get; set; }
        public int TotalEstiamtedReqtBudget { get; set; }

    }

    public class WorkForcePlanReadDto
    {
        public string laborId { get; set; }
        public string GranderFK { get; set; }
        public string labor { get; set; }
        public int number { get; set; }
        public double budget { get; set; }
    }

    public class ResourcePlanReadDto
    {        
        public string equipmentId { get; set; }
        public string equipment { get; set; }
        public string GranderFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
    }

    public class SubcontractingPlanReadDto
    {
        public string subcontractingPlanId { get; set; }
        public string SubcontractingWork { get; set; }
        public string Subcontractor { get; set; }
        public string GranderFK { get; set; }
    }



}
