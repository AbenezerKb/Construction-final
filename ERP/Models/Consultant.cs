namespace ERP.Models
{
    public class Consultant
    {
        public string consultantId { get; set; }
        public string projectId { get; set; }      
        public string contractorId { get; set; }
        public DateTime reviewDate { get; set; }
        public ICollection<ApprovedWorkList> approvedWorkList { get; set; }        
        public string changesTaken { get; set; }
        public string reasonForChange { get; set; }
        
        public ICollection<DeclinedWorkList> declinedWorkList { get; set; }
       public string defectsSeen { get; set; }
        public string nextWork { get; set; }
        public ICollection<DefectsCorrectionlist> defectsCorrectionlist { get; set; }
        public string remarks { get; set; }
    }

}
