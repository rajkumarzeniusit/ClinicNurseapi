
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISurveyMaster
    {
        Task<APIResponse<SurveyMaster>> AddSurveyAsync(SurveyMasterDto symptomsmaster);
        Task<APIResponse<SurveyMaster>> GetSurveyByIdAsync(int id);
        Task<APIResponse<SurveyMaster>> UpdateSurveyAsync(int id, SurveyMasterDto updatedSymptomsmaster);
        Task<APIResponse<SurveyMaster>> DeleteSurveyAsync(int id);

        Task<APIResponse<List<SurveyMaster>>> GetAllSurveysAsync();

    }
}
