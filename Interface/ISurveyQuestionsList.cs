

using TrudoseAdminPortalAPI.Dto;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISurveyQuestionsList
    {
        //Task<List<SurveyQuestionSymptomDto>> GetSurveyDataAsync(List<int> symptomIds);
        Task<List<SurveyResponseDto>> GetSurveyDataAsync(List<int> symptomIds);

       
    }
}
