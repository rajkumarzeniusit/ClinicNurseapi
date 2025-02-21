
using Admin.Models;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace TrudoseAdminPortalAPI.Service
{
    public class SurveyQuestionmapService : ISurveyQuestionMapService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SurveyQuestionmapService> _logger;

        public SurveyQuestionmapService(ApplicationDbContext dbContext, ILogger<SurveyQuestionmapService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<APIResponse<SurveyQuestionMapResponseDto>> AddSymptomsAsync(PatientSurveyQuestionMapDto symptoms)
        {
            try
            {
                _logger.LogInformation("Adding SurveyQuestionmap information.");

                // Basic validation
                if (symptoms == null)
                {
                    _logger.LogError("SurveyQuestionmap information cannot be null.");
                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "SurveyQuestionmap information cannot be null.",
                        data = null
                    };
                }

                // Map DTO to entity
                var patientSymptoms = new PatientSurveyQuestionMap
                {
                    
                    symptom_id = symptoms.symptom_id,
                    question_id = symptoms.question_id,
                    survey_id = symptoms.survey_id,
                    order_no = symptoms.order_no,
               



                };

                // Add the symptoms to the database
                await _dbContext.survey_question_symptom_map.AddAsync(patientSymptoms);
                await _dbContext.SaveChangesAsync();
                var responseobj = new SurveyQuestionMapResponseDto
                {
                    id= patientSymptoms.id,
                    symptom_id = patientSymptoms.symptom_id,
                    question_id = patientSymptoms.question_id,
                    survey_id = patientSymptoms.survey_id,
                    order_no = patientSymptoms.order_no,

                };
                _logger.LogInformation($"SurveyQuestionmap information with Id {patientSymptoms.id} added successfully");

                // Return a success response
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = responseobj
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database error: {ErrorMessage}", innerException.Message);

                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    };
                }
                _logger.LogError($"Database error: {dbEx.Message}");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "A database error occurred. Please check the input data.",
                    data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyQuestionmap");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }

   



        public async Task<APIResponse<SurveyQuestionMapResponseDto>> GetSymptomsByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving SurveyQuestionmap information for Id {id}.");

                // Retrieve the patient by ID
                var patient = await _dbContext.survey_question_symptom_map.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SurveyQuestionmap information with Id {id} not found.");
                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyQuestionmap information with Id {id} not found.",
                        data = null
                    };
                }
                var responseobj = new SurveyQuestionMapResponseDto
                {
                    id=patient.id,
                    symptom_id = patient.symptom_id,
                    question_id = patient.question_id,
                    survey_id = patient.survey_id,
                    order_no = patient.order_no,

                };

                _logger.LogInformation($"SurveyQuestionmap information for Id {id} retrieved successfully.");

                // Return a success response
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseobj
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyQuestionmap");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        public async Task<APIResponse<SurveyQuestionMapResponseDto>> UpdateSymptomsAsync(int id, PatientSurveyQuestionMapUpdateDto updatedSymptoms)
        {
            try
            {
                _logger.LogInformation($"Updating SurveyQuestionmap information for Id {id}.");

                // Retrieve the existing patient record from the database
                var existingPatient = await _dbContext.survey_question_symptom_map.FindAsync(id);

                if (existingPatient == null)
                {
                    _logger.LogError($"SurveyQuestionmap information with Id {id} not found.");
                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyQuestionmap information with Id {id} not found.",
                        data = null
                    };
                }

                // Update the fields with new data
                existingPatient.symptom_id = updatedSymptoms.symptom_id;
                existingPatient.question_id = updatedSymptoms.question_id;
                existingPatient.survey_id = updatedSymptoms.survey_id;
                existingPatient.order_no = updatedSymptoms.order_no;
              


                // Save changes to the database
                _dbContext.survey_question_symptom_map.Update(existingPatient);
                await _dbContext.SaveChangesAsync();
                var responseobj = new SurveyQuestionMapResponseDto
                {
                    id = existingPatient.id,
                    symptom_id = existingPatient.symptom_id,
                    question_id = existingPatient.question_id,
                    survey_id = existingPatient.survey_id,
                    order_no = existingPatient.order_no,

                };
                _logger.LogInformation($"SurveyQuestionmap information with Id {id} updated successfully.");

                // Return a success response
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseobj
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database error: {ErrorMessage}", innerException.Message);

                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    };
                }
                _logger.LogError($"Database error: {dbEx.Message}");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "A database error occurred. Please check the input data.",
                    data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyQuestionmap update.");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }







        public async Task<APIResponse<SurveyQuestionMapResponseDto>> DeleteSymptomsAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting SurveyQuestionmap information for Id {id}.");

                // Retrieve the existing patient record from the database
                var patient = await _dbContext.survey_question_symptom_map.FindAsync(id);

                if (patient == null)
                {
                    _logger.LogError($"SurveyQuestionmap information with Id {id} not found.");
                    return new APIResponse<SurveyQuestionMapResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"SurveyQuestionmap information with Id {id} not found.",
                        data = null
                    };
                }

                // Remove the patient record
                _dbContext.survey_question_symptom_map.Remove(patient);
                await _dbContext.SaveChangesAsync();
                var responseobj = new SurveyQuestionMapResponseDto
                {
                    id = patient.id,
                    symptom_id = patient.symptom_id,
                    question_id = patient.question_id,
                    survey_id = patient.survey_id,
                    order_no = patient.order_no,

                };
                _logger.LogInformation($"SurveyQuestionmap information with Id {id} deleted successfully.");

                // Return a success response
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseobj
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyQuestionmap deletion.");
                return new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        public async Task<APIResponse<List<SurveyQuestionMapResponseDto>>> GetAllquestionsbysurveyidAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all SurveyQuestionmap information.");

                // Retrieve all records from the database, including related data
                var records = await _dbContext.survey_question_symptom_map
                    .Include(sqsm => sqsm.Question)  
                    .Include(sqsm => sqsm.Symptom)  
                    .Include(sqsm => sqsm.Survey)
                    .OrderByDescending(id => id)
                    .ToListAsync();

                if (records == null || !records.Any())
                {
                    _logger.LogError("No SurveyQuestionmap information found.");
                    return new APIResponse<List<SurveyQuestionMapResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No SurveyQuestionmap information found.",
                        data = null
                    };
                }

                // Convert to DTO with related data
                var responseList = records.Select(record => new SurveyQuestionMapResponseDto
                {
                    id = record.id,
                    question_id = record.question_id,
                    question_name = record.Question.question_name,  // Fetch question text
                    symptom_id = record.symptom_id,
                    symptom_name = record.Symptom.symptom_name,    // Fetch symptom name
                    survey_id = record.survey_id,
                    survey_name = record.Survey?.survey_name,             // Fetch survey name
                    order_no = record.order_no
                }).ToList();

                _logger.LogInformation("All SurveyQuestionmap information retrieved successfully.");

                // Return a success response
                return new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving SurveyQuestionmap data.");
                return new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }



        //public async Task<APIResponse<List<SurveyQuestionMapResponseDto>>> GetAllquestionsbysurveyidAsync()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Retrieving all Symptoms information.");

        //        // Retrieve all records from the database
        //        var patients = await _dbContext.survey_question_symptom_map.ToListAsync();

        //        if (patients == null || !patients.Any())
        //        {
        //            _logger.LogError("No Symptoms information found.");
        //            return new APIResponse<List<SurveyQuestionMapResponseDto>>
        //            {
        //                isError = true,
        //                statusCode = StatusCodes.Status404NotFound,
        //                errorMessage = "No Symptoms information found.",
        //                data = null
        //            };
        //        }

        //        // Convert to DTO
        //        var responseList = patients.Select(patient => new SurveyQuestionMapResponseDto
        //        {
        //            id = patient.id,
        //            symptom_id = patient.symptom_id,
        //            question_id = patient.question_id,
        //            survey_id = patient.survey_id,
        //            order_no = patient.order_no
        //        }).ToList();

        //        _logger.LogInformation("All Symptoms information retrieved successfully.");

        //        // Return a success response
        //        return new APIResponse<List<SurveyQuestionMapResponseDto>>
        //        {
        //            isError = false,
        //            statusCode = StatusCodes.Status200OK,
        //            errorMessage = null,
        //            data = responseList
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Exception occurred while retrieving Symptoms data.");
        //        return new APIResponse<List<SurveyQuestionMapResponseDto>>
        //        {
        //            isError = true,
        //            statusCode = StatusCodes.Status500InternalServerError,
        //            errorMessage = "An unexpected error occurred. Please try again later.",
        //            data = null
        //        };
        //    }
        //}

        public async Task<APIResponse<List<SurveyQuestionMapResponseDto>>> GetQuestionsBySurveyIdAsync(int surveyId)
        {
            try
            {
                _logger.LogInformation("Retrieving questions for Survey ID: {SurveyId}", surveyId);

                // Fetch records where survey_id matches
                var questions = await _dbContext.survey_question_symptom_map
                                    .Where(q => q.survey_id == surveyId)
                                    .ToListAsync();

                if (questions == null || !questions.Any())
                {
                    _logger.LogError("No questions found for Survey ID {SurveyId}", surveyId);
                    return new APIResponse<List<SurveyQuestionMapResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No questions found for the given survey ID.",
                        data = null
                    };
                }

                // Map to DTO
                var responseList = questions.Select(q => new SurveyQuestionMapResponseDto
                {
                    id = q.id,
                    symptom_id = q.symptom_id,
                    question_id = q.question_id,
                    survey_id = q.survey_id,
                    order_no = q.order_no
                }).ToList();

                _logger.LogInformation("Questions for Survey ID {SurveyId} retrieved successfully.", surveyId);

                return new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving questions for Survey ID {SurveyId}", surveyId);
                return new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                };
            }
        }





        //public async Task<APIResponse<SurveyQuestionMapDeleteResponseDto>> DeleteByQuestionAndSurveyAsync(int questionId, int surveyId)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Deleting survey question symptom mapping for Question ID {questionId} and Survey ID {surveyId}.");

        //        // Retrieve the existing record from the database
        //        var mapping = await _dbContext.survey_question_symptom_map
        //            .FirstOrDefaultAsync(x => x.question_id == questionId && x.survey_id == surveyId);

        //        if (mapping == null)
        //        {
        //            _logger.LogError($"Survey question symptom mapping with Question ID {questionId} and Survey ID {surveyId} not found.");
        //            return new APIResponse<SurveyQuestionMapDeleteResponseDto>
        //            {
        //                isError = true,
        //                statusCode = StatusCodes.Status404NotFound,
        //                errorMessage = $"Survey question symptom mapping with Question ID {questionId} and Survey ID {surveyId} not found.",
        //                data = null
        //            };
        //        }

        //        // Remove the record
        //        _dbContext.survey_question_symptom_map.Remove(mapping);
        //        await _dbContext.SaveChangesAsync();

        //        var responseObj = new SurveyQuestionMapDeleteResponseDto
        //        {
        //            id = mapping.id,
        //            symptom_id = mapping.symptom_id,                    
        //            question_id = mapping.question_id,
        //            survey_id = mapping.survey_id,
        //            order_no = mapping.order_no
        //        };

        //        _logger.LogInformation($"Survey question symptom mapping with Question ID {questionId} and Survey ID {surveyId} deleted successfully.");

        //        return new APIResponse<SurveyQuestionMapDeleteResponseDto>
        //        {
        //            isError = false,
        //            statusCode = StatusCodes.Status200OK,
        //            errorMessage = null,
        //            data = responseObj
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Exception occurred while deleting survey question symptom mapping for Question ID {QuestionId} and Survey ID {SurveyId}", questionId, surveyId);
        //        return new APIResponse<SurveyQuestionMapDeleteResponseDto>
        //        {
        //            isError = true,
        //            statusCode = StatusCodes.Status500InternalServerError,
        //            errorMessage = "An unexpected error occurred. Please try again later.",
        //            data = null
        //        };
        //    }
        //}


        public async Task<APIResponse<List<SurveyQuestionMapDeleteResponseDto>>> DeleteAllByQuestionAndSurveyAsync(int questionId, int surveyId)
        {
            try
            {
                _logger.LogInformation($"Deleting all survey question symptom mappings for Question ID {questionId} and Survey ID {surveyId}.");

                // Retrieve all matching records from the database
                var mappings = await _dbContext.survey_question_symptom_map
                    .Where(x => x.question_id == questionId && x.survey_id == surveyId)
                    .ToListAsync();

                if (mappings == null || !mappings.Any())
                {
                    _logger.LogError($"No survey question symptom mappings found for Question ID {questionId} and Survey ID {surveyId}.");
                    return new APIResponse<List<SurveyQuestionMapDeleteResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"No survey question symptom mappings found for Question ID {questionId} and Survey ID {surveyId}.",
                        data = null
                    };
                }

                // Convert records to response DTO before deletion
                var responseList = mappings.Select(mapping => new SurveyQuestionMapDeleteResponseDto
                {
                    id = mapping.id,
                    symptom_id = mapping.symptom_id,
                    question_id = mapping.question_id,
                    survey_id = mapping.survey_id,
                    order_no = mapping.order_no
                }).ToList();

                // Remove all matching records
                _dbContext.survey_question_symptom_map.RemoveRange(mappings);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Deleted {mappings.Count} survey question symptom mappings for Question ID {questionId} and Survey ID {surveyId}.");

                return new APIResponse<List<SurveyQuestionMapDeleteResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = responseList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting survey question symptom mappings for Question ID {QuestionId} and Survey ID {SurveyId}", questionId, surveyId);
                return new APIResponse<List<SurveyQuestionMapDeleteResponseDto>>
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
