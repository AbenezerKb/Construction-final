namespace ERP.DTOs
{
    public class ApproveIssueDTO
    {
        public int IssueId { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<ApproveIssueItems> IssueItems { get; set; }
    }

    public class ApproveIssueItems
    {
        public int ItemId { get; set; }

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }
    }
}
