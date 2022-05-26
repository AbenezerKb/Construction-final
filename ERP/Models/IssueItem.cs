using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class IssueItem
    {
        [Key]
        public int IssueId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int? QtyApproved { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

        public string? ApproveRemark { get; set; } = string.Empty;

        public string? HandRemark { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("IssueId")]
        public Issue Issue { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

    }
}
