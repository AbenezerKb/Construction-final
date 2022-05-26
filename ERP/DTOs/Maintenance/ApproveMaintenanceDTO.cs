namespace ERP.DTOs
{
    public class ApproveMaintenanceDTO
    {
        public int MaintenanceId { get; set; }

        public int ApprovedById { get; set; }

        public string ApproveRemark { get; set; }
    }
}
