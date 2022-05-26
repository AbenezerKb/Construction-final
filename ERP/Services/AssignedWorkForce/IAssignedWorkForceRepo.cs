using ERP.Models;
namespace ERP.Services
{
    public interface IAssignedWorkForceRepo
    {
        bool SaveChanges();
        IEnumerable<AssignedWorkForce> GetAllAssignedWorkForces();
        AssignedWorkForce GetAssignedWorkForce(string id);
        void CreateAssignedWorkForce(AssignedWorkForce assignedWork);
        public void DeleteAssignedWorkForce(string id);
        void UpdateAssignedWorkForce(string id,AssignedWorkForce updatedAssignedWorkForce);
    }
}
