
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace TrudoseAdminPortalAPI.Service
{
    public class QuestionsMasterService : IQuestionsMaster
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<QuestionsMasterService> _logger;

        public QuestionsMasterService(ApplicationDbContext dbContext, ILogger<QuestionsMasterService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<APIResponse<QuestionsMaster>> AddQuestionnaireAsync(QuestionsMasterDto symptoms)
        {
            try
            {
                _logger.LogInformation("Adding Questionnaire information.");

                // Basic validation
                if (symptoms == null)
                {
                    _logger.LogError("Questionnaire information cannot be null.");
                    return new APIResponse<QuestionsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Survey information cannot be null.",
                        data = null
                    };
                }

                // Map DTO to entity
                var patientSymptoms = new QuestionsMaster
                {
                    question_name = symptoms.question_name,
                    question_type = symptoms.question_type,
                    question_choices=symptoms.question_choices,

                };

                // Add the symptoms to the database
                await _dbContext.questions_master.AddAsync(patientSymptoms);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Questionnaire information with QuestionId {patientSymptoms.id} added successfully");

                // Return a success response
                return new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patientSymptoms
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire");
                return new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<QuestionsMaster>> GetQuestionnaireByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving Questionnaire information for QuestionId {id}.");

                // Retrieve the patient by ID
                var patient = await _dbContext.questions_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"Questionnaire information with QuestionId {id} not found.");
                    return new APIResponse<QuestionsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Questionnaire information with QuestionId {id} not found.",
                        data = null
                    };
                }

                _logger.LogInformation($"Questionnaire information for QuestionId {id} retrieved successfully.");

                // Return a success response
                return new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire");
                return new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }

        public async Task<APIResponse<List<QuestionsMaster>>> GetAllQuestionnairesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all questionnaire information.");

                var questionnaires = await _dbContext.questions_master.ToListAsync();

                if (!questionnaires.Any())
                {
                    _logger.LogWarning("No questionnaires found.");
                    return new APIResponse<List<QuestionsMaster>>
                    {
                        isError = false,
                        statusCode = StatusCodes.Status200OK,
                        errorMessage = "No questionnaires found.",
                        data = new List<QuestionsMaster>()
                    };
                }

                _logger.LogInformation("All questionnaires retrieved successfully.");

                return new APIResponse<List<QuestionsMaster>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = questionnaires
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving all questionnaires.");
                return new APIResponse<List<QuestionsMaster>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<QuestionsMaster>> UpdateQuestionnaireAsync(int id, QuestionsMasterDto updatedSymptoms)
        {
            try
            {
                _logger.LogInformation($"Updating Questionnaire information for QuestionId {id}.");

                // Retrieve the existing patient record from the database
                var existingPatient = await _dbContext.questions_master.FindAsync(id);

                if (existingPatient == null)
                {
                    _logger.LogError($"Questionnaire information with QuestionId {id} not found.");
                    return new APIResponse<QuestionsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Questionnaire information with QuestionId {id} not found.",
                        data = null
                    };
                }

                // Update the fields with new data
                existingPatient.question_name = updatedSymptoms.question_name;
                existingPatient.question_type = updatedSymptoms.question_type;
                existingPatient.question_choices = updatedSymptoms.question_choices;


                // Save changes to the database
                _dbContext.questions_master.Update(existingPatient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Questionnaire information with QuestionId {id} updated successfully.");

                // Return a success response
                return new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = existingPatient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire update.");
                return new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }

        public async Task<APIResponse<QuestionsMaster>> DeleteQuestionnaireAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting Questionnaire information for QuestionId {id}.");

                // Retrieve the existing patient record from the database
                var patient = await _dbContext.questions_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"Questionnaire information with QuestionId {id} not found.");
                    return new APIResponse<QuestionsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Questionnaire information with QuestionId {id} not found.",
                        data = null
                    };
                }

                // Remove the patient record
                _dbContext.questions_master.Remove(patient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Questionnaire information with QuestionId {id} deleted successfully.");

                // Return a success response
                return new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire deletion.");
                return new APIResponse<QuestionsMaster>
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
