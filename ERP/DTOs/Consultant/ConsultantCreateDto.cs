using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs
{
    public class ConsultantCreateDto
    {
        [Required]
        public string projectId { get; set; }
        [Required]
        public string projectName { get; set; }
        [Required]
        public string consultantName { get; set; }
        [Required]
        public string contractorId { get; set; }
        [Required]
        public DateTime reviewDate { get; set; }
        [Required]
        public IList<ApprovedWorkListCreateDto> approvedWorkList { get; set; }
        [Required]
        public string changesTaken { get; set; }
        [Required]
        public string reasonForChange { get; set; }
        [Required]
        public IList<DeclinedWorkListCreateDto> declinedWorkList { get; set; }
        [Required]
        public string defectsSeen { get; set; }
        [Required]
        public string nextWork { get; set; }
        [Required]
        public IList<DefectsCorrectionlistCreateDto> defectsCorrectionlist { get; set; }
        [Required]
        public string remarks { get; set; }
    }


    public class ApprovedWorkListCreateDto
    {
        [Required]
        public string ApprovedWorks { get; set; }
    }

    public class DeclinedWorkListCreateDto
    {
        [Required]
        public string DeclinedWorks { get; set; }

    }

    public class DefectsCorrectionlistCreateDto
    {
        [Required]
        public string DefectsCorrections { get; set; } 

    }


}
