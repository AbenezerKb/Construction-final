namespace ERP.DTOs.Item
{
    public class ImportAssetNoDTO
    {

        public int EquipmentModelId { get; set; } = -1;

        public int SiteId { get; set; } = -1;

        public ICollection<ImportAssetNoItem> Assets { get; set; }

    }

    public class ImportAssetNoItem{

        public string AssetNo { get; set; }

        public string SerialNo { get; set; }
}
}
