using ERP.Models;

namespace ERP.DTOs
{
    public class WeeklyRequirementReadDto
    {
        public string Id { get; set; }
        public string projName { get; set; }
        public string projManager { get; set; }
        public string projCoordinator { get; set; }

        public DateTime date { get; set; }
        public IList<WeeklyMaterialReadDto> material { get; set; }

        public IList<WeeklyEquipmentReadDto> equipment { get; set; }

        public IList<WeeklyLaborReadDto> labor { get; set; }
        public string specialRequest { get; set; }

        public string remark { get; set; }
        public string weekNo { get; set; }

    }

     public class WeeklyEquipmentReadDto
    {
        
        public string equipmentId { get; set; }
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //    public WeeklyRequirement WeeklyRequirement { get; set; }
    }




    public class WeeklyLaborReadDto
    {
        
        public string laborId { get; set; }
        public string WeeklyRequirementFK { get; set; }
        public string labor { get; set; }
        public int number { get; set; }
        public double budget { get; set; }
        // public WeeklyRequirement WeeklyRequirement { get; set; }
    }



    public class WeeklyMaterialReadDto
    {
        public string materialId { get; set; }
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //public WeeklyRequirement WeeklyRequirement { get; set; }
    }

}

