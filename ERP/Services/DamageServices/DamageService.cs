using ERP.Context;
using ERP.DTOs.Item;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.DamageServices
{
    public class DamageService : IDamageService
    {
        private readonly DataContext _context;

        public DamageService(DataContext context)
        {
            _context = context;
        }

        public async Task<AssetDamage> AddDamage (AddDamageDTO damageDTO)
        {
            AssetDamage damage = new AssetDamage();
            damage.Name = damageDTO.Name;
            damage.PenalityPercentage = (decimal)damageDTO.PenalityPercentage;

            _context.AssetDamages.Add(damage);
            await _context.SaveChangesAsync();

            return damage;
        }

        public async Task<List<AssetDamage>> GetDamages()
        {
            var damages = await _context.AssetDamages.ToListAsync();

            return damages;
        }

        public async Task<bool> UpdateDamages(List<AssetDamage> damagesDTO)
        {
            foreach (var damageDTO in damagesDTO)
            {
                var damage = await _context.AssetDamages
                    .Where(d => d.AssetDamageId == damageDTO.AssetDamageId)
                    .FirstOrDefaultAsync();

                if (damage == null)
                    throw new KeyNotFoundException($"Asset Damage with Id {damageDTO.AssetDamageId} Not Found");

                damage.PenalityPercentage = damageDTO.PenalityPercentage;
                damage.Name = damageDTO.Name;

            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
