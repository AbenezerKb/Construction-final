namespace ERP.DTOs
{
    public class DeclineIssueDTO
    {
        public int IssueId { get; set; }

        public int ApprovedById { get; set; }


        public ICollection<DeclineIssueItems> IssueItems { get; set; }
    }

    public class DeclineIssueItems
    {
        public int ItemId { get; set; }

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }

    }
}
