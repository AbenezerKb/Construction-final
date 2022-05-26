using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Type { get; set; }

        public Materials Material { get; set; }

        public Equipment Equipment { get; set; }

        public ICollection<TransferItem> TransferItems { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }

        public ICollection<BorrowItem> BorrowItems { get; set; }

        public ICollection<BulkPurchaseItem> BulkPurchaseItems { get; set; }
        
        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        public ICollection<BuyItem> BuyItems { get; set; }

        public ICollection<MaterialSiteQty> MaterialSiteQties { get; set; }

        public ICollection<EquipmentSiteQty> EquipmentSiteQties { get; set; }

    }
}
