namespace ERP.DTOs
{
    public class DailyLaborReadDto
    {
        public string LaborerID { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }
        public string jobTitle { get; set; }
        public double wagePerhour { get; set; }
        public DateTime date { get; set; }
        public string remarks { get; set; }
    }
}
