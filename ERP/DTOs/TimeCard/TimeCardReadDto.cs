namespace ERP.DTOs
{
    public class TimeCardReadDto
    {
        public string Id { get; set; }
        public DateTime dateOfWork { get; set; }
        public string employeeName { get; set; }
        public string jobType { get; set; }
        public int weekNo { get; set; }
        public int NoOfPresents { get; set; }
        public int NoOfAbscents { get; set; }
        public int NoOfHrsPerSession { get; set; }
        public int totalWorkedHrs { get; set; }
        public double wages { get; set; }
        public double totalPayment { get; set; }

        //FK
        public string preparedByFK { get; set; }

        //FK
        public string approvedBy { get; set; }

        //FK
        public string LaborerID { get; set; }

        public string remark { get; set; }
    }
}
