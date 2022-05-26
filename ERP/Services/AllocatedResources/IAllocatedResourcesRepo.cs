using ERP.Models;

namespace ERP.Services
{
    public interface IAllocatedResourcesRepo
    {
        bool SaveChanges();
        IEnumerable<AllocatedResources> GetAllAllocatedResources();
        AllocatedResources GetAllocatedResources(string id);
        void CreateAllocatedResources(AllocatedResources allocatedResources);
        void DeleteAllocatedResource(string id);
        void UpdateAllocatedResource(string id,AllocatedResources allocatedResources);
    }
}

