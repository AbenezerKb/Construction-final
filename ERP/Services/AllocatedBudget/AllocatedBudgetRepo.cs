using ERP.Context;
using ERP.Exceptions;
using ERP.Models;

namespace ERP.Services
{
    public class AllocatedBudgetRepo : IAllocatedBudgetRepo
    {
        private readonly AppDbContext _context;


        public AllocatedBudgetRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateAllocatedBudget(AllocatedBudget allocatedBudget)
        {
            if (allocatedBudget == null)
            {
                throw new ArgumentNullException();
            }
            allocatedBudget.Id = Guid.NewGuid().ToString();

             _context.AllocatedBudgets.Add(allocatedBudget);
        }

        public IEnumerable<AllocatedBudget> GetAllAllocatedBudgets()
        {

           var list =   _context.AllocatedBudgets.ToList();
            return list;
        }


        public AllocatedBudget GetAllocatedBudget(string id)
        {
           var allocatedBudget = _context.AllocatedBudgets.FirstOrDefault(c => c.Id == id);
            if (allocatedBudget.Equals(null))
                throw new ItemNotFoundException($"Allocated budget not found with allocatedbudgetId={id}");
            return allocatedBudget;
        }

       public void DeleteAllocatedBudget(string id)
        {
            var allocatedBudget =  _context.AllocatedBudgets.FirstOrDefault(c => c.Id == id);
            if (allocatedBudget.Equals(null))
                throw new ItemNotFoundException($"Allocated budget not found with allocatedbudgetId={id}");
            _context.AllocatedBudgets.Remove(allocatedBudget);
        }

        
        public void UpdateAllocatedBudget(string id,AllocatedBudget updateAllocatedBudget)
        {
            if (updateAllocatedBudget.Equals( null))
            {
                throw new ArgumentNullException();
            }

            AllocatedBudget allocatedBudget = _context.AllocatedBudgets.FirstOrDefault(c => c.Id == id);
            if (allocatedBudget.Equals(null))
                throw new ItemNotFoundException($"Allocated budget not found with allocatedbudgetId={id}");
            allocatedBudget.date = updateAllocatedBudget.date;
            allocatedBudget.projectId = updateAllocatedBudget.projectId;
            allocatedBudget.activity = updateAllocatedBudget.activity;
            allocatedBudget.amount = updateAllocatedBudget.amount;            
            allocatedBudget.contingency = updateAllocatedBudget.contingency;
            allocatedBudget.preparedBy = updateAllocatedBudget.preparedBy;
            allocatedBudget.ApprovedBy = updateAllocatedBudget.ApprovedBy;
            _context.AllocatedBudgets.Add(allocatedBudget);
        }
                            


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
