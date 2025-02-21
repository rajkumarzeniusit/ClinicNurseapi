using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Interface;
using Microsoft.AspNetCore.Mvc;

namespace TrudoseAuthAPIService.Controllers
{
    [ApiController]
    [Route("api/system-settings")]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly ILogger<SystemSettingsController> _logger;

        public SystemSettingsController(ISystemSettingsService systemSettingsService, ILogger<SystemSettingsController> logger)
        {
            _systemSettingsService = systemSettingsService;
            _logger = logger;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            try
            {
                _logger.LogInformation("Received request to fetch all system settings.");
                var settings = await _systemSettingsService.GetAllSettingsAsync();

                return Ok(new APIResponse<Dictionary<string, string>>
                {
                    isError = false,
                    statusCode = 200,
                    data = settings.data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch system settings.");
                return StatusCode(500, new APIResponse<Dictionary<string, string>>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error"
                });
            }
        }

        
        [HttpGet("{key}")]
        public async Task<IActionResult> GetSetting(string key)
        {
            try
            {
                _logger.LogInformation("Received request to fetch setting: {Key}", key);
                var value = await _systemSettingsService.GetSettingAsync(key);

                if (value == null)
                {
                    _logger.LogWarning("Setting not found: {Key}", key);
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = 404,
                        errorMessage = $"Setting '{key}' not found."
                    });
                }

                return Ok(new APIResponse<string>
                {
                    isError = false,
                    statusCode = 200,
                    data = value.data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch setting: {Key}", key);
                return StatusCode(500, new APIResponse<string>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error"
                });
            }
        }

        
        [HttpPost("{key}")]
        public async Task<IActionResult> UpdateSetting(string key, [FromBody] string value)
        {
            try
            {
                _logger.LogInformation("Received request to update setting: {Key}", key);
                var result = await _systemSettingsService.CreateSettingAsync(key, value);

                if (!result.isError)
                {
                    return Ok(new APIResponse<bool>
                    {
                        isError = false,
                        statusCode = 200,
                        data = true
                    });
                }

                return BadRequest(new APIResponse<bool>
                {
                    isError = true,
                    statusCode = 400,
                    errorMessage = "Failed to update setting."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update setting: {Key}", key);
                return StatusCode(500, new APIResponse<bool>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error"
                });
            }
        }



        [HttpPut("batch-update")]
        public async Task<IActionResult> UpdateMultipleSettings([FromBody] Dictionary<string, string> settings)
        {
            try
            {
                if (settings == null || settings.Count == 0)
                {
                    _logger.LogWarning("Invalid request: Settings cannot be empty.");
                    return BadRequest(new APIResponse<bool>
                    {
                        isError = true,
                        statusCode = 400,
                        errorMessage = "Settings cannot be empty."
                    });
                }

                _logger.LogInformation("Received request to update multiple system settings.");
                var result = await _systemSettingsService.UpdateMultipleSettingsAsync(settings);

                if (!result.isError)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update multiple settings.");
                return StatusCode(500, new APIResponse<bool>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error"
                });
            }
        }

    }
}
