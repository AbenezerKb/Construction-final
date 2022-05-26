using ERP.Models;

namespace ERP.Services
{
    public interface IContractRepo
    {
        bool SaveChanges();
        IEnumerable<Contract> GetAllContract();
        Contract GetContract(string id);
        void CreateContract(Contract contract);
        void DeleteContract(string id);
        void UpdateContract(string id,Contract updatedContract);
    }

}
