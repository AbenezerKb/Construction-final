namespace ERP.DTOs
{
    public class HandIssueDTO
    {
        public int IssueId { get; set; }

        public int HandedById { get; set; }

        public ICollection<SendIssueItems> IssueItems { get; set; }

    }

    public class SendIssueItems
    {
        public int ItemId { get; set; }

        public string HandRemark { get; set; }
    }

}
