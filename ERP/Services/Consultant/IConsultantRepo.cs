using ERP.Models;


namespace ERP.Services
{
    public interface IConsultantRepo
    {       
            bool SaveChanges();
            IEnumerable<Consultant> GetAllConsultant();
            Consultant GetConsultant(string id);
            void CreateConsultant(Consultant consultant);
            void DeleteConsultant(string id);
            void UpdateConsultant(string id,Consultant updatedConsultant);
        }
    }

