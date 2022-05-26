namespace ERP.DTOs
{
    public class GeneralReportDTO
    {
        public string FromDate { get; set; } = "";

        public string ToDate { get; set; } = "";

        public int SiteId { get; set; } = -1;

        public int ItemType { get; set; } = -1;

        public int EquipmentCategoryId { get; set; } = -1;

        public int ItemId { get; set; } = -1;

        public int EmployeeId { get; set; } = -1;

        public string GroupBy { get; set; } = "";

        public List<string> Include { get; set; } = new List<string>();

        public DateTime? DateFrom;

        public DateTime? DateTo;


        public void SetDates()
        {
            if (FromDate != "")
                DateFrom = DateTime.Parse(FromDate);

            if (ToDate != "")
                DateTo = DateTime.Parse(ToDate).AddDays(1);

        }
               
    }

}
