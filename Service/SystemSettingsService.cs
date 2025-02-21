using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace TrudoseAdminPortalAPI.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SystemSettingsService> _logger;

        public SystemSettingsService(ApplicationDbContext dbContext, ILogger<SystemSettingsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

       
        public async Task<APIResponse<Dictionary<string, string>>> GetAllSettingsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all system settings.");
                var settings = await _dbContext.system_settings
                    .ToDictionaryAsync(s => s.config_name!, s => s.config_value ?? "");

                return new APIResponse<Dictionary<string, string>>
                {
                    isError = false,
                    statusCode = 200,
                    data = settings,
                    errorMessage = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching system settings.");
                return new APIResponse<Dictionary<string, string>>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error",
                    data = null
                };
            }
        }

        
        public async Task<APIResponse<string?>> GetSettingAsync(string key)
        {
            try
            {
                _logger.LogInformation("Fetching system setting for key: {Key}", key);
                var setting = await _dbContext.system_settings
                    .FirstOrDefaultAsync(s => s.config_name == key);

                if (setting == null)
                {
                    return new APIResponse<string?>
                    {
                        isError = true,
                        statusCode = 404,
                        errorMessage = "Setting not found",
                        data = null
                    };
                }

                return new APIResponse<string?>
                {
                    isError = false,
                    statusCode = 200,
                    data = setting.config_value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching setting for key: {Key}", key);
                return new APIResponse<string?>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error",
                    data = null
                };
            }
        }

   
        public async Task<APIResponse<bool>> CreateSettingAsync(string key, string value)
        {
            try
            {
                _logger.LogInformation("Updating system setting: {Key} = {Value}", key, value);

                var setting = await _dbContext.system_settings
                    .FirstOrDefaultAsync(s => s.config_name == key);

                if (setting == null)
                {
                    _logger.LogInformation("Setting not found, creating a new one.");
                    await _dbContext.system_settings.AddAsync(new SystemSetting { config_name = key, config_value = value });
                }
                else
                {
                    setting.config_value = value;
                }

                await _dbContext.SaveChangesAsync();

                return new APIResponse<bool>
                {
                    isError = false,
                    statusCode = 200,
                    data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating setting: {Key}", key);
                return new APIResponse<bool>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error",
                    data = false
                };
            }
        }



        public async Task<APIResponse<bool>> UpdateMultipleSettingsAsync(Dictionary<string, string> settings)
        {
            try
            {
                _logger.LogInformation("Updating multiple system settings.");

                foreach (var kvp in settings)
                {
                    var existingSetting = await _dbContext.system_settings
                        .FirstOrDefaultAsync(s => s.config_name == kvp.Key);

                    if (existingSetting != null)
                    {
                        // Update only if the value is different
                        if (existingSetting.config_value != kvp.Value)
                        {
                            existingSetting.config_value = kvp.Value;
                            await _dbContext.SaveChangesAsync(); // Save changes for each update
                        }
                    }
                }

                return new APIResponse<bool>
                {
                    isError = false,
                    statusCode = 200,
                    data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating multiple settings.");
                return new APIResponse<bool>
                {
                    isError = true,
                    statusCode = 500,
                    errorMessage = "Internal Server Error",
                    data = false
                };
            }
        }



    }
}
