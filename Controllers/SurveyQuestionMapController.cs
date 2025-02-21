
using Admin.Models;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyQuestionMapController : ControllerBase
    {

        private readonly ISurveyQuestionMapService _symptomsService;
        private readonly ILogger<SurveyQuestionMapController> _logger;

        public SurveyQuestionMapController(ISurveyQuestionMapService symptomsService, ILogger<SurveyQuestionMapController> logger)
        {
            _symptomsService = symptomsService;
            _logger = logger;
        }


        [HttpPost("surveyquestionmap")]
        public async Task<IActionResult> AddPatientInfo([FromBody] PatientSurveyQuestionMapDto symptoms)
        {
            try
            {
                var response = await _symptomsService.AddSymptomsAsync(symptoms);

                if (response.isError)
                {
                    _logger.LogError("surveyquestionmap creation failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during surveyquestionmap creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<PatientSurveyQuestionMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpGet("surveyquestionmap/all")]
        public async Task<IActionResult> GetAllSymptoms()
        {
            try
            {
                var response = await _symptomsService.GetAllquestionsbysurveyidAsync();

                if (response.isError)
                {
                    _logger.LogError("Failed to retrieve all Symptoms. Status Code: {StatusCode}, Message: {Message}",
                        response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                return Ok(new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = response.data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving all Symptoms data");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("getsurveyquestionmapbyid/{id}")]
        public async Task<IActionResult> GetSymptomById(int id)
        {
            try
            {
                var response = await _symptomsService.GetSymptomsByIdAsync(id);

                if (response.isError)
                {
                    _logger.LogError("surveyquestionmap get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during surveyquestionmap get");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<PatientSurveyQuestionMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpPut("updatesurveyquestionmapbyid/{id}")]
        public async Task<IActionResult> UpdatePatientInfo(int id, [FromBody] PatientSurveyQuestionMapUpdateDto updatedSymptoms)
        {
            try
            {
                var response = await _symptomsService.UpdateSymptomsAsync(id, updatedSymptoms);

                if (response.isError)
                {
                    _logger.LogError("surveyquestionmap update failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;
                return Ok(new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for surveyquestionmap");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<PatientSurveyQuestionMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }



        [HttpDelete("deletesurveyquestionmapbyid/{id}")]
        public async Task<IActionResult> Deletesymptoms(int id)
        {
            try
            {
                var response = await _symptomsService.DeleteSymptomsAsync(id);

                if (response.isError)
                {
                    _logger.LogError("surveyquestionmap delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyQuestionMapResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for surveyquestionmap");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<PatientSurveyQuestionMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("surveyquestionmap/questions/{surveyId}")]
        public async Task<IActionResult> GetQuestionsBySurveyId(int surveyId)
        {
            try
            {
                var response = await _symptomsService.GetQuestionsBySurveyIdAsync(surveyId);

                if (response.isError)
                {
                    _logger.LogError("Failed to retrieve questions for Survey ID {SurveyId}. Status Code: {StatusCode}, Message: {Message}",
                        surveyId, response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                return Ok(new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = response.data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving questions for Survey ID {SurveyId}", surveyId);
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<SurveyQuestionMapResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }



        [HttpDelete("deletesurveyquestionmapby/{questionId}/{surveyId}")]
        public async Task<IActionResult> DeleteSurveyQuestionMap(int questionId, int surveyId)
        {
            try
            {
                var response = await _symptomsService.DeleteAllByQuestionAndSurveyAsync(questionId, surveyId);

                if (response.isError)
                {
                    _logger.LogError("surveyquestionmap delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                return Ok(response);

                //return Ok(new APIResponse<SurveyQuestionMapDeleteResponseDto>
                //{
                //    isError = false,
                //    statusCode = StatusCodes.Status200OK,
                //    errorMessage = null,
                //    data = response.data
                //});

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for surveyquestionmap");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyQuestionMapDeleteResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });

            }
        }

    }
}
