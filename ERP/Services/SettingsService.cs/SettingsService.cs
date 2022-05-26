using ERP.Context;
using ERP.DTOs.Others;
using ERP.Exceptions;
using ERP.Models;
using ERP.Services.SettingService;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.SettingService
{
    public class SettingsService : ISettingsService
    {
        private readonly DataContext dbContext;

        public SettingsService(DataContext appDbContext)
        {
            dbContext = appDbContext;

        }

        public async Task<List<Setting>> GetAllSettings()
        {
            return await dbContext.Settings.ToListAsync();
        }

        public async Task<Setting> GetByName(string settingName)
        {
            var setting = await dbContext.Settings.FirstOrDefaultAsync(s => s.Name == settingName);
            if (setting == null) throw new ItemNotFoundException($"Setting not found with name={settingName}");
            return setting;
        }

        public async Task<Setting> UpdateSetting(SettingDto settingDto)
        {
            var setting = await dbContext.Settings.FirstOrDefaultAsync(s => s.Name == settingDto.Name);


            if (setting == null)
            {
                setting = new Setting()
                {
                    Name = settingDto.Name,
                    Description = settingDto.Description,
                    Value = settingDto.Value
                };
                await dbContext.Settings.AddAsync(setting);
            }
            else
            {
                setting.Name = settingDto.Name;
                setting.Description = settingDto.Description;
                setting.Value = settingDto.Value;
                dbContext.Settings.Update(setting);
            }

            await dbContext.SaveChangesAsync();

            return setting;
        }
    }
}