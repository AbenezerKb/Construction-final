using ERP.Models;

namespace ERP.Services
{
    public interface IGranderRepo
    {
        bool SaveChanges();
        public void CreateGrander(Grander grander);
        public Grander GetGrander(string granderNo);
        public IEnumerable<Grander> GetAllGranders();

        void DeleteGrander(string id);
        void UpdateGrander(string id,Grander updatedGrander);        
    }
}
