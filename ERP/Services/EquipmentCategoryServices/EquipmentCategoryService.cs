using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.EquipmentCategoryServices
{
    public class EquipmentCategoryService : IEquipmentCategoryService
    {
        private readonly DataContext _context;

        public EquipmentCategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<EquipmentCategory>> GetAll()
        {
            var categories = await _context.EquipmentCategories.ToListAsync();

            return categories;
        }

        public async Task<EquipmentCategory> GetById(int id)
        {
            var category = await _context.EquipmentCategories
                .Where(ec => ec.EquipmentCategoryId == id)
                .Include(ec => ec.Equipments)
                .ThenInclude(equipment => equipment.Item)
                .FirstOrDefaultAsync();

            if (category == null) throw new KeyNotFoundException("Equipment Category Not Found");

            return category;
        }
    }
}
