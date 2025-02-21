
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;



namespace TrudoseAdminPortalAPI.Interface
{
    public interface ISymptomsMasterService
    {
        Task<APIResponse<SymptomsMaster>> AddSymptomsMasterAsync(SymptomsMasterDto symptomsmaster);
        Task<APIResponse<SymptomsMaster>> GetSymptomsMasterByIdAsync(int id);
        Task<APIResponse<SymptomsMaster>> UpdateSymptomsMasterAsync(int id, SymptomsMasterDto updatedSymptomsmaster);
        Task<APIResponse<SymptomsMaster>> DeleteSymptomsMasterAsync(int id);

        Task<APIResponse<List<SymptomsMaster>>> GetAllSymptomsMasterAsync();
    }
}
