using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISurveyClinicMap
    {

        Task<APIResponse<SurveyClinicMap>> AddSurveyClinicMapAsync(SurveyClinicMapDto symptomsmaster);
        Task<APIResponse<SurveyClinicMap>> GetSurveyClinicMapByIdAsync(int id);
        Task<APIResponse<SurveyClinicMap>> UpdateSurveyClinicMapAsync(int id, SurveyClinicMapDto updatedSymptomsmaster);
        Task<APIResponse<SurveyClinicMap>> DeleteSurveyClinicMapAsync(int id);

        Task<APIResponse<List<SurveyClinicMap>>> GetAllSurveyClinicMapsAsync();
    }
}
