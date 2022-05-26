using ERP.Context;
using ERP.Exceptions;
using ERP.Models;

namespace ERP.Services
{
    public class SubContractWorkRepo : ISubContractWorkRepo
    {
        private readonly AppDbContext _context;

        public SubContractWorkRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateSubContractWork(SubContractWork subContractWork)
        {
            if (subContractWork == null)
            {
                throw new ArgumentNullException();
            }

            subContractWork.subconractingid = Guid.NewGuid().ToString();
            _context.SubContractWorks.Add(subContractWork);
        }


        public IEnumerable<SubContractWork> GetAllSubContractWorks()
        {
            return _context.SubContractWorks.ToList();
        }


        public SubContractWork GetSubContractWork(string id)
        {
            return _context.SubContractWorks.FirstOrDefault(c => c.subconractingid == id);
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }




        public void UpdateSubContractWork(string id,SubContractWork updatedSubContractWork)
        {

            if (updatedSubContractWork.Equals(null))
            {
                throw new ArgumentNullException();
            }

            SubContractWork subContractWork = _context.SubContractWorks.FirstOrDefault(c => c.subconractingid == id);
            if (subContractWork.Equals(null))
                throw new ItemNotFoundException($"SubContractWork not found with SubContractWork Id={updatedSubContractWork.subconractingid}");


            subContractWork.remarks = updatedSubContractWork.remarks;
            subContractWork.workName = updatedSubContractWork.workName;            

            _context.SubContractWorks.Add(subContractWork);
        }





        public void DeleteSubContractWorks(string id)
        {
            var subContractWork = _context.SubContractWorks.FirstOrDefault(c => c.subconractingid == id);
            _context.SubContractWorks.Remove(subContractWork);
        }


    }
}
