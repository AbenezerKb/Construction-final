using ERP.DTOs;
using ERP.Models;

namespace ERP.Services.ReturnServices
{
    public interface IReturnService
    {
        Task<List<Return>> GetByCondition();
        Task<Return> GetById(int id);
        Task<List<AssetDamage>> GetDamageTypes();
        Task<List<EquipmentAsset>> GetReturnableItems(GetReturnItemsDTO returnItemsDTO);
        Task<Return> ReturnEquipments(ReturnBorrowDTO returnDTO);
    }
}
