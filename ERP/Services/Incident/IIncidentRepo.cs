using ERP.Models;
namespace ERP.Services
{
    public interface IIncidentRepo
    {
        bool SaveChanges();
        public void CreateIncident(Incident incident);
        public Incident GetIncident(string incidentNo);
        public IEnumerable<Incident> GetAllIncidents();
        void DeleteIncident(string id);
        void UpdateIncident(string id,Incident updatedIncident);
    }
}
