
using ERP.DTOs.Others;
using ERP.Exceptions;
using ERP.Services.SettingService;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {

            this.settingsService = settingsService;
        }
        [HttpGet]
        public async Task<ActionResult<CustomApiResponse>> GetSettings([FromQuery] string? name)
        {

            try
            {
                if (name == null)
                {
                    return Ok(new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await settingsService.GetAllSettings()
                    });
                }

                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await settingsService.GetByName(name)
                });

            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(
                    new CustomApiResponse
                    {
                        Message = infe.Message
                    }
                );
            }
        }
        [HttpPut()]
        public async Task<ActionResult<CustomApiResponse>> UpdateSetting(SettingDto settingDto)
        {
            int parsedValue;

            if (settingDto.Name == "DeadlineNotificationDay" && !int.TryParse(settingDto.Name, out parsedValue))
            {
                return BadRequest(
                    new CustomApiResponse
                    {
                        Message = "Invalid value, DeadlineNotificationDay should be a number"
                    }
                );
            }

            return Ok(new CustomApiResponse
            {
                Message = "Success",
                Data = await settingsService.UpdateSetting(settingDto)
            });
        }

    }
}