using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ClinicController : ControllerBase
{
    private readonly ApplicationDbContext _dbcontext;
    private readonly ILogger<ClinicController> _logger;

    public ClinicController(ApplicationDbContext dbcontext, ILogger<ClinicController> logger)
    {
        _dbcontext = dbcontext;
        _logger = logger;
    }

    // GET: api/clinic
    [HttpGet]
    public async Task<IActionResult> GetAllClinics()
    {
        try
        {
            _logger.LogInformation("Fetching all clinics from the database.");
            var clinics = await _dbcontext.clinics
                .Select(c => new ClinicResponseDto
                {
                    id = c.id,
                    account_id = c.account_id,
                    clinic_name = c.clinic_name,
                    clinic_type = c.clinic_type,
                    clinic_status = c.clinic_status,
                    address1 = c.address1,
                    address2 = c.address2,
                    city = c.city,
                    state = c.state,
                    zip_code = c.zip_code,
                    country = c.country,
                    primary_contact_name = c.primary_contact_name,
                    primary_contact_number = c.primary_contact_number,
                    secondary_contact_number = c.secondary_contact_number,
                    email = c.email,
                    is_deleted = c.is_deleted,
                    created_at = c.created_at,
                    updated_by = c.updated_by,
                    created_by = c.created_by,
                    updated_at  = c.updated_at,
                })
                .Where(clinics => !clinics.is_deleted)
                    .AsNoTracking()
                    .ToListAsync();


            if (clinics == null)
            {
                _logger.LogError($"Clinics not found in database.");
                return NotFound(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Clinics not found in database.",
                    data = null
                });
            }


            _logger.LogInformation("Successfully retrieved {Count} clinics.", clinics.Count);           
            var response = new APIResponse<List<ClinicResponseDto>>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data = clinics
            };
            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving clinics.");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }

    // GET: api/clinic/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClinicById(int id)
    {
        try
        {
            _logger.LogInformation("Fetching clinic with ID: {Id}", id);
            var clinic = await _dbcontext.clinics.AsNoTracking()
                    .FirstOrDefaultAsync(clinic => clinic.id == id && !clinic.is_deleted);

            if (clinic == null)
            {
                _logger.LogError($"Clinics not found in database.");
                return NotFound(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Clinics not found in database.",
                    data = null
                });
            }


       

            var response = new ClinicResponseDto
            {
                id = clinic.id,
                account_id = clinic.account_id,
                clinic_name = clinic.clinic_name,
                clinic_type = clinic.clinic_type,
                clinic_status = clinic.clinic_status,
                address1 = clinic.address1,
                address2 = clinic.address2,
                city = clinic.city,
                state = clinic.state,
                zip_code = clinic.zip_code,
                country = clinic.country,
                primary_contact_name = clinic.primary_contact_name,
                primary_contact_number = clinic.primary_contact_number,
                secondary_contact_number = clinic.secondary_contact_number,
                email = clinic.email,
                is_deleted = clinic.is_deleted,
                created_at = clinic.created_at,
                updated_at = clinic.updated_at,
                created_by = clinic.created_by,
                updated_by = clinic.updated_by
            };

            _logger.LogInformation("Successfully retrieved clinic with ID: {Id}", id);
            return Ok(new APIResponse<ClinicResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data = response
            });
          
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving clinic with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateClinic([FromBody] ClinicRequestDto request)
    {
        try
        {
            _logger.LogInformation("Creating a new clinic: {ClinicName}", request.clinic_name);

            if (!ModelState.IsValid)  // Fix the ModelState check
            {
                _logger.LogError("Clinic creation failed. Invalid request.");
                return BadRequest(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "Invalid request. Please check the input fields.",
                    data = null
                });
            }

            // Ensure the account_id exists
            bool accountExists = await _dbcontext.accounts.AsNoTracking()
                .AnyAsync(a => a.id == request.account_id);

            if (!accountExists)
            {
                _logger.LogError("Clinic creation failed. Account ID '{AccountId}' does not exist.", request.account_id);
                return NotFound(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Account ID '{request.account_id}' does not exist.",
                    data = null
                });
            }

            var clinic = new Clinic
            {
                account_id = request.account_id,
                clinic_name = request.clinic_name,
                clinic_type = request.clinic_type,
                clinic_status = request.clinic_status,
                address1 = request.address1,
                address2 = request.address2,
                city = request.city,
                state = request.state,
                zip_code = request.zip_code,
                country = request.country,
                primary_contact_name = request.primary_contact_name,
                primary_contact_number = request.primary_contact_number,
                secondary_contact_number = request.secondary_contact_number,
                email = request.email,
                is_deleted = false,
                created_by = request.created_by,               
                created_at = DateTime.UtcNow,  
               
            };

            await _dbcontext.clinics.AddAsync(clinic);  // Ensure it's async
            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation("Clinic created successfully with ID: {Id}", clinic.id);

            var responseDto = new ClinicResponseDto
            {
                id = clinic.id,
                account_id = clinic.account_id,
                clinic_name = clinic.clinic_name,
                clinic_type = clinic.clinic_type,
                clinic_status = clinic.clinic_status,
                address1 = clinic.address1,
                address2 = clinic.address2,
                city = clinic.city,
                state = clinic.state,
                zip_code = clinic.zip_code,
                country = clinic.country,
                primary_contact_name = clinic.primary_contact_name,
                primary_contact_number = clinic.primary_contact_number,
                secondary_contact_number = clinic.secondary_contact_number,
                email = clinic.email,
                created_at = clinic.created_at,
                updated_by = clinic.updated_by,
                created_by = clinic.created_by,
                updated_at = clinic.updated_at
            };

            return CreatedAtAction(nameof(GetClinicById), new { id = clinic.id }, new APIResponse<ClinicResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status201Created,
                errorMessage = null,
                data = responseDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new clinic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClinic(int id, [FromBody] ClinicUpdateDto request)
    {
        try
        {
            _logger.LogInformation("Updating clinic with ID: {Id}", id);

            var clinic = await _dbcontext.clinics.FirstOrDefaultAsync(c => c.id == id);
            if (clinic == null)
            {
                _logger.LogError("Clinic with ID: {Id} not found.", id);
                return NotFound(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Clinic with ID {id} not found",
                    data = null
                });
            }

            clinic.account_id = request.account_id;
            clinic.clinic_name = request.clinic_name;
            clinic.clinic_type = request.clinic_type;
            clinic.clinic_status = request.clinic_status;
            clinic.address1 = request.address1;
            clinic.address2 = request.address2;
            clinic.city = request.city;
            clinic.state = request.state;
            clinic.zip_code = request.zip_code;
            clinic.country = request.country;
            clinic.primary_contact_name = request.primary_contact_name;
            clinic.primary_contact_number = request.primary_contact_number;
            clinic.secondary_contact_number = request.secondary_contact_number;
            clinic.email = request.email;          
           
            clinic.updated_by = request.updated_by;
            clinic.updated_at = DateTime.UtcNow;

            _dbcontext.Entry(clinic).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation("Clinic with ID: {Id} updated successfully.", id);

            var responseDto = new ClinicResponseDto
            {
                id = clinic.id,
                account_id = clinic.account_id,
                clinic_name = clinic.clinic_name,
                clinic_type = clinic.clinic_type,
                clinic_status = clinic.clinic_status,
                address1 = clinic.address1,
                address2 = clinic.address2,
                city = clinic.city,
                state = clinic.state,
                zip_code = clinic.zip_code,
                country = clinic.country,
                primary_contact_name = clinic.primary_contact_name,
                primary_contact_number = clinic.primary_contact_number,
                secondary_contact_number = clinic.secondary_contact_number,
                email = clinic.email,
                created_at = clinic.created_at, 
                updated_by = clinic.updated_by,
                created_by = clinic.created_by,
                updated_at = clinic.updated_at,
            };

            return Ok(new APIResponse<ClinicResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data = responseDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the clinic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        try
        {
            var clinic = await _dbcontext.clinics.FirstOrDefaultAsync(u => u.id == id && u.is_deleted == false);

            if (clinic == null)
            {
                _logger.LogError($"Clinic with id {id} not found.");
                return NotFound(new APIResponse<SuccessResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Clinic with id {id} not found.",
                    data = null
                });
            }

            clinic.is_deleted = true;
            clinic.updated_at = DateTime.UtcNow;

            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation($"Clinic {id} deleted successfully.");
            return Ok(new APIResponse<SuccessResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data =
                {
                    message = $"Clinic {id} deleted successfully."
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting Clinic: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<SuccessResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }




}
