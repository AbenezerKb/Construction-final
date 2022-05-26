using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class LaborDetailRepo: ILaborDetailRepo
    {


        private readonly AppDbContext _context;

        public LaborDetailRepo(AppDbContext context)
        {
            _context = context;
        }


        public void CreateLaborDetail(LaborDetail laborDetail)
        {
            if (laborDetail == null)
            {
                throw new ArgumentNullException();
            }

            laborDetail.Id = Guid.NewGuid().ToString();           

            _context.LaborDetails.Add(laborDetail);
        }



        public LaborDetail GetLaborDetail(string id)
        {
            return _context.LaborDetails.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<LaborDetail> GetAllLaborDetails()
        {
            return _context.LaborDetails.ToList();
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeleteLaborDetails(string id)
        {
            var laborDetail = _context.LaborDetails.FirstOrDefault(c => c.Id == id);
            _context.LaborDetails.Remove(laborDetail);
        }



        public void UpdateLaborDetail(string id,LaborDetail updatedLaborDetail)
        {

            if (updatedLaborDetail.Equals(null))
            {
                throw new ArgumentNullException();
            }

            LaborDetail laborDetail = _context.LaborDetails.FirstOrDefault(c => c.Id == updatedLaborDetail.Id);
            if (laborDetail.Equals(null))
                throw new ItemNotFoundException($"LaborDetail not found with LaborDetail Id={updatedLaborDetail.Id}");
            laborDetail.afternoonSession = updatedLaborDetail.afternoonSession;
            laborDetail.dateOfWork = updatedLaborDetail.dateOfWork;
            laborDetail.dateType = updatedLaborDetail.dateType;
            laborDetail.eveningSession = updatedLaborDetail.eveningSession;
            laborDetail.morningSession = updatedLaborDetail.morningSession;
            laborDetail.NoOfHrsPerSession = updatedLaborDetail.NoOfHrsPerSession;
            laborDetail.weekNo = updatedLaborDetail.weekNo;

            _context.LaborDetails.Add(laborDetail);
        }




    }
}
