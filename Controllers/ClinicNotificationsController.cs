using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Models;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicNotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ClinicNotificationsController> _logger;

        public ClinicNotificationsController(
            ILogger<ClinicNotificationsController> logger,
            ApplicationDbContext dbContext
        )
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET: api/ClinicNotifications
        [HttpGet]
        public async Task<IActionResult> GetAllClinicNotifications()
        {
            try
            {
                var clinicNotifications = await _dbContext.clinic_notifications                   
                    .Select(cn => new ClinicNotificationResponseDto
                    {
                        id = cn.id,
                        notification_datetime = cn.notification_datetime,
                        message = cn.message,
                        status = cn.status,
                        clinic_id = cn.clinic_id,
                        is_deleted = cn.is_deleted,
                        created_at = cn.created_at,
                        created_by = cn.created_by,
                        updated_at = cn.updated_at,
                        updated_by = cn.updated_by
                    })
                    .Where(clinicNotifications => !clinicNotifications.is_deleted)
                    .AsNoTracking()
                    .ToListAsync();

                if (clinicNotifications == null || !clinicNotifications.Any())
                {
                    _logger.LogError("No clinic notifications found.");
                    return NotFound(new APIResponse<List<ClinicNotificationResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No clinic notifications found.",
                        data = null
                    });
                }

                _logger.LogInformation("Fetched all clinic notifications.");
                return Ok(new APIResponse<List<ClinicNotificationResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = clinicNotifications
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving clinic notifications: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<ClinicNotificationResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // GET: api/ClinicNotifications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinicNotificationById(int id)
        {
            try
            {
                var clinicNotification = await _dbContext.clinic_notifications                    
                    .Select(cn => new ClinicNotificationResponseDto
                    {
                        id = cn.id,
                        notification_datetime = cn.notification_datetime,
                        message = cn.message,
                        status = cn.status,
                        clinic_id = cn.clinic_id,
                        is_deleted = cn.is_deleted,
                        created_at = cn.created_at,
                        created_by = cn.created_by,
                        updated_at = cn.updated_at,
                        updated_by = cn.updated_by
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(clinicNotification => clinicNotification.id == id && !clinicNotification.is_deleted);

                if (clinicNotification == null)
                {
                    _logger.LogError($"Clinic notification with id {id} not found.");
                    return NotFound(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic notification with id {id} not found.",
                        data = null
                    });
                }

                _logger.LogInformation($"Fetched clinic notification with id {id}.");
                return Ok(new APIResponse<ClinicNotificationResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = clinicNotification
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching clinic notification: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicNotificationResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // POST: api/ClinicNotifications
        [HttpPost]
        public async Task<IActionResult> CreateClinicNotification([FromBody] ClinicNotificationRequestDto newClinicNotificationDto)
        {
            try
            {
                if (newClinicNotificationDto == null)
                {
                    _logger.LogError("Invalid request: Clinic notification data is null.");
                    return BadRequest(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Invalid request. Please check the input fields.",
                        data = null
                    });
                }

                // Validate clinic_id (if provided)
                if (newClinicNotificationDto.clinic_id.HasValue &&
                    !await _dbContext.clinics.AsNoTracking().AnyAsync(c => c.id == newClinicNotificationDto.clinic_id))
                {
                    _logger.LogError($"Clinic ID {newClinicNotificationDto.clinic_id} does not exist.");
                    return NotFound(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic ID {newClinicNotificationDto.clinic_id} does not exist.",
                        data = null
                    });
                }

                // Validate created_by user (if provided)
                if (newClinicNotificationDto.created_by.HasValue &&
                    !await _dbContext.clinic_users.AsNoTracking().AnyAsync(u => u.id == newClinicNotificationDto.created_by))
                {
                    _logger.LogError($"User ID {newClinicNotificationDto.created_by} does not exist.");
                    return NotFound(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"User ID {newClinicNotificationDto.created_by} does not exist.",
                        data = null
                    });
                }

                var newClinicNotification = new ClinicNotification
                {
                    notification_datetime = newClinicNotificationDto.notification_datetime,
                    message = newClinicNotificationDto.message,
                    status = newClinicNotificationDto.status ?? "Pending" ?? "Resolved" ?? "Active" ?? "Archived",
                    clinic_id = newClinicNotificationDto.clinic_id,
                    is_deleted = false,
                    created_at = DateTime.UtcNow,
                    created_by = newClinicNotificationDto.created_by,
                   
                };

                _dbContext.clinic_notifications.Add(newClinicNotification);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Clinic notification created successfully with ID {newClinicNotification.id}.");

                return CreatedAtAction(
                    nameof(GetClinicNotificationById),
                    new { id = newClinicNotification.id },
                    new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = false,
                        statusCode = StatusCodes.Status201Created,
                        errorMessage = null,
                        data = new ClinicNotificationResponseDto
                        {
                            id = newClinicNotification.id,
                            notification_datetime = newClinicNotification.notification_datetime,
                            message = newClinicNotification.message,
                            status = newClinicNotification.status,
                            clinic_id = newClinicNotification.clinic_id,
                            is_deleted = newClinicNotification.is_deleted,
                            created_at = newClinicNotification.created_at,
                            created_by = newClinicNotification.created_by,
                            updated_at = newClinicNotification.updated_at,
                            updated_by = newClinicNotification.updated_by
                        }
                    }
                );
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<ClinicNotificationResponseDto>
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
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicNotificationResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // PUT: api/ClinicNotifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinicNotification(int id, [FromBody] ClinicNotificationUpdateDto updatedClinicNotificationDto)
        {
            try
            {
                var clinicNotification = await _dbContext.clinic_notifications.FindAsync(id);
                if (clinicNotification == null || clinicNotification.is_deleted)
                {
                    _logger.LogError($"Clinic notification with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic notification with id {id} not found.",
                        data = null
                    });
                }

                // Validate clinic_id (if provided)
                if (updatedClinicNotificationDto.clinic_id.HasValue &&
                    !await _dbContext.clinics.AsNoTracking().AnyAsync(c => c.id == updatedClinicNotificationDto.clinic_id))
                {
                    _logger.LogError($"Clinic ID {updatedClinicNotificationDto.clinic_id} does not exist.");
                    return NotFound(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic ID {updatedClinicNotificationDto.clinic_id} does not exist.",
                        data = null
                    });
                }

                // Validate updated_by user (if provided)
                if (updatedClinicNotificationDto.updated_by.HasValue &&
                    !await _dbContext.clinic_users.AsNoTracking().AnyAsync(u => u.id == updatedClinicNotificationDto.updated_by))
                {
                    _logger.LogError($"User ID {updatedClinicNotificationDto.updated_by} does not exist.");
                    return NotFound(new APIResponse<ClinicNotificationResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"User ID {updatedClinicNotificationDto.updated_by} does not exist.",
                        data = null
                    });
                }

                clinicNotification.notification_datetime = updatedClinicNotificationDto.notification_datetime;
                clinicNotification.message = updatedClinicNotificationDto.message;
                clinicNotification.status = updatedClinicNotificationDto.status;
                clinicNotification.clinic_id = updatedClinicNotificationDto.clinic_id;
                clinicNotification.updated_by = updatedClinicNotificationDto.updated_by;
                clinicNotification.updated_at = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Clinic notification {id} updated successfully.");

                return Ok(new APIResponse<ClinicNotificationResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = new ClinicNotificationResponseDto
                    {
                        id = clinicNotification.id,
                        notification_datetime = clinicNotification.notification_datetime,
                        message = clinicNotification.message,
                        status = clinicNotification.status,
                        clinic_id = clinicNotification.clinic_id,
                        is_deleted = clinicNotification.is_deleted,
                        created_at = clinicNotification.created_at,
                        created_by = clinicNotification.created_by,
                        updated_at = clinicNotification.updated_at,
                        updated_by = clinicNotification.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<ClinicNotificationResponseDto>
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
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicNotificationResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // DELETE: api/ClinicNotifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinicNotification(int id)
        {
            try
            {
                var clinicNotification = await _dbContext.clinic_notifications.FirstOrDefaultAsync(u => u.id == id && u.is_deleted == false);
                if (clinicNotification == null || clinicNotification.is_deleted)
                {
                    _logger.LogError($"Clinic notification with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic notification with id {id} not found.",
                        data = null
                    });
                }

                clinicNotification.is_deleted = true;
                clinicNotification.updated_at = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Clinic notification {id} deleted successfully.");
                return Ok(new APIResponse<string>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = "Clinic notification deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting clinic notification: {ex.Message}");
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