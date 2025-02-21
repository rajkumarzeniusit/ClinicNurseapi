using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Dtos;
using TrudoseAdminPortalAPI.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicAdminContactController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ClinicAdminContactController> _logger;

        public ClinicAdminContactController(ApplicationDbContext dbContext, ILogger<ClinicAdminContactController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateClinicAdminContact([FromBody] ClinicAdminContactRequestDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ClinicAdminContact creation failed. Invalid request.");
                    return BadRequest(new APIResponse<ClinicAdminContactResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = "Invalid request. Please check the input fields.",
                        data = null
                    });
                }

                bool clinicExists = await _dbContext.clinics.AnyAsync(a => a.id == requestDto.clinic_id);

                if (!clinicExists)
                {
                    _logger.LogError("Clinic ID '{ClinicId}' does not exist.", requestDto.clinic_id);
                    return NotFound(new APIResponse<ClinicAdminContactResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Clinic ID '{requestDto.clinic_id}' does not exist.",
                        data = null
                    });
                }

                var newAdminContact = new ClinicAdminContact
                {
                    admin_email = requestDto.admin_email,
                    admin_contact_name = requestDto.admin_contact_name,
                    admin_contact_number = requestDto.admin_contact_number,
                    clinic_id = requestDto.clinic_id,
                    created_by = requestDto.created_by,                   
                    created_at = DateTime.UtcNow,
                   
                };

                _dbContext.clinic_admin_contacts.Add(newAdminContact);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"ClinicAdminContact {newAdminContact.admin_email} created successfully.");

                // Convert entity to response DTO
                var responseDto = new ClinicAdminContactResponseDto
                {
                    id = newAdminContact.id,
                    admin_email = newAdminContact.admin_email,
                    admin_contact_name = newAdminContact.admin_contact_name,
                    admin_contact_number = newAdminContact.admin_contact_number,
                    clinic_id = newAdminContact.clinic_id,
                    created_by = newAdminContact.created_by,
                    updated_by = newAdminContact.updated_by,
                    created_at = newAdminContact.created_at,
                    updated_at = newAdminContact.updated_at
                };

                return CreatedAtAction(nameof(GetClinicAdminContactById), new { id = newAdminContact.id }, new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = responseDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a ClinicAdminContact.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        private ClinicAdminContactResponseDto MapToDto(ClinicAdminContact contact)
        {
            return new ClinicAdminContactResponseDto
            {
                id = contact.id,
                admin_email = contact.admin_email,
                admin_contact_name = contact.admin_contact_name,
                admin_contact_number = contact.admin_contact_number,
                clinic_id = contact.clinic_id,
                created_by = contact.created_by,
                updated_by = contact.updated_by,
                created_at = contact.created_at,
                updated_at = contact.updated_at
            };
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinicAdminContactById(int id)
        {
            try
            {
                var contact = await _dbContext.clinic_admin_contacts.FindAsync(id);
                if (contact == null)
                {
                    _logger.LogError("ClinicAdminContact with ID {Id} not found.", id);
                    return NotFound(new APIResponse<ClinicAdminContactResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"ClinicAdminContact with ID {id} not found.",
                        data = null
                    });
                }


                var reposne = new ClinicAdminContactResponseDto
                {
                    id = contact.id,
                    admin_email = contact.admin_email,
                    admin_contact_name = contact.admin_contact_name,
                    admin_contact_number = contact.admin_contact_number,
                    clinic_id = contact.clinic_id,
                    created_by = contact.created_by,
                    updated_by = contact.updated_by,
                    created_at = contact.created_at,
                    updated_at = contact.updated_at

                };


                _logger.LogInformation("Fetched all Clinic Admin Contacts. ");
                return Ok(new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = reposne
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ClinicAdminContact with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllClinicAdminContacts()
        {
            try
            {
                var contacts = await _dbContext.clinic_admin_contacts.ToListAsync();
                if (!contacts.Any())
                {
                    _logger.LogInformation("No ClinicAdminContacts found.");
                    return NotFound(new APIResponse<List<ClinicAdminContactResponseDto>>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = "No ClinicAdminContacts found.",
                        data = null
                    });
                }

               

                return Ok(new APIResponse<List<ClinicAdminContactResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = contacts.Select(MapToDto).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ClinicAdminContacts.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<ClinicAdminContactResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinicAdminContact(int id, [FromBody] ClinicAdminContactUpdateDto requestDto)
        {
            try
            {
                var existingContact = await _dbContext.clinic_admin_contacts.FindAsync(id);
                if (existingContact == null)
                {
                    _logger.LogWarning("ClinicAdminContact with ID {Id} not found for update.", id);
                    return NotFound(new APIResponse<ClinicAdminContactResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"ClinicAdminContact with ID {id} not found.",
                        data = null
                    });
                }

                // Update fields
                existingContact.admin_email = requestDto.admin_email;
                existingContact.admin_contact_name = requestDto.admin_contact_name;
                existingContact.admin_contact_number = requestDto.admin_contact_number;
                existingContact.updated_by = requestDto.updated_by;
                existingContact.updated_at = DateTime.UtcNow;

                _dbContext.clinic_admin_contacts.Update(existingContact);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("ClinicAdminContact with ID {Id} updated successfully.", id);
                return Ok(new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = MapToDto(existingContact) // Convert to DTO before returning
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating ClinicAdminContact with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinicAdminContact(int id)
        {
            try
            {
                var contact = await _dbContext.clinic_admin_contacts.FindAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning("ClinicAdminContact with ID {Id} not found for deletion.", id);
                    return NotFound(new APIResponse<SuccessResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"ClinicAdminContact with ID {id} not found.",
                        data = null
                    });
                }

                _dbContext.clinic_admin_contacts.Remove(contact);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("ClinicAdminContact with ID {Id} deleted successfully.", id);
                return Ok(new APIResponse<SuccessResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data =
                    {
                        message = "ClinicAdminContact with ID {Id} deleted successfully.",
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting ClinicAdminContact with ID {Id}.", id);
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
}
