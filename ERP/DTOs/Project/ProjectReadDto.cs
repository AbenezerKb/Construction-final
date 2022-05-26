namespace ERP.DTOs
{
    public class ProjectReadDto
    {
        public string proId { get; set; }
        public string proName { get; set; }
        public DateTime dateOfStart { get; set; }
        public DateTime dateOfComplete { get; set; }
        public string proCoordinator { get; set; }
        public string proManager { get; set; }
        public string status { get; set; }
        public string siteId { get; set; }
    }
}
