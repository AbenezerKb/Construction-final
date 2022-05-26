using System.ComponentModel.DataAnnotations;


namespace ERP.Models
{
    public class WeeklyEquipment
    {
        [Key]
        [Required]
        public string equipmentId { get; set; }        
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //    public WeeklyRequirement WeeklyRequirement { get; set; }
    }

    public class WeeklyLabor
    {
        [Key]
        [Required]
        public string laborId { get; set; }
        public string WeeklyRequirementFK { get; set; }
        public string labor { get; set; }
        public int number { get; set; }
        public double  budget { get; set; }
        // public WeeklyRequirement WeeklyRequirement { get; set; }
    }

    public class WorkForcePlan
    {
        [Key]
        [Required]
        public string laborId { get; set; }
        public string GranderFK { get; set; }
        public string labor { get; set; }
        public int number { get; set; }
        public double budget { get; set; }       

    }


    public class WeeklyMaterial
    {
        [Key]
        [Required]
        public string materialId { get; set; }        
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //public WeeklyRequirement WeeklyRequirement { get; set; }
    }

    public class SubcontractingPlan
    {
        [Key]
        [Required]
        public string subcontractingPlanId { get; set; }        
        public string Subcontractor { get; set; }
        public string GranderFK { get; set; }        
    }

    public class ResourcePlan 
    {
        [Key]
        [Required]
        public string equipmentId { get; set; }        
        public string GranderFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }        
    }

    public class ApprovedWorkList
    {
        [Key]
        [Required]
        public string ApprovedWorkId { get; set; }
        public string ConsultantId { get; set; }
        public string ApprovedWorks { get; set; }
         
    }
    public class DeclinedWorkList
    {
        [Key]
        [Required]
        public string DeclinedWorkId { get; set; }
        public string ConsultantId { get; set; }
        public string DeclinedWorks { get; set; }

    }
    
public class DefectsCorrectionlist
    {

        [Key]
        [Required]
        public string DefectsCorrectionId { get; set; }
        public string ConsultantId { get; set; }
        public string DefectsCorrections { get; set; }
    }

}
