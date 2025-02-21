

using TrudoseAdminPortalAPI.Dto;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISurveyQuestionMapService
    {
        Task<APIResponse<SurveyQuestionMapResponseDto>> AddSymptomsAsync(PatientSurveyQuestionMapDto symptomsmaster);
        Task<APIResponse<SurveyQuestionMapResponseDto>> GetSymptomsByIdAsync(int id);
        Task<APIResponse<SurveyQuestionMapResponseDto>> UpdateSymptomsAsync(int id, PatientSurveyQuestionMapUpdateDto updatedSymptomsmaster);
        Task<APIResponse<SurveyQuestionMapResponseDto>> DeleteSymptomsAsync(int id);

        //Task<APIResponse<SurveyQuestionMapDeleteResponseDto>> DeleteByQuestionAndSurveyAsync(int questionId, int surveyId);
        Task<APIResponse<List<SurveyQuestionMapDeleteResponseDto>>> DeleteAllByQuestionAndSurveyAsync(int questionId, int surveyId);
        Task<APIResponse<List<SurveyQuestionMapResponseDto>>> GetAllquestionsbysurveyidAsync();
        Task<APIResponse<List<SurveyQuestionMapResponseDto>>> GetQuestionsBySurveyIdAsync(int surveyId);
    }
}
