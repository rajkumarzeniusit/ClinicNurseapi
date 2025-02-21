using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;
using TrudoseAdminPortalAPI.Service;
using ClinicNurse.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyClinicMapController : ControllerBase
    {
        private readonly ISurveyClinicMap _symptomsService;
        private readonly ILogger<SurveyClinicMapController> _logger;

        public SurveyClinicMapController(ISurveyClinicMap symptomsService, ILogger<SurveyClinicMapController> logger)
        {
            _symptomsService = symptomsService;
            _logger = logger;
        }



        [HttpPost("surveyclinicmap")]
        public async Task<IActionResult> AddQuestionnaireAsync([FromBody] SurveyClinicMapDto symptoms)
        {
            try
            {
                var response = await _symptomsService.AddSurveyClinicMapAsync(symptoms);

                if (response.isError)
                {
                    _logger.LogError("SurveyClinicMap creation failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SurveyClinicMap creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpGet("surveyclinicmap/{id}")]
        public async Task<IActionResult> GetQuestionnaireByIdAsync(int id)
        {
            try
            {
                var response = await _symptomsService.GetSurveyClinicMapByIdAsync(id);

                if (response.isError)
                {
                    _logger.LogError("SurveyClinicMap get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SurveyClinicMap creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpDelete("surveyclinicmapbyid/{id}")]
        public async Task<IActionResult> DeleteQuestionnaireAsync(int id)
        {
            try
            {
                var response = await _symptomsService.DeleteSurveyClinicMapAsync(id);

                if (response.isError)
                {
                    _logger.LogError("SurveyClinicMap delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyClinicMap");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSurveyClinicMap(int id, [FromBody] SurveyClinicMapDto surveyClinicMapDto)
        {
            try
            {
                if (surveyClinicMapDto == null)
                {
                    return BadRequest(new { message = "Invalid input data." });
                }

                var response = await _symptomsService.UpdateSurveyClinicMapAsync(id, surveyClinicMapDto);

                if (response.isError)
                {
                    _logger.LogError("SurveyClinicMap update failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SurveyClinicMap>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for SurveyClinicMap");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyClinicMap>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("getall")]
        public async Task<IActionResult> GetAllSurveyClinicMaps()
        {
            try
            {
                var response = await _symptomsService.GetAllSurveyClinicMapsAsync();

                if (response.isError)
                {
                    _logger.LogError("SurveyClinicMap getall failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new { message = "SurveyClinicMap records retrieved successfully.", data = response.data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for Questionnaire");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SurveyClinicMap>
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
