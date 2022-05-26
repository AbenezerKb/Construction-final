namespace ERP.DTOs
{
    public class PurchaseReportDTO
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

        public DateTime? RequestDateFrom;

        public DateTime? RequestDateTo;

        public DateTime? ApproveDateFrom;

        public DateTime? ApproveDateTo;

        public DateTime? CheckDateFrom;

        public DateTime? CheckDateTo;

        public DateTime? PurchaseDateFrom;

        public DateTime? PurchaseDateTo;

        public int? RequestedById;

        public int? ApprovedById;

        public int? CheckedById;

        public int? PurchasedById;


        public void SetDates()
        {
            if (DateOf != -1 && FromDate != "")
            {
                DateTime fromDate = DateTime.Parse(FromDate);

                if (DateOf == PURCHASESTATUS.DECLINED)
                {
                    Status = PURCHASESTATUS.DECLINED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == PURCHASESTATUS.REQUESTED)
                    RequestDateFrom = fromDate;
                else if (DateOf == PURCHASESTATUS.APPROVED)
                {
                    Status = PURCHASESTATUS.APPROVED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == PURCHASESTATUS.CHECKED)
                    CheckDateFrom = fromDate;
                else if (DateOf == PURCHASESTATUS.PURCHASED)
                    PurchaseDateFrom = fromDate;
            }

            if (DateOf != -1 && ToDate != "")
            {
                DateTime toDate = DateTime.Parse(ToDate).AddDays(1);

                if (DateOf == PURCHASESTATUS.DECLINED)
                {
                    Status = PURCHASESTATUS.DECLINED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == PURCHASESTATUS.REQUESTED)
                    RequestDateTo = toDate;
                else if (DateOf == PURCHASESTATUS.APPROVED)
                {
                    //Status = TRANSFERSTATUS.APPROVED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == PURCHASESTATUS.CHECKED)
                    CheckDateTo = toDate;
                else if (DateOf == PURCHASESTATUS.PURCHASED)
                    PurchaseDateTo = toDate;
            }
        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                RequestedById = EmployeeId;
                ApprovedById = EmployeeId;
                CheckedById = EmployeeId;
                PurchasedById = EmployeeId;
                return;
            }

            if (EmployeeRole != -1 && EmployeeId != -1)
            {
                if (EmployeeRole == PURCHASESTATUS.DECLINED)
                {
                    Status = PURCHASESTATUS.DECLINED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == PURCHASESTATUS.REQUESTED)
                    RequestedById = EmployeeId;
                else if (EmployeeRole == PURCHASESTATUS.APPROVED)
                {
                    //Status=TRANSFERSTATUS.APPROVED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == PURCHASESTATUS.CHECKED)
                    CheckedById = EmployeeId;
                else if (EmployeeRole == PURCHASESTATUS.PURCHASED)
                    PurchasedById = EmployeeId;

            }

        }
    }

}
