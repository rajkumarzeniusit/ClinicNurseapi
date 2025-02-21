
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsMasterController : ControllerBase
    {

        private readonly ISymptomsMasterService _symptomsService;
        private readonly ILogger<SymptomsMasterController> _logger;

        public SymptomsMasterController(ISymptomsMasterService symptomsService, ILogger<SymptomsMasterController> logger)
        {
            _symptomsService = symptomsService;
            _logger = logger;
        }


        [HttpPost("symptoms")]
        public async Task<IActionResult> AddSymptomsMasterAsync([FromBody] SymptomsMasterDto symptoms)
        {
            try
            {
                var response = await _symptomsService.AddSymptomsMasterAsync(symptoms);

                if (response.isError)
                {
                    _logger.LogError("symptomsmaster creation failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during symptomsmaster creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }




        [HttpGet("allsymptoms")]
        public async Task<IActionResult> GetAllSymptomsMasterAsync()
        {
            try
            {
                var response = await _symptomsService.GetAllSymptomsMasterAsync();

                if (response.isError)
                {
                    _logger.LogError("symptomsmaster get all failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching all symptoms");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<SymptomsMaster>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("getsymptomsbyid/{id}")]
        public async Task<IActionResult> GetSymptomsMasterByIdAsync(int id)
        {
            try
            {
                var response = await _symptomsService.GetSymptomsMasterByIdAsync(id);

                if (response.isError)
                {
                    _logger.LogError("symptomsmaster get failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during symptomsmaster creation");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpPut("updatesymptomsbyid/{id}")]
        public async Task<IActionResult> UpdateSymptomsMasterAsync(int id, [FromBody] SymptomsMasterDto updatedSymptoms)
        {
            try
            {
                var response = await _symptomsService.UpdateSymptomsMasterAsync(id, updatedSymptoms);

                if (response.isError)
                {
                    _logger.LogError("symptomsmaster update failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;
                return Ok(new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for symptomsmaster");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SymptomsMaster>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpDelete("deletesymptomsbyid/{id}")]
        public async Task<IActionResult> DeleteSymptomsMasterAsync(int id)
        {
            try
            {
                var response = await _symptomsService.DeleteSymptomsMasterAsync(id);

                if (response.isError)
                {
                    _logger.LogError("symptomsmaster delete failed with status code {StatusCode}: {Message}", response.statusCode, response.errorMessage);
                    return StatusCode(response.statusCode, response);
                }

                var patient = response.data;

                return Ok(new APIResponse<SymptomsMaster>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing request for symptomsmaster");
                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SymptomsMaster>
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
