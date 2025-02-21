
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicNurse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsMasterController : ControllerBase
    {
        private readonly IQuestionsMaster _symptomsService;
        private readonly ILogger<QuestionsMasterController> _logger;

        public QuestionsMasterController(IQuestionsMaster symptomsService, ILogger<QuestionsMasterController> logger)
        {
            _symptomsService = symptomsService;
            _logger = logger;
        }

        [HttpPost("questions")]
        public async Task<IActionResult> AddQuestionnaireAsync([FromBody] QuestionsMasterDto symptoms)
        {
            try
            {
                var response = await _symptomsService.AddQuestionnaireAsync(symptoms);

                if (response.isError)
                {
                    _logger.LogError("Questionnaire creation failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Questionnaire creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuestionnaires()
        {
            try
            {
                var response = await _symptomsService.GetAllQuestionnairesAsync();
                if (response.isError)
                {
                    _logger.LogError("Questionnaire get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                return StatusCode(response.statusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Questionnaire creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }

        }


        [HttpGet("questionbyid/{id}")]
        public async Task<IActionResult> GetQuestionnaireByIdAsync(int id)
        {
            try
            {
                var response = await _symptomsService.GetQuestionnaireByIdAsync(id);

                if (response.isError)
                {
                    _logger.LogError("Questionnaire get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Questionnaire creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }



        [HttpPut("updatequestionbyid/{id}")]
        public async Task<IActionResult> UpdateQuestionnaireAsync(int id, [FromBody] QuestionsMasterDto updatedSymptoms)
        {
            try
            {
                var response = await _symptomsService.UpdateQuestionnaireAsync(id, updatedSymptoms);

                if (response.isError)
                {
                    _logger.LogError("Questionnaire update failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;
                return Ok(new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<QuestionsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpDelete("deletequestionbyid/{id}")]
        public async Task<IActionResult> DeleteQuestionnaireAsync(int id)
        {
            try
            {
                var response = await _symptomsService.DeleteQuestionnaireAsync(id);

                if (response.isError)
                {
                    _logger.LogError("Questionnaire delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<QuestionsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<QuestionsMaster>
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
