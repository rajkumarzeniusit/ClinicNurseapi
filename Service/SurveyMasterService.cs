
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Data;

using TrudoseAdminPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Dto;


namespace TrudoseAdminPortalAPI.Service
{
    public class SurveyMasterService : ISurveyMaster
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SurveyMasterService> _logger;

        public SurveyMasterService(ApplicationDbContext dbContext, ILogger<SurveyMasterService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<APIResponse<SurveyMaster>> AddSurveyAsync(SurveyMasterDto symptoms)
        {
            try
            {
                _logger.LogInformation("Adding Survey information.");

                // Basic validation
                if (symptoms == null)
                {
                    _logger.LogError("Survey information cannot be null.");
                    return new APIResponse<SurveyMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Survey information cannot be null.",
                        data = null
                    };
                }

                // Map DTO to entity
                var patientSymptoms = new SurveyMaster
                {
                    survey_name = symptoms.survey_name,
                    survey_description = symptoms.survey_description,
                    is_mandatory=symptoms.is_mandatory,

                };

                // Add the symptoms to the database
                await _dbContext.surveys_master.AddAsync(patientSymptoms);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Survey information with Survey_id {patientSymptoms.id} added successfully");

                // Return a success response
                return new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patientSymptoms
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey");
                return new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        public async Task<APIResponse<List<SurveyMaster>>> GetAllSurveysAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all survey information.");

                var surveys = await _dbContext.surveys_master.ToListAsync();

                if (!surveys.Any())
                {
                    _logger.LogWarning("No surveys found.");
                    return new APIResponse<List<SurveyMaster>>
                    {
                        isError = false,
                        statusCode = StatusCodes.Status200OK,
                        errorMessage = "No surveys found.",
                        data = new List<SurveyMaster>()
                    };
                }

                _logger.LogInformation("All surveys retrieved successfully.");

                return new APIResponse<List<SurveyMaster>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = surveys
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving all surveys.");
                return new APIResponse<List<SurveyMaster>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }

        public async Task<APIResponse<SurveyMaster>> GetSurveyByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving Survey information for SurveyId {id}.");

                // Retrieve the patient by ID
                var patient = await _dbContext.surveys_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"Survey information with SurveyId {id} not found.");
                    return new APIResponse<SurveyMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyMaster information with SurveyId {id} not found.",
                        data = null
                    };
                }

                _logger.LogInformation($"Survey information for SurveyId {id} retrieved successfully.");

                // Return a success response
                return new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey");
                return new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<SurveyMaster>> UpdateSurveyAsync(int id, SurveyMasterDto updatedSymptoms)
        {
            try
            {
                _logger.LogInformation($"Updating Survey information for SurveyId {id}.");

                // Retrieve the existing patient record from the database
                var existingPatient = await _dbContext.surveys_master.FindAsync(id);

                if (existingPatient == null)
                {
                    _logger.LogError($"Survey information with SurveyId {id} not found.");
                    return new APIResponse<SurveyMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Survey information with SurveyId {id} not found.",
                        data = null
                    };
                }

                // Update the fields with new data
                existingPatient.survey_name = updatedSymptoms.survey_name;
                existingPatient.survey_description = updatedSymptoms.survey_description;
                existingPatient.is_mandatory= updatedSymptoms.is_mandatory;


                // Save changes to the database
                _dbContext.surveys_master.Update(existingPatient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Survey information with SurveyId {id} updated successfully.");

                // Return a success response
                return new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = existingPatient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey update.");
                return new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<SurveyMaster>> DeleteSurveyAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting Survey information for SurveyId {id}.");

                // Retrieve the existing patient record from the database
                var patient = await _dbContext.surveys_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"Survey information with SurveyId {id} not found.");
                    return new APIResponse<SurveyMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Survey information with SurveyId {id} not found.",
                        data = null
                    };
                }

                // Remove the patient record
                _dbContext.surveys_master.Remove(patient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Survey information with SurveyId {id} deleted successfully.");

                // Return a success response
                return new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey deletion.");
                return new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


    }
}
