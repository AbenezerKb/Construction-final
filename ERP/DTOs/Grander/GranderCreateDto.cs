using System.ComponentModel.DataAnnotations;
using ERP.Models;

namespace ERP.DTOs
{
    public class GranderCreateDto
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectManager { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string RequestNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public IList<WorkForcePlanCreateDto> WorkForcePlans { get; set; }
        [Required]
        public IList<ResourcePlanCreateDto> ResourcePlans { get; set; }
        [Required]
        public IList<SubcontractingPlanCreateDto> SubcontractingPlans { get; set; }
        [Required]
        public string ApprovedBy { get; set; }
        [Required]
        public int TotalEstiamtedReqtBudget { get; set; }

    }





    
    
    





    public class WorkForcePlanCreateDto {

        //public string laborId { get; set; }
        
    //    public string GranderFK { get; set; }
        [Required]
        public string labor { get; set; }
        [Required]
        public int number { get; set; }
        [Required]
        public double budget { get; set; }
    }


    

    public class ResourcePlanCreateDto
    {
      
        [Required]
        public string equipment { get; set; }
        
      //  public string GranderFK { get; set; }
        [Required]
        public string unit { get; set; }
        [Required]
        public int amount { get; set; }
        [Required]
        public double budget { get; set; }
    }

    public class SubcontractingPlanCreateDto
    {
        [Required]
        public string SubcontractingWork { get; set; }
        [Required]
        public string Subcontractor { get; set; }
        
     //   public string GranderFK { get; set; }
    }
}
