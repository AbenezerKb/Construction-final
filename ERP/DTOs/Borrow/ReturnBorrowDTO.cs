namespace ERP.DTOs
{
    public class ReturnBorrowDTO
    {
        public int RequestedById { get; set; }

        public int ReturnedById { get; set; }

        public int SiteId { get; set; }

        public ICollection<ReturnBorrowAsset> BorrowAssets { get; set; }

    }

    public class ReturnBorrowAsset
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public int EquipmentAssetId { get; set; }

        public int AssetDamageId { get; set; } = -1;

        public string FileName { get; set; } = "";

        public string ReturnRemark { get; set; }


    }
}
