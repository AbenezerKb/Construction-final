using ERP.Models;


namespace ERP.Services
{
    public interface IAllocatedBudgetRepo
    {
        bool SaveChanges();
        IEnumerable<AllocatedBudget> GetAllAllocatedBudgets();
        AllocatedBudget GetAllocatedBudget(string id);
        void CreateAllocatedBudget(AllocatedBudget allocatedBudget);
        void DeleteAllocatedBudget(string id);
        void UpdateAllocatedBudget(string id,AllocatedBudget allocatedBudget);
    }
}
