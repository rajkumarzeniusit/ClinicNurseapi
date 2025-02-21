using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;

namespace TrudoseAdminPortalAPI.Service
{
    public class SurveyClinicMapService :ISurveyClinicMap
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SurveyClinicMapService> _logger;
        public SurveyClinicMapService(ApplicationDbContext dbContext, ILogger<SurveyClinicMapService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<APIResponse<SurveyClinicMap>> AddSurveyClinicMapAsync(SurveyClinicMapDto symptoms)
        {
            try
            {
                _logger.LogInformation("Adding SurveyClinicMap information.");

                // Basic validation
                if (symptoms == null)
                {
                    _logger.LogError("SurveyClinicMap information cannot be null.");
                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Survey information cannot be null.",
                        data = null
                    };
                }

                // Map DTO to entity
                var patientSymptoms = new SurveyClinicMap
                {
                    survey_id = symptoms.survey_id,
                    clinic_id = symptoms.clinic_id,   

                };

                // Add the symptoms to the database
                await _dbContext.survey_clinic_map.AddAsync(patientSymptoms);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"SurveyClinicMap information with Id {patientSymptoms.id} added successfully");

                // Return a success response
                return new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patientSymptoms
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database error: {ErrorMessage}", innerException.Message);

                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    };
                }
                _logger.LogError($"Database error: {dbEx.Message}");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "A database error occurred. Please check the input data.",
                    data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyClinicMap");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<List<SurveyClinicMap>>> GetAllSurveyClinicMapsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all SurveyClinicMap records.");

                var surveyClinicMaps = await _dbContext.survey_clinic_map.ToListAsync();

                if (surveyClinicMaps == null || !surveyClinicMaps.Any())
                {
                    _logger.LogWarning("No SurveyClinicMap records found.");
                    return new APIResponse<List<SurveyClinicMap>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No SurveyClinicMap records found.",
                        data = null
                    };
                }

                _logger.LogInformation($"Retrieved {surveyClinicMaps.Count} SurveyClinicMap records successfully.");

                return new APIResponse<List<SurveyClinicMap>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = surveyClinicMaps
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching SurveyClinicMap records.");
                return new APIResponse<List<SurveyClinicMap>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An error occurred while processing your request.",
                    data = null
                };
            }
        }




        public async Task<APIResponse<SurveyClinicMap>> GetSurveyClinicMapByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving SurveyClinicMap information for SurveyClinicMap {id}.");

                // Retrieve the patient by ID
                var patient = await _dbContext.survey_clinic_map.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SurveyClinicMap information with SurveyClinicMap {id} not found.");
                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyClinicMap information with SurveyClinicMap {id} not found.",
                        data = null
                    };
                }

                _logger.LogInformation($"SurveyClinicMap information for SurveyClinicMap {id} retrieved successfully.");

                // Return a success response
                return new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyClinicMap");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }




     



        public async Task<APIResponse<SurveyClinicMap>> DeleteSurveyClinicMapAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting SurveyClinicMap information for SurveyClinicMap {id}.");

                // Retrieve the existing patient record from the database
                var patient = await _dbContext.survey_clinic_map.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SurveyClinicMap information with SurveyClinicMap {id} not found.");
                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyClinicMap information with SurveyClinicMap {id} not found.",
                        data = null
                    };
                }

                // Remove the patient record
                _dbContext.survey_clinic_map.Remove(patient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"SurveyClinicMap information with SurveyClinicMap {id} deleted successfully.");

                // Return a success response
                return new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyClinicMap deletion.");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        public async Task<APIResponse<SurveyClinicMap>> UpdateSurveyClinicMapAsync(int id, SurveyClinicMapDto updatedSurveyClinicMapDto)
        {
            try
            {
                _logger.LogInformation($"Updating SurveyClinicMap with ID {id}.");

                var surveyClinicMap = await _dbContext.survey_clinic_map.FindAsync(id);

                if (surveyClinicMap == null)
                {
                    _logger.LogWarning($"SurveyClinicMap with ID {id} not found.");
                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "SurveyClinicMap not found.",
                        data = null
                    };
                }

                // Update the entity fields
                surveyClinicMap.survey_id = updatedSurveyClinicMapDto.survey_id;
                surveyClinicMap.clinic_id = updatedSurveyClinicMapDto.clinic_id;

                _dbContext.survey_clinic_map.Update(surveyClinicMap);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"SurveyClinicMap with ID {id} updated successfully.");

                return new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = surveyClinicMap
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database error: {ErrorMessage}", innerException.Message);

                    return new APIResponse<SurveyClinicMap>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    };
                }
                _logger.LogError($"Database error: {dbEx.Message}");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "A database error occurred. Please check the input data.",
                    data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating SurveyClinicMap with ID {id}.");
                return new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An error occurred while processing your request.",
                    data = null
                };
            }
        }

    }
}
