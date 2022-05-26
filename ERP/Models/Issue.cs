using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }

        public int Status { get; set; } = ISSUESTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ApproveDate { get; set; }

        public DateTime? HandDate { get; set; }

        public Site Site { get; set; }

        public int SiteId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee HandedBy { get; set; }

        public int? HandedById { get; set; }

        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }
    }
}