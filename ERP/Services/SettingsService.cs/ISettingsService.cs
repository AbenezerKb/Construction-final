using ERP.DTOs.Others;
using ERP.Models;

namespace ERP.Services.SettingService
{
    public interface ISettingsService
    {

        Task<List<Setting>> GetAllSettings();
        Task<Setting> GetByName(String settingName);
        Task<Setting> UpdateSetting(SettingDto settingDto);


    }

}