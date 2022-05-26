using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class ConsultantRepo: IConsultantRepo
    {


        private readonly AppDbContext _context;

        public ConsultantRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateConsultant(Consultant consultant)
        {
            if (consultant == null)
            {
                throw new ArgumentNullException();
            }
            consultant.consultantId = Guid.NewGuid().ToString();

            foreach (ApprovedWorkList awl in consultant.approvedWorkList)
            {                
                awl.ApprovedWorkId = Guid.NewGuid().ToString();
                awl.ConsultantId = consultant.consultantId;
                _context.ApprovedWorkLists.Add(awl);
            }

            foreach (DeclinedWorkList dwl in consultant.declinedWorkList)
            {
                dwl.DeclinedWorkId = Guid.NewGuid().ToString();
                dwl.ConsultantId = consultant.consultantId;
                _context.DeclinedWorkLists.Add(dwl);
            }

            foreach (DefectsCorrectionlist dcl in consultant.defectsCorrectionlist)
            {
                dcl.DefectsCorrectionId = Guid.NewGuid().ToString();
                dcl.ConsultantId = consultant.consultantId;
                _context.DefectsCorrectionlists.Add(dcl);
            }

            //consultant. = DateTime.Now.Date;//DateTime.Now.ToString("yyyy-MM-dd");

            _context.Consultants.Add(consultant);
        }



        public IEnumerable<Consultant> GetAllConsultant()
        {

            var consultants = _context.Consultants.ToList();
            var approvedWorkLists = _context.ApprovedWorkLists.ToList();
            var declinedWorkLists = _context.DeclinedWorkLists.ToList();
            var defectsCorrectionlists = _context.DefectsCorrectionlists.ToList();

            for (int i = 0; i < consultants.Count; i++)
            {
                foreach (ApprovedWorkList awl in approvedWorkLists)
                {
                    if (consultants[i].consultantId == awl.ConsultantId)
                    {                        
                        consultants[i].approvedWorkList.Add(awl);
                    }
                }


                foreach (DeclinedWorkList dwl in declinedWorkLists)
                {
                    if (consultants[i].consultantId == dwl.ConsultantId)
                    {
                        consultants[i].declinedWorkList.Add(dwl);
                    }
                }

                foreach (DefectsCorrectionlist dcl in defectsCorrectionlists)
                {
                    if (consultants[i].consultantId == dcl.ConsultantId)
                    {
                        consultants[i].defectsCorrectionlist.Add(dcl);
                    }
                }

            }

            return consultants;
        }


        public Consultant GetConsultant(string id)
        {

            var consultants = _context.Consultants.FirstOrDefault(c => c.consultantId == id);
            var approvedWorkLists = _context.ApprovedWorkLists.ToList();
            var declinedWorkLists = _context.DeclinedWorkLists.ToList();
            var defectsCorrectionlists = _context.DefectsCorrectionlists.ToList();


            foreach (ApprovedWorkList awl in approvedWorkLists)
            {
                if (consultants.consultantId == awl.ConsultantId)
                {
                    consultants.approvedWorkList.Add(awl);
                }
            }


            foreach (DeclinedWorkList dwl in declinedWorkLists)
            {
                if (consultants.consultantId == dwl.ConsultantId)
                {
                    consultants.declinedWorkList.Add(dwl);
                }
            }

            foreach (DefectsCorrectionlist dcl in defectsCorrectionlists)
            {
                if (consultants.consultantId == dcl.ConsultantId)
                {
                    consultants.defectsCorrectionlist.Add(dcl);
                }
            }


            return consultants;
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeleteConsultant(string id)
        {
            var consultant = _context.Consultants.FirstOrDefault(c => c.consultantId == id);
            _context.Consultants.Remove(consultant);
        }




        public void UpdateConsultant(string id,Consultant updatedConsultant)
        {
            if (updatedConsultant.Equals(null))
            {
                throw new ArgumentNullException();
            }

            Consultant consultant = _context.Consultants.FirstOrDefault(c => c.consultantId == id);
            if (consultant.Equals(null))
                throw new ItemNotFoundException($"Consultant not found with Consultant Id={updatedConsultant.consultantId}");          

            consultant.approvedWorkList = updatedConsultant.approvedWorkList;
            consultant.changesTaken = updatedConsultant.changesTaken;
            consultant.contractorId = updatedConsultant.contractorId;
            consultant.declinedWorkList = updatedConsultant.declinedWorkList;
            consultant.defectsCorrectionlist = updatedConsultant.defectsCorrectionlist;
            consultant.defectsSeen = updatedConsultant.defectsSeen;
            consultant.nextWork = updatedConsultant.nextWork;
            consultant.projectId = updatedConsultant.projectId;
            consultant.reasonForChange = updatedConsultant.reasonForChange;
            consultant.remarks = updatedConsultant.remarks;
            consultant.reviewDate = updatedConsultant.reviewDate;

            _context.Consultants.Add(consultant);
        }






    }
}
