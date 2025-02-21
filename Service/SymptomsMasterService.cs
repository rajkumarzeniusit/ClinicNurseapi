using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TrudoseAdminPortalAPI.Service
{
    public class SymptomsMasterService : ISymptomsMasterService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SymptomsMasterService> _logger;

        public SymptomsMasterService(ApplicationDbContext dbContext, ILogger<SymptomsMasterService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<APIResponse<SymptomsMaster>> AddSymptomsMasterAsync(SymptomsMasterDto symptoms)
        {
            try
            {
                _logger.LogInformation("Adding Symptoms Master information.");

                // Basic validation
                if (symptoms == null)
                {
                    _logger.LogError("Symptoms information cannot be null.");
                    return new APIResponse<SymptomsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Symptoms Master information cannot be null.",
                        data = null
                    };
                }

                // Map DTO to entity
                var patientSymptoms = new SymptomsMaster
                {
                    symptom_name = symptoms.symptom_name,
                    symptom_description = symptoms.symptom_description

                };

                // Add the symptoms to the database
                await _dbContext.symptoms_master.AddAsync(patientSymptoms);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Symptoms Master information with symptom_id {patientSymptoms.id} added successfully");

                // Return a success response
                return new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patientSymptoms
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Symptoms Master");
                return new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }

        public async Task<APIResponse<List<SymptomsMaster>>> GetAllSymptomsMasterAsync()
        {
            try
            {
                var symptoms = await _dbContext.symptoms_master.ToListAsync();

                if (symptoms == null || !symptoms.Any())
                {
                    return new APIResponse<List<SymptomsMaster>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No symptoms found",
                        data = null
                    };
                }

                return new APIResponse<List<SymptomsMaster>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = symptoms
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<SymptomsMaster>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An error occurred while fetching symptoms data.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<SymptomsMaster>> GetSymptomsMasterByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving Symptoms Master information for Id {id}.");

                // Retrieve the patient by ID
                var patient = await _dbContext.symptoms_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SymptomsMaster information with SymptomId {id} not found.");
                    return new APIResponse<SymptomsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SymptomsMaster information with SymptomId {id} not found.",
                        data = null
                    };
                }

                _logger.LogInformation($"SymptomsMaster information for SymptomId {id} retrieved successfully.");

                // Return a success response
                return new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Symptoms");
                return new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        public async Task<APIResponse<SymptomsMaster>> UpdateSymptomsMasterAsync(int id, SymptomsMasterDto updatedSymptoms)
        {
            try
            {
                _logger.LogInformation($"Updating SymptomsMaster information for SymptomId {id}.");

                // Retrieve the existing patient record from the database
                var existingPatient = await _dbContext.symptoms_master.FindAsync(id);

                if (existingPatient == null)
                {
                    _logger.LogError($"SymptomsMaster information with SymptomId {id} not found.");
                    return new APIResponse<SymptomsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SymptomsMaster information with SymptomId {id} not found.",
                        data = null
                    };
                }

                // Update the fields with new data
                existingPatient.symptom_name = updatedSymptoms.symptom_name;
                existingPatient.symptom_description = updatedSymptoms.symptom_description;


                // Save changes to the database
                _dbContext.symptoms_master.Update(existingPatient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"SymptomsMaster information with SymptomId {id} updated successfully.");

                // Return a success response
                return new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = existingPatient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SymptomsMaster update.");
                return new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }


        public async Task<APIResponse<SymptomsMaster>> DeleteSymptomsMasterAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting SymptomsMaster information for SymptomId {id}.");

                // Retrieve the existing patient record from the database
                var patient = await _dbContext.symptoms_master.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SymptomsMaster information with SymptomId {id} not found.");
                    return new APIResponse<SymptomsMaster>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SymptomsMaster information with SymptomId {id} not found.",
                        data = null
                    };
                }

                // Remove the patient record
                _dbContext.symptoms_master.Remove(patient);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"SymptomsMaster information with SymptomId {id} deleted successfully.");

                // Return a success response
                return new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SymptomsMaster deletion.");
                return new APIResponse<SymptomsMaster>
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
