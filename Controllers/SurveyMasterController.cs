
//using TrudoseAdminPortalAPI.DTO;

using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;

//using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyMasterController : ControllerBase
    {

        private readonly ISurveyMaster _symptomsService;
        private readonly ILogger<SurveyMasterController> _logger;

        public SurveyMasterController(ISurveyMaster symptomsService, ILogger<SurveyMasterController> logger)
        {
            _symptomsService = symptomsService;
            _logger = logger;
        }


        [HttpPost("survey")]
        public async Task<IActionResult> AddSurveyAsync([FromBody] SurveyMasterDto symptoms)
        {
            try
            {
                var response = await _symptomsService.AddSurveyAsync(symptoms);

                if (response.isError)
                {
                    _logger.LogError("Survey creation failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Survey creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSurveys()
        {
            try
            {
                var response = await _symptomsService.GetAllSurveysAsync();
                if (response.isError)
                {
                    _logger.LogError("Survey get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }
                return StatusCode(response.statusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Survey creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("getsurveybyid/{id}")]
        public async Task<IActionResult> GetSurveyByIdAsync(int id)
        {
            try
            {
                var response = await _symptomsService.GetSurveyByIdAsync(id);

                if (response.isError)
                {
                    _logger.LogError("Survey get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Survey creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }




        [HttpPut("updatesurveybyid/{id}")]
        public async Task<IActionResult> UpdateSurveyAsync(int id, [FromBody] SurveyMasterDto updatedSymptoms)
        {
            try
            {
                var response = await _symptomsService.UpdateSurveyAsync(id, updatedSymptoms);

                if (response.isError)
                {
                    _logger.LogError("Survey update failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;
                return Ok(new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }




        [HttpDelete("deletesurveybyid/{id}")]
        public async Task<IActionResult> DeleteSurveyAsync(int id)
        {
            try
            {
                var response = await _symptomsService.DeleteSurveyAsync(id);

                if (response.isError)
                {
                    _logger.LogError("Survey delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Survey");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyMaster>
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
