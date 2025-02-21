


using TrudoseAdminPortalAPI.Dto;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISystemSettingsService
    {
        Task<APIResponse<Dictionary<string, string>>> GetAllSettingsAsync();
        Task<APIResponse<string?>> GetSettingAsync(string key);
        Task<APIResponse<bool>> CreateSettingAsync(string key, string value);

        Task<APIResponse<bool>> UpdateMultipleSettingsAsync(Dictionary<string, string> settings);

    }
}
