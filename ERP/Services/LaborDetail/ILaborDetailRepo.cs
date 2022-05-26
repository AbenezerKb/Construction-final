using ERP.Models;


namespace ERP.Services
{
    public interface ILaborDetailRepo
    {

        bool SaveChanges();

        IEnumerable<LaborDetail> GetAllLaborDetails();
        LaborDetail GetLaborDetail(string id);
        void CreateLaborDetail(LaborDetail laborDetail);
        void DeleteLaborDetails(string id);
        void UpdateLaborDetail(string id, LaborDetail updatedLaborDetail);
    }
}
