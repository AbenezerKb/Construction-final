using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class WeeklyRequirementCreateDto
    { 
        [Required]
        public string projName { get; set; }
    [Required]
    public string projManager { get; set; }

        public string projCoordinator { get; set; }

        public DateTime date { get; set; }
        [Required]
    public IList<WeeklyMaterialCreateDto> material { get; set; }

   // 
    //public string[] materialNo { get; set; }

    [Required]
    public IList<WeeklyEquipmentCreateDto> equipment { get; set; }

    

    //public string[] equipmentNo { get; set; }
    [Required]
    public IList<WeeklyLaborCreateDto> labor { get; set; }

        //public string[] laborNo { get; set; }
        [Required]
        public string specialRequest { get; set; }

        [Required]
        public string remark { get; set; }

        [Required]
        public string weekNo { get; set; }

    }





    public class WeeklyEquipmentCreateDto
    {        
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //    public WeeklyRequirement WeeklyRequirement { get; set; }
    }




    public class WeeklyLaborCreateDto
    {

        public string WeeklyRequirementFK { get; set; }
        public string labor { get; set; }
        public int number { get; set; }
        public double budget { get; set; }
        // public WeeklyRequirement WeeklyRequirement { get; set; }
    }



    public class WeeklyMaterialCreateDto
    {        
        public string WeeklyRequirementFK { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public double budget { get; set; }
        //public WeeklyRequirement WeeklyRequirement { get; set; }
    }









}
