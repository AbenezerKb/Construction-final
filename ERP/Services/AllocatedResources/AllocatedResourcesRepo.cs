using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class AllocatedResourcesRepo: IAllocatedResourcesRepo
    {
        private readonly AppDbContext _context;


        public AllocatedResourcesRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateAllocatedResources(AllocatedResources allocatedResources)
        {
            if (allocatedResources == null)
            {
                throw new ArgumentNullException();
            }         
            allocatedResources.allocatedResourcesNo = Guid.NewGuid().ToString();

            _context.AllocatedResources.Add(allocatedResources);
        }

        public IEnumerable<AllocatedResources> GetAllAllocatedResources()
        {
            return _context.AllocatedResources.ToList();
        }


        public AllocatedResources GetAllocatedResources(string id)
        {
            return _context.AllocatedResources.FirstOrDefault(c => c.allocatedResourcesNo == id);
        }


        public void DeleteAllocatedResource(string id)
        {
            var allocatedResources = _context.AllocatedResources.FirstOrDefault(c => c.allocatedResourcesNo == id);
            if (allocatedResources.Equals(null)){
                throw new ArgumentNullException();

            }
            _context.AllocatedResources.Remove(allocatedResources);
        }

        public void UpdateAllocatedResource(string id,AllocatedResources updatedallocatedResources)
        {

            if (updatedallocatedResources.Equals( null))
            {
                throw new ArgumentNullException();
            }

            AllocatedResources allocatedResources = _context.AllocatedResources.FirstOrDefault(c => c.allocatedResourcesNo == id);
            if (allocatedResources.Equals(null))
                throw new ItemNotFoundException($"Allocated budget not found with allocatedbudgetId={updatedallocatedResources.allocatedResourcesNo}");
            allocatedResources.date = updatedallocatedResources.date;
            allocatedResources.itemName = updatedallocatedResources.itemName;
            allocatedResources.projId = updatedallocatedResources.projId;
            allocatedResources.remark = updatedallocatedResources.remark;
            allocatedResources.unit = allocatedResources.unit;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
