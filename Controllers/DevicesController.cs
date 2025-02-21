
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            try
            {
                var devicesList = await _dbContext.devices
                    .Select(d => new DeviceResponseDto
                    {
                        id = d.id,
                        device_id = d.device_id,
                        device_type = d.device_type,
                        account_id = d.account_id,
                        clinic_id = d.clinic_id,
                        description = d.description,
                        device_status = d.device_status,
                        is_deleted = d.is_deleted,
                        created_at = d.created_at,
                        created_by = d.created_by,
                        updated_at = d.updated_at,
                        updated_by = d.updated_by
                    })
                    .Where(devicesList => !devicesList.is_deleted)
                    .AsNoTracking()
                    .ToListAsync();

                if (devicesList == null || !devicesList.Any())
                {
                    _logger.LogError("No devices found in the database.");
                    return NotFound(new APIResponse<List<DeviceResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No devices found in the database.",
                        data = null
                    });
                }

                _logger.LogInformation("Fetched all devices.");
                return Ok(new APIResponse<List<DeviceResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = devicesList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving devices: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<DeviceResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            try
            {
                var device = await _dbContext.devices
                    .Where(d => d.id == id)
                    .Select(d => new DeviceResponseDto
                    {
                        id = d.id,
                        device_id = d.device_id,
                        device_type = d.device_type,
                        account_id = d.account_id,
                        clinic_id = d.clinic_id,
                        description = d.description,
                        device_status = d.device_status,
                        is_deleted = d.is_deleted,
                        created_at = d.created_at,
                        created_by = d.created_by,
                        updated_at = d.updated_at,
                        updated_by = d.updated_by
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(device => device.id == id && !device.is_deleted);

                if (device == null)
                {
                    _logger.LogError($"Device with id {id} not found.");
                    return NotFound(new APIResponse<DeviceResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Device with id {id} not found.",
                        data = null
                    });
                }

                _logger.LogInformation($"Fetched device with id {id}.");
                return Ok(new APIResponse<DeviceResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = device
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching device: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<DeviceResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceRequestDto newDeviceDto)
        {
            try
            {
                if (newDeviceDto == null)
                {
                    _logger.LogError("Devices creation failed. Invalid request.");
                    return BadRequest(new APIResponse<DeviceResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Invalid request. Please check the input fields.",
                        data = null
                    });
                }

                bool clinicExists = await _dbContext.clinics.AnyAsync(a => a.id == newDeviceDto.clinic_id);

                if (!clinicExists)
                {
                    _logger.LogError("Clinic ID '{ClinicId}' does not exist.", newDeviceDto.clinic_id);
                    return NotFound(new APIResponse<DeviceResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic ID '{newDeviceDto.clinic_id}' does not exist.",
                        data = null
                    });
                }


                bool accountExists = await _dbContext.accounts.AnyAsync(a => a.id == newDeviceDto.account_id);

                if (!accountExists)
                {
                    _logger.LogError("Account ID '{account_id}' does not exist.", newDeviceDto.account_id);
                    return NotFound(new APIResponse<DeviceResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Account ID '{newDeviceDto.account_id}' does not exist.",
                        data = null
                    });
                }


                var newDevice = new Device
                {
                    device_id = newDeviceDto.device_id,
                    device_type = newDeviceDto.device_type,
                    account_id = newDeviceDto.account_id,
                    clinic_id = newDeviceDto.clinic_id,
                    description = newDeviceDto.description,
                    device_status = newDeviceDto.device_status,
                    is_deleted = false,
                    created_at = DateTime.UtcNow,                  
                   created_by = newDeviceDto.created_by,
                    
                };

                _dbContext.devices.Add(newDevice);
                await _dbContext.SaveChangesAsync();




                _logger.LogInformation($"Device {newDevice.device_id} created successfully.");

                return CreatedAtAction(nameof(GetDeviceById), new { id = newDevice.id }, new APIResponse<DeviceResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = new DeviceResponseDto
                    {
                        id = newDevice.id,
                        device_id = newDevice.device_id,
                        device_type = newDevice.device_type,
                        account_id = newDevice.account_id,
                        clinic_id = newDevice.clinic_id,
                        description = newDevice.description,
                        device_status = newDevice.device_status,
                        is_deleted = newDevice.is_deleted,
                        created_at = newDevice.created_at,
                        created_by = newDevice.created_by,
                        updated_at = newDevice.updated_at,
                        updated_by = newDevice.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<DeviceResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = $"Database error: {dbEx.Message}",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<DeviceResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceUpdateDto updatedDeviceDto)
        {
            try
            {
                var device = await _dbContext.devices.FindAsync(id);
                if (device == null)
                {
                    _logger.LogError($"Device with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Device with id {id} not found.",
                        data = null
                    });
                }

                device.device_id = updatedDeviceDto.device_id;
                device.device_type = updatedDeviceDto.device_type;
                device.account_id = updatedDeviceDto.account_id;
                device.clinic_id = updatedDeviceDto.clinic_id;
                device.description = updatedDeviceDto.description;
                device.device_status = updatedDeviceDto.device_status;
                device.updated_by = updatedDeviceDto.updated_by;
                device.updated_at = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Device {id} updated successfully.");
                return Ok(new APIResponse<DeviceResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = new DeviceResponseDto
                    {
                        id = device.id,
                        device_id = device.device_id,
                        device_type = device.device_type,
                        account_id = device.account_id,
                        clinic_id = device.clinic_id,
                        description = device.description,
                        device_status = device.device_status,
                        is_deleted = device.is_deleted,
                        created_at = device.created_at,
                        created_by = device.created_by,
                        updated_at = device.updated_at,
                        updated_by = device.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error: {dbEx.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<DeviceResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = $"Database error: {dbEx.Message}",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<DeviceResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                var device = await _dbContext.devices.FirstOrDefaultAsync(u => u.id == id && u.is_deleted == false);

                if (device == null)
                {
                    _logger.LogError($"Device with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Device with id {id} not found.",
                        data = null
                    });
                }

                device.is_deleted = true; // Soft delete
                device.updated_at = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Device {id} deleted successfully.");
                return Ok(new APIResponse<string>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = "Device deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting device: {ex.Message}");
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