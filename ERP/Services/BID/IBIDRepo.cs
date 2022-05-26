using ERP.Models;

namespace ERP.Services
{
    public interface IBIDRepo
    {
        bool SaveChanges();

        IEnumerable<BID> GetAllBIDs();
        BID GetBID(string id);
        void CreateBID(BID bid);
        void DeleteBID(string id);
        void UpdateBID(string id,BID updateBID);
    }
}
