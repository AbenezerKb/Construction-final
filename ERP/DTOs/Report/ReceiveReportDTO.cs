namespace ERP.DTOs
{
    public class ReceiveReportDTO
    {
        public int DateOf { get; set; } = -1;

        public string FromDate { get; set; } = "";

        public string ToDate { get; set; } = "";

        public int SiteId { get; set; } = -1;

        public int Status { get; set; } = -1;

        public int ItemType { get; set; } = -1;

        public int EquipmentCategoryId { get; set; } = -1;

        public int ItemId { get; set; } = -1;

        public int EmployeeRole { get; set; } = -1;

        public int EmployeeId { get; set; } = -1;

        public string GroupBy { get; set; } = "";

        public DateTime? PurchaseDateFrom;

        public DateTime? PurchaseDateTo;

        public DateTime? ApproveDateFrom;

        public DateTime? ApproveDateTo;

        public DateTime? ReceiveDateFrom;

        public DateTime? ReceiveDateTo;

        public int? PurchasedById;

        public int? ApprovedById;

        public int? ReceivedById;


        public void SetDates()
        {
            if (DateOf != -1 && FromDate != "")
            {
                DateTime fromDate = DateTime.Parse(FromDate);

                if (DateOf == RECEIVESTATUS.PURCHASED)
                    PurchaseDateFrom = fromDate;
                else if (DateOf == RECEIVESTATUS.APPROVED)
                {
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == RECEIVESTATUS.RECEIVED)
                    ReceiveDateFrom = fromDate;
            }

            if (DateOf != -1 && ToDate != "")
            {
                DateTime toDate = DateTime.Parse(ToDate).AddDays(1);

                if (DateOf == RECEIVESTATUS.PURCHASED)
                    PurchaseDateTo = toDate;
                else if (DateOf == RECEIVESTATUS.APPROVED)
                {
                    ApproveDateTo = toDate;
                }
                else if (DateOf == RECEIVESTATUS.RECEIVED)
                    ReceiveDateTo = toDate;
            }
        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                PurchasedById = EmployeeId;
                ApprovedById = EmployeeId;
                ReceivedById = EmployeeId;
                return;
            }

            if (EmployeeRole != -1 && EmployeeId != -1)
            {
                if (EmployeeRole == RECEIVESTATUS.PURCHASED)
                    PurchasedById = EmployeeId;
                else if (EmployeeRole == RECEIVESTATUS.APPROVED)
                {
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == RECEIVESTATUS.RECEIVED)
                    ReceivedById = EmployeeId;

            }

        }
    }

}
