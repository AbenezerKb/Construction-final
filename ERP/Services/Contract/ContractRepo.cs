using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class ContractRepo : IContractRepo
    {
        private readonly AppDbContext _context;

        public ContractRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateContract(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException();
            }
            contract.ConId = Guid.NewGuid().ToString();

            contract.Date = DateTime.Now.Date;//DateTime.Now.ToString("yyyy-MM-dd");
           
            foreach (SubContractingWork scw in contract.SubConstructWorkDetail)
            {
                scw.ContractID = contract.ConId;               
                _context.SubcontractingWorks.Add(scw);
            }
            
            _context.Contracts.Add(contract);
        }



        public IEnumerable<Contract> GetAllContract()
        {
          /*
            Contract contracts = _context.Contracts.ToList();
            foreach (SubContractingWork scw in contracts.SubConstructWorkDetail)
            {
                scw.ContractID = contracts.ConId;
                //w.assigneWorkForceNo = assignedWorkForce.assigneWorkForceNo;
                _context.WorkForces.Add(w);
            }
            //return _context.Contracts.ToList();


            */

            var contracts = _context.Contracts.ToList();
            var workwithForceList = _context.SubcontractingWorks.ToList();
            for (int i = 0; i < contracts.Count; i++)
            {
                foreach (SubContractingWork scw in workwithForceList)
                {
                    if (scw.ContractID == contracts[i].ConId)
                    {
                        contracts[i].SubConstructWorkDetail.Add(scw);
                    }
                }

            }

            return contracts;


        }


        public Contract GetContract(string id)
        {

            var contract = _context.Contracts.FirstOrDefault(c => c.ConId == id);
            var scw = _context.SubcontractingWorks.ToList();
            foreach (SubContractingWork aw in scw)
            {
                if (aw.ContractID == contract.ConId)
                {
                    contract.SubConstructWorkDetail.Add(aw);
                }
            }
            return contract;
           
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

       public void DeleteContract(string id)
        {
            var contract = _context.Contracts.FirstOrDefault(c => c.ConId == id);
            _context.Contracts.Remove(contract);
        }

      public  void UpdateContract(string id, Contract updatedContract)
        {

            if (updatedContract.Equals(null))
            {
                throw new ArgumentNullException();
            }

            Contract contract = _context.Contracts.FirstOrDefault(c => c.ConId == id);
            if (contract.Equals(null))
                throw new ItemNotFoundException($"Contract not found with ContractId={id}");
            contract.ConGiver = updatedContract.ConGiver;
            contract.ConReciever = updatedContract.ConReciever;
            contract.ConType = updatedContract.ConType;
            contract.Cost = updatedContract.Cost;
            contract.Date = updatedContract.Date;
            contract.SubConstructWorkDetail = updatedContract.SubConstructWorkDetail;
            contract.Unit = updatedContract.Unit;
            _context.Contracts.Add(contract);
        }
    }
}