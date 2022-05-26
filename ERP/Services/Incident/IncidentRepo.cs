using ERP.Context;
using ERP.Exceptions;
using ERP.Models;

namespace ERP.Services
{
    public class IncidentRepo:IIncidentRepo
    {
        private readonly AppDbContext _context;
        public IncidentRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateIncident(Incident incident )
        {
            if (incident == null)
            {
                throw new ArgumentNullException();
            }
            incident.incidentNo = Guid.NewGuid().ToString();
            //project = DateTime.Now.ToString("yyyy-MM-dd");

            _context.Incidents.Add(incident);

        }

        public Incident GetIncident(string incidentNo)
        {
            return _context.Incidents.FirstOrDefault(c => c.incidentNo == incidentNo);

        }

        public IEnumerable<Incident> GetAllIncidents()
        {
            return _context.Incidents.ToList();

        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }


        public void DeleteIncident(string id)
        {
            var incident = _context.Incidents.FirstOrDefault(c => c.incidentNo == id);
            _context.Incidents.Remove(incident);
        }



        public void UpdateIncident(string id,Incident updatedIncident)
        {

            if (updatedIncident.Equals(null))
            {
                throw new ArgumentNullException();
            }

            Incident incident = _context.Incidents.FirstOrDefault(c => c.incidentNo == updatedIncident.incidentNo);
            if (incident.Equals(null))
                throw new ItemNotFoundException($"Incident not found with Incident Id={updatedIncident.incidentNo}");

            incident.date = updatedIncident.date;
            incident.Description = updatedIncident.Description;
            incident.empName = updatedIncident.empName;
            incident.incidentName = updatedIncident.incidentName;
            incident.proID = updatedIncident.proID;
            _context.Incidents.Add(incident);
        }




    }
}
