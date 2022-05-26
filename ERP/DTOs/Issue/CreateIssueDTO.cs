namespace ERP.DTOs
{
    public class CreateIssueDTO
    {

        public int RequestedById { get; set; }

        public int SiteId { get; set; }

        public ICollection<CreateIssueItem> IssueItems { get; set; }
    }

    public class CreateIssueItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;


    }
}
