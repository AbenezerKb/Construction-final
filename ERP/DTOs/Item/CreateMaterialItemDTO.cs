namespace ERP.DTOs.Item
{
    public class CreateMaterialItemDTO
    {
        public string Name { get; set; }

        public string Spec { get; set; }
        
        public string Unit { get; set; }

        public bool IsTransferable { get; set; }

        public decimal Cost { get; set; }

    }
}
