using ERP.Models;

namespace ERP.Services.EquipmentCategoryServices
{
    public interface IEquipmentCategoryService
    {
        Task<List<EquipmentCategory>> GetAll();
        Task<EquipmentCategory> GetById(int id);
    }
}
