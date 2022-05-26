namespace ERP.DTOs
{
    public class TransferReportDTO
    {
        public int DateOf { get; set; } = -1;

        public string FromDate { get; set; } = "";

        public string ToDate { get; set; } = "";

        public int SendSiteId { get; set; } = -1;

        public int ReceiveSiteId { get; set; } = -1;

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

        public DateTime? SendDateFrom;

        public DateTime? SendDateTo;

        public DateTime? ReceiveDateFrom;

        public DateTime? ReceiveDateTo;

        public int? RequestedById;

        public int? ApprovedById;

        public int? SentById;

        public int? ReceivedById;


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
                    SendDateFrom = fromDate;
                else if (DateOf == TRANSFERSTATUS.RECEIVED)
                    ReceiveDateFrom = fromDate;
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
                    SendDateTo = toDate;
                else if (DateOf == TRANSFERSTATUS.RECEIVED)
                    ReceiveDateTo = toDate;
            }
        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                RequestedById = EmployeeId;
                ApprovedById = EmployeeId;
                SentById = EmployeeId;
                ReceivedById = EmployeeId;
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
                    SentById = EmployeeId;
                else if (EmployeeRole == TRANSFERSTATUS.RECEIVED)
                    ReceivedById = EmployeeId;

            }

        }
    }

}
