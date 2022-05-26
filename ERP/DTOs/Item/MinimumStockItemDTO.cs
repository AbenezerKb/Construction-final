namespace ERP.DTOs
{
    public class MinimumStockItemDTO
    {

        public string Name { get; set; }

        public int ItemType { get; set; } = -1;

        public int ItemId { get; set; }

        public int MinimumQty { get; set; }

        public int SiteId { get; set; }

        public int EquipmentModelId { get; set; }

        public int Qty { get; set; }

    }

}
