using ERP.Models;


namespace ERP.Services
{
    public interface IDailyLaborRepo
    {
        bool SaveChanges();
        public void CreateDailyLabor(DailyLabor grander);
        public DailyLabor GetDailyLabor(string granderNo);
        public IEnumerable<DailyLabor> GetAllDailyLabors();
        public void DeleteDailyLabor(string id);
        public void UpdateDailyLabor(string id,DailyLabor updatedDailyLabor);
    }
}
