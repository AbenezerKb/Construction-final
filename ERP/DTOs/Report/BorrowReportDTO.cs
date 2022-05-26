namespace ERP.DTOs
{
    public class BorrowReportDTO
    {
        public int DateOf { get; set; } = -1;

        public string FromDate { get; set; } = "";

        public string ToDate { get; set; } = "";

        public int SiteId { get; set; } = -1;

        public int Status { get; set; } = -1;

        public int EquipmentCategoryId { get; set; } = -1;

        public int ItemId { get; set; } = -1;

        public int EmployeeRole { get; set; } = -1;

        public int EmployeeId { get; set; } = -1;

        public string GroupBy { get; set; } = "";

        public DateTime? RequestDateFrom;

        public DateTime? RequestDateTo;

        public DateTime? ApproveDateFrom;

        public DateTime? ApproveDateTo;

        public DateTime? HandDateFrom;

        public DateTime? HandDateTo;

        public int? RequestedById;

        public int? ApprovedById;

        public int? HandedById;


        public void SetDates()
        {
            if (DateOf != -1 && FromDate != "")
            {
                DateTime fromDate = DateTime.Parse(FromDate);

                if (DateOf == TRANSFERSTATUS.DECLINED)
                {
                    Status = TRANSFERSTATUS.DECLINED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == TRANSFERSTATUS.REQUESTED)
                    RequestDateFrom = fromDate;
                else if (DateOf == TRANSFERSTATUS.APPROVED)
                {
                    Status = TRANSFERSTATUS.APPROVED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == TRANSFERSTATUS.SENT)
                    HandDateFrom = fromDate;
            }

            if (DateOf != -1 && ToDate != "")
            {
                DateTime toDate = DateTime.Parse(ToDate).AddDays(1);

                if (DateOf == TRANSFERSTATUS.DECLINED)
                {
                    Status = TRANSFERSTATUS.DECLINED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == TRANSFERSTATUS.REQUESTED)
                    RequestDateTo = toDate;
                else if (DateOf == TRANSFERSTATUS.APPROVED)
                {
                    //Status = TRANSFERSTATUS.APPROVED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == TRANSFERSTATUS.SENT)
                    HandDateTo = toDate;
            }
        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                RequestedById = EmployeeId;
                ApprovedById = EmployeeId;
                HandedById = EmployeeId;
                return;
            }

            if (EmployeeRole != -1 && EmployeeId != -1)
            {
                if (EmployeeRole == TRANSFERSTATUS.DECLINED)
                {
                    Status = TRANSFERSTATUS.DECLINED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == TRANSFERSTATUS.REQUESTED)
                    RequestedById = EmployeeId;
                else if (EmployeeRole == TRANSFERSTATUS.APPROVED)
                {
                    //Status=TRANSFERSTATUS.APPROVED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == TRANSFERSTATUS.SENT)
                    HandedById = EmployeeId;

            }

        }
    }

}
