namespace ERP.DTOs
{
    public class MaintenanceReportDTO
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

        public DateTime? FixDateFrom;

        public DateTime? FixDateTo;

        public int? RequestedById;

        public int? ApprovedById;

        public int? FixedById;


        public void SetDates()
        {
            if (DateOf != -1 && FromDate != "")
            {
                DateTime fromDate = DateTime.Parse(FromDate);

                if (DateOf == MAINTENANCESTATUS.DECLINED)
                {
                    Status = MAINTENANCESTATUS.DECLINED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == MAINTENANCESTATUS.REQUESTED)
                    RequestDateFrom = fromDate;
                else if (DateOf == MAINTENANCESTATUS.APPROVED)
                {
                    //Status = MAINTENANCESTATUS.APPROVED;
                    ApproveDateFrom = fromDate;
                }
                else if (DateOf == MAINTENANCESTATUS.FIXED)
                    FixDateFrom = fromDate;
            }

            if (DateOf != -1 && ToDate != "")
            {
                DateTime toDate = DateTime.Parse(ToDate).AddDays(1);

                if (DateOf == MAINTENANCESTATUS.DECLINED)
                {
                    Status = MAINTENANCESTATUS.DECLINED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == MAINTENANCESTATUS.REQUESTED)
                    RequestDateTo = toDate;
                else if (DateOf == MAINTENANCESTATUS.APPROVED)
                {
                    //Status = MAINTENANCESTATUS.APPROVED;
                    ApproveDateTo = toDate;
                }
                else if (DateOf == MAINTENANCESTATUS.FIXED)
                    FixDateTo = toDate;
            }
        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                RequestedById = EmployeeId;
                ApprovedById = EmployeeId;
                FixedById = EmployeeId;
                return;
            }

            if (EmployeeRole != -1 && EmployeeId != -1)
            {
                if (EmployeeRole == MAINTENANCESTATUS.DECLINED)
                {
                    Status = MAINTENANCESTATUS.DECLINED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == MAINTENANCESTATUS.REQUESTED)
                    RequestedById = EmployeeId;
                else if (EmployeeRole == MAINTENANCESTATUS.APPROVED)
                {
                    //Status=MAINTENANCESTATUS.APPROVED;
                    ApprovedById = EmployeeId;
                }
                else if (EmployeeRole == MAINTENANCESTATUS.FIXED)
                    FixedById = EmployeeId;

            }

        }
    }

}
