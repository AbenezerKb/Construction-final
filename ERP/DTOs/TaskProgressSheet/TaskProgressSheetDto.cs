namespace ERP.DTOs.TaskProgressSheet
{
    public class TaskProgressSheetDto
    {
        public int MainTaskId { get; set; }
        public string MainTaskName { get; set; } = string.Empty;
        public float Progress { get; set; }
    }

}