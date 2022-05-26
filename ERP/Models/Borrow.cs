using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }

        public int Status { get; set; } = BORROWSTATUS.REQUESTED;

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

        public ICollection<BorrowItem> BorrowItems { get; set; }
    }
}