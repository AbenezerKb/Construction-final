using ERP.Models;

namespace ERP.DTOs
{
    public class ConsultantReadDto
    {
        public string projectId { get; set; }
        public string projectName { get; set; }
        public string consultantName { get; set; }
        public string contractorId { get; set; }
        public DateTime reviewDate { get; set; }
        public IList<ApprovedWorkListReadDto> approvedWorkList { get; set; }
        public string changesTaken { get; set; }
        public string reasonForChange { get; set; }
        public IList<DeclinedWorkListReadDto> declinedWorkList { get; set; }
        public string defectsSeen { get; set; }
        public string nextWork { get; set; }
        public IList<DefectsCorrectionlistReadDto> defectsCorrectionlist { get; set; }
        public string remarks { get; set; }
    }








    public class ApprovedWorkListReadDto
    {

        public string ApprovedWorkId { get; set; }
        public string ConsultantId { get; set; }
        public string ApprovedWorks { get; set; }
    }

    public class DeclinedWorkListReadDto
    {
        public string DeclinedWorkId { get; set; }
        public string ConsultantId { get; set; }
        public string DeclinedWorks { get; set; }

    }

    public class DefectsCorrectionlistReadDto
    {
        public string DefectsCorrectionId { get; set; }
        public string DefectsCorrectionlistId { get; set; }
        public string DefectsCorrections { get; set; }

    }





}
