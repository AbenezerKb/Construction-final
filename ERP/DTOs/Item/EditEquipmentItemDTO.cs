﻿namespace ERP.DTOs.Item
{
    public class EditEquipmentItemDTO
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public string Description { get; set; }

        public int EquipmentCategoryId { get; set; }

    }
}
