

using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;

namespace TrudoseAdminPortalAPI.Interface
{
    public interface IQuestionsMaster
    {
        Task<APIResponse<QuestionsMaster>> AddQuestionnaireAsync(QuestionsMasterDto symptomsmaster);
        Task<APIResponse<QuestionsMaster>> GetQuestionnaireByIdAsync(int id);
        Task<APIResponse<QuestionsMaster>> UpdateQuestionnaireAsync(int id, QuestionsMasterDto updatedSymptomsmaster);
        Task<APIResponse<QuestionsMaster>> DeleteQuestionnaireAsync(int id);

        Task<APIResponse<List<QuestionsMaster>>> GetAllQuestionnairesAsync();
    }
}
