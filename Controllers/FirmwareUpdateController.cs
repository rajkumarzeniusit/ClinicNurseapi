using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;

using TrudoseAdminPortalAPI.Models;
using TrudoseAdminPortalAPI.Dto;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmwareUpdatesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FirmwareUpdatesController> _logger;

        public FirmwareUpdatesController(
            ILogger<FirmwareUpdatesController> logger,
            ApplicationDbContext dbContext
        )
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET: api/FirmwareUpdates
        [HttpGet]
        public async Task<IActionResult> GetAllFirmwareUpdates()
        {
            try
            {
                var firmwareUpdates = await _dbContext.firmware_updates                    
                    .Select(f => new FirmwareUpdateResponseDto
                    {
                        id = f.id,
                        firmware_update_name = f.firmware_update_name,
                        firmware_update_path = f.firmware_update_path,
                        firmware_update_version = f.firmware_update_version,
                        device_type = f.device_type,
                        device_applicable_version = f.device_applicable_version,
                        description = f.description,
                        is_deleted = f.is_deleted,
                        created_at = f.created_at,
                        created_by = f.created_by,
                        updated_at = f.updated_at,
                        updated_by = f.updated_by
                    })
                    .Where(firmwareUpdates => !firmwareUpdates.is_deleted)
                    .AsNoTracking()
                    .ToListAsync();

                if (firmwareUpdates == null || !firmwareUpdates.Any())
                {
                    _logger.LogError("No firmware updates found.");
                    return NotFound(new APIResponse<List<FirmwareUpdateResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No firmware updates found.",
                        data = null
                    });
                }

                _logger.LogInformation("Fetched all firmware updates.");
                return Ok(new APIResponse<List<FirmwareUpdateResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = firmwareUpdates
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving firmware updates: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<FirmwareUpdateResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // GET: api/FirmwareUpdates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFirmwareUpdateById(int id)
        {
            try
            {
                var firmwareUpdate = await _dbContext.firmware_updates                    
                    .Select(f => new FirmwareUpdateResponseDto
                    {
                        id = f.id,
                        firmware_update_name = f.firmware_update_name,
                        firmware_update_path = f.firmware_update_path,
                        firmware_update_version = f.firmware_update_version,
                        device_type = f.device_type,
                        device_applicable_version = f.device_applicable_version,
                        description = f.description,
                        is_deleted = f.is_deleted,
                        created_at = f.created_at,
                        created_by = f.created_by,
                        updated_at = f.updated_at,
                        updated_by = f.updated_by
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(firmwareUpdate => firmwareUpdate.id == id && !firmwareUpdate.is_deleted);

                if (firmwareUpdate == null)
                {
                    _logger.LogError($"Firmware update with id {id} not found.");
                    return NotFound(new APIResponse<FirmwareUpdateResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Firmware update with id {id} not found.",
                        data = null
                    });
                }

                _logger.LogInformation($"Fetched firmware update with id {id}.");
                return Ok(new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = firmwareUpdate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching firmware update: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // POST: api/FirmwareUpdates
        [HttpPost]
        public async Task<IActionResult> CreateFirmwareUpdate([FromBody] FirmwareUpdateRequestDto newFirmwareUpdateDto)
        {
            try
            {
                if (newFirmwareUpdateDto == null)
                {
                    _logger.LogError("Invalid request: Firmware update data is null.");
                    return BadRequest(new APIResponse<FirmwareUpdateResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Invalid request. Please check the input fields.",
                        data = null
                    });
                }

                // Validate created_by user (if provided)
                if (newFirmwareUpdateDto.created_by.HasValue &&
                    !await _dbContext.clinic_users.AsNoTracking().AnyAsync(u => u.id == newFirmwareUpdateDto.created_by))
                {
                    _logger.LogError($"User ID {newFirmwareUpdateDto.created_by} does not exist.");
                    return NotFound(new APIResponse<FirmwareUpdateResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"User ID {newFirmwareUpdateDto.created_by} does not exist.",
                        data = null
                    });
                }

                var newFirmwareUpdate = new FirmwareUpdate
                {
                    firmware_update_name = newFirmwareUpdateDto.firmware_update_name,
                    firmware_update_path = newFirmwareUpdateDto.firmware_update_path,
                    firmware_update_version = newFirmwareUpdateDto.firmware_update_version,
                    device_type = newFirmwareUpdateDto.device_type,
                    device_applicable_version = newFirmwareUpdateDto.device_applicable_version,
                    description = newFirmwareUpdateDto.description,
                    is_deleted = false,
                    created_at = DateTime.UtcNow,
                    created_by = newFirmwareUpdateDto.created_by,                   
                   
                };

                _dbContext.firmware_updates.Add(newFirmwareUpdate);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Firmware update {newFirmwareUpdate.firmware_update_name} created successfully.");

                return CreatedAtAction(
                    nameof(GetFirmwareUpdateById),
                    new { id = newFirmwareUpdate.id },
                    new APIResponse<FirmwareUpdateResponseDto>
                    {
                        isError = false,
                        statusCode = StatusCodes.Status201Created,
                        errorMessage = null,
                        data = new FirmwareUpdateResponseDto
                        {
                            id = newFirmwareUpdate.id,
                            firmware_update_name = newFirmwareUpdate.firmware_update_name,
                            firmware_update_path = newFirmwareUpdate.firmware_update_path,
                            firmware_update_version = newFirmwareUpdate.firmware_update_version,
                            device_type = newFirmwareUpdate.device_type,
                            device_applicable_version = newFirmwareUpdate.device_applicable_version,
                            description = newFirmwareUpdate.description,
                            is_deleted = newFirmwareUpdate.is_deleted,
                            created_at = newFirmwareUpdate.created_at,
                            created_by = newFirmwareUpdate.created_by,
                            updated_at = newFirmwareUpdate.updated_at,
                            updated_by = newFirmwareUpdate.updated_by
                        }
                    }
                );
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = $"Database error: {dbEx.Message}",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // PUT: api/FirmwareUpdates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFirmwareUpdate(int id, [FromBody] FirmwareUpdateDto updatedFirmwareUpdateDto)
        {
            try
            {
                var firmwareUpdate = await _dbContext.firmware_updates.FindAsync(id);
                if (firmwareUpdate == null || firmwareUpdate.is_deleted)
                {
                    _logger.LogError($"Firmware update with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Firmware update with id {id} not found.",
                        data = null
                    });
                }

                // Validate updated_by user (if provided)
                if (updatedFirmwareUpdateDto.updated_by.HasValue &&
                    !await _dbContext.clinic_users.AsNoTracking().AnyAsync(u => u.id == updatedFirmwareUpdateDto.updated_by))
                {
                    _logger.LogError($"User ID {updatedFirmwareUpdateDto.updated_by} does not exist.");
                    return NotFound(new APIResponse<FirmwareUpdateResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"User ID {updatedFirmwareUpdateDto.updated_by} does not exist.",
                        data = null
                    });
                }

                firmwareUpdate.firmware_update_name = updatedFirmwareUpdateDto.firmware_update_name;
                firmwareUpdate.firmware_update_path = updatedFirmwareUpdateDto.firmware_update_path;
                firmwareUpdate.firmware_update_version = updatedFirmwareUpdateDto.firmware_update_version;
                firmwareUpdate.device_type = updatedFirmwareUpdateDto.device_type;
                firmwareUpdate.device_applicable_version = updatedFirmwareUpdateDto.device_applicable_version;
                firmwareUpdate.description = updatedFirmwareUpdateDto.description;
                firmwareUpdate.updated_by = updatedFirmwareUpdateDto.updated_by;
                firmwareUpdate.updated_at = DateTime.UtcNow;
               

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Firmware update {id} updated successfully.");

                return Ok(new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = new FirmwareUpdateResponseDto
                    {
                        id = firmwareUpdate.id,
                        firmware_update_name = firmwareUpdate.firmware_update_name,
                        firmware_update_path = firmwareUpdate.firmware_update_path,
                        firmware_update_version = firmwareUpdate.firmware_update_version,
                        device_type = firmwareUpdate.device_type,
                        device_applicable_version = firmwareUpdate.device_applicable_version,
                        description = firmwareUpdate.description,
                        is_deleted = firmwareUpdate.is_deleted,
                        created_at = firmwareUpdate.created_at,
                        created_by = firmwareUpdate.created_by,
                        updated_at = firmwareUpdate.updated_at,
                        updated_by = firmwareUpdate.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = $"Database error: {dbEx.Message}",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<FirmwareUpdateResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // DELETE: api/FirmwareUpdates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFirmwareUpdate(int id)
        {
            try
            {
                var firmwareUpdate = await _dbContext.firmware_updates.FirstOrDefaultAsync(u => u.id == id && u.is_deleted == false);

                if (firmwareUpdate == null || firmwareUpdate.is_deleted)
                {
                    _logger.LogError($"Firmware update with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Firmware update with id {id} not found.",
                        data = null
                    });
                }

                firmwareUpdate.is_deleted = true;
                firmwareUpdate.updated_at = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Firmware update {id} deleted successfully.");
                return Ok(new APIResponse<string>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = "Firmware update deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting firmware update: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<string>
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