namespace ERP.DTOs.TaskSchedule
{
    public class TaskScheduleDto
    {
        public string TaskName { get; set; } = string.Empty;
        public int Priority { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

}