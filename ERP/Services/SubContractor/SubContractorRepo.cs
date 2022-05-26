using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class SubContractorRepo: ISubContractorRepo
    {
        private readonly AppDbContext _context;

        public SubContractorRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateSubContractor(SubContractor subcontract)
        {
            if (subcontract == null){
                throw new ArgumentNullException();
            }

            subcontract.SubId = Guid.NewGuid().ToString();
            _context.SubContractors.Add(subcontract);
        }



        public IEnumerable<SubContractor> GetAllSubContractors()
        {
            return _context.SubContractors.ToList();
        }


        public SubContractor GetSubContractor(string id)
        {
            return _context.SubContractors.FirstOrDefault(c => c.SubId == id);
        }
        
        public void DeleteSubContractor(string id)
        {
            var subContractor = _context.SubContractors.FirstOrDefault(c => c.SubId == id);
            _context.SubContractors.Remove(subContractor);
        }


        public void UpdateSubContractor(string id,SubContractor updatedSubContractor)
        {

            if (updatedSubContractor.Equals(null))
            {
                throw new ArgumentNullException();
            }

            SubContractor subContractor = _context.SubContractors.FirstOrDefault(c => c.SubId == id);
            if (subContractor.Equals(null))
                throw new ItemNotFoundException($"SubContractor not found with SubContractor Id={updatedSubContractor.SubId}");

            subContractor.Status = updatedSubContractor.Status;
            subContractor.SubAddress = updatedSubContractor.SubAddress;
            subContractor.SubName = updatedSubContractor.SubName;
            subContractor.SubWorkId = updatedSubContractor.SubWorkId;            


            _context.SubContractors.Add(subContractor);
        }





        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        
    }
}