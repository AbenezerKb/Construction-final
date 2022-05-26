using ERP.DTOs.Item;
using ERP.Models;

namespace ERP.Services.DamageServices
{
    public interface IDamageService
    {
        Task<AssetDamage> AddDamage(AddDamageDTO damageDTO);
        Task<List<AssetDamage>> GetDamages();
        Task<bool> UpdateDamages(List<AssetDamage> damagesDTO);
    }
}
