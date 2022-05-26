using ERP.Models;

namespace ERP.Services
{
    public interface ISubContractorRepo
    {
        bool SaveChanges();

        IEnumerable<SubContractor> GetAllSubContractors();
        SubContractor GetSubContractor(string id);
        void CreateSubContractor(SubContractor contract);
        void DeleteSubContractor(string id);
        void UpdateSubContractor(string id,SubContractor updatedSubContractor);
    }
}
