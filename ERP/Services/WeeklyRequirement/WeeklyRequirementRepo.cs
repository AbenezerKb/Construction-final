using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class WeeklyRequirementRepo: IWeeklyRequirementRepo
    {


        private readonly AppDbContext _context;

        public WeeklyRequirementRepo(AppDbContext context)
        {
            _context = context;
        }


        public void CreateWeeklyRequirement(WeeklyRequirement weeklyRequirement)
        {
            if (weeklyRequirement == null)
            {
                throw new ArgumentNullException();
            }

            weeklyRequirement.Id = Guid.NewGuid().ToString();

            foreach (WeeklyMaterial s in weeklyRequirement.material)
            {
                s.materialId = Guid.NewGuid().ToString();
            }
            foreach (WeeklyLabor r in weeklyRequirement.labor)
            {
                r.laborId = Guid.NewGuid().ToString();
            }

            foreach (WeeklyEquipment w in weeklyRequirement.equipment)
            {
                w.equipmentId = Guid.NewGuid().ToString();
            }

            _context.WeeklyRequirements.Add(weeklyRequirement);
        }



        public WeeklyRequirement GetWeeklyRequirements(string id)
        {
            var weeklyRequirement = _context.WeeklyRequirements.FirstOrDefault(c => c.Id == id);
            var material = _context.WeeklyMaterials.ToList();
            var equipment = _context.WeeklyEquipments.ToList();
            var labor = _context.Labors.ToList();



            foreach (WeeklyMaterial scp in material)
            {
                if (scp.WeeklyRequirementFK == weeklyRequirement.Id)
                {
                    weeklyRequirement.material.Add(scp);
                }
            }


            foreach (WeeklyEquipment scp in equipment)
            {
                if (scp.WeeklyRequirementFK == weeklyRequirement.Id)
                {
                    weeklyRequirement.equipment.Add(scp);
                }
            }


            foreach (WeeklyLabor scp in labor)
            {
                if (scp.WeeklyRequirementFK == weeklyRequirement.Id)
                {
                    weeklyRequirement.labor.Add(scp);
                }
            }

            return weeklyRequirement;
        }

        public IEnumerable<WeeklyRequirement> GetAllWeeklyRequirement()
        {
            var weeklyRequirement = _context.WeeklyRequirements.ToList();
            var material = _context.WeeklyMaterials.ToList();
            var equipment = _context.WeeklyEquipments.ToList();
            var labor = _context.Labors.ToList();

            for (int i = 0; i < weeklyRequirement.Count; i++)
            {
                foreach (WeeklyMaterial scp in material)
                {
                    if (scp.WeeklyRequirementFK == weeklyRequirement[i].Id)
                    {
                        weeklyRequirement[i].material.Add(scp);
                    }
                }


                foreach (WeeklyEquipment scp in equipment)
                {
                    if (scp.WeeklyRequirementFK == weeklyRequirement[i].Id)
                    {
                        weeklyRequirement[i].equipment.Add(scp);
                    }
                }


                foreach (WeeklyLabor scp in labor)
                {
                    if (scp.WeeklyRequirementFK == weeklyRequirement[i].Id)
                    {
                        weeklyRequirement[i].labor.Add(scp);
                    }
                }


            }

            return weeklyRequirement;















            //return _context.WeeklyRequirements.ToList();
        }

        public void DeleteWeeklyRequirements(string id)
        {
            var weeklyRequirement = _context.WeeklyRequirements.FirstOrDefault(c => c.Id == id);
            _context.WeeklyRequirements.Remove(weeklyRequirement);
        }


        public void UpdateWeeklyRequirement(string id,WeeklyRequirement updatedWeeklyRequirement)
        {

            if (updatedWeeklyRequirement.Equals(null))
            {
                throw new ArgumentNullException();
            }

            WeeklyRequirement weeklyRequirements = _context.WeeklyRequirements.FirstOrDefault(c => c.Id == id);
            if (weeklyRequirements.Equals(null))
                throw new ItemNotFoundException($"WeeklyRequirement not found with WeeklyRequirement Id={id}");
            

            for (int i=0; i<weeklyRequirements.material.Count; i++)
            {
                for (int j = 0; j < updatedWeeklyRequirement.material.Count; j++)
                {
                    if (updatedWeeklyRequirement.material[j].materialId == weeklyRequirements.material[j].materialId)
                    weeklyRequirements.material[j].amount = updatedWeeklyRequirement.material[j].amount;
                    weeklyRequirements.material[j].budget = updatedWeeklyRequirement.material[j].budget;
                    weeklyRequirements.material[j].unit = updatedWeeklyRequirement.material[j].unit;                    
                }
             }
        


          for (int i=0; i<weeklyRequirements.equipment.Count; i++)
            {
                for (int j = 0; j<updatedWeeklyRequirement.equipment.Count; j++)
                {
                    if (updatedWeeklyRequirement.equipment[j].equipmentId == weeklyRequirements.equipment[j].equipmentId)
                    weeklyRequirements.equipment[j].amount = updatedWeeklyRequirement.equipment[j].amount;
                    weeklyRequirements.equipment[j].budget = updatedWeeklyRequirement.material[j].budget;
                    weeklyRequirements.equipment[j].unit = updatedWeeklyRequirement.equipment[j].unit;

                }
            }
        

           for (int i=0; i<weeklyRequirements.labor.Count; i++)
            {
                for (int j = 0; j<updatedWeeklyRequirement.labor.Count; j++)
                {
                    if (updatedWeeklyRequirement.labor[j].laborId == weeklyRequirements.labor[j].laborId)
                    weeklyRequirements.labor[j].number = updatedWeeklyRequirement.labor[j].number;
                    weeklyRequirements.labor[j].budget = updatedWeeklyRequirement.labor[j].budget;
                    weeklyRequirements.labor[j].labor = updatedWeeklyRequirement.labor[j].labor;                    

                }
                }
            

            weeklyRequirements.date = updatedWeeklyRequirement.date;          
            weeklyRequirements.projCoordinator = updatedWeeklyRequirement.projCoordinator;
            weeklyRequirements.projManager = updatedWeeklyRequirement.projManager;
            weeklyRequirements.projId = updatedWeeklyRequirement.projId;
            weeklyRequirements.remark = updatedWeeklyRequirement.remark;
            weeklyRequirements.specialRequest = updatedWeeklyRequirement.specialRequest;
            weeklyRequirements.weekNo = updatedWeeklyRequirement.weekNo;


            _context.WeeklyRequirements.Add(weeklyRequirements);
        }















        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }



	

}
}
