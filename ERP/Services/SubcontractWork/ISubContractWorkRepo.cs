using ERP.Models;


namespace ERP.Services
{
    public interface ISubContractWorkRepo
    {

        bool SaveChanges();

        IEnumerable<SubContractWork> GetAllSubContractWorks();
        SubContractWork GetSubContractWork(string id);
        void CreateSubContractWork(SubContractWork contract);
        void DeleteSubContractWorks(string id);
        void UpdateSubContractWork(string id, SubContractWork updatedSubContractWork);
    }
}
