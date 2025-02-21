using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Models;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Data;
using Org.BouncyCastle.Asn1.Ocsp;
using TrudoseAdminPortalAPI.Dtos;

[Route("api/[controller]")]
[ApiController]
public class ClinicBillingContactController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ClinicBillingContactController> _logger;

    public ClinicBillingContactController(ApplicationDbContext context, ILogger<ClinicBillingContactController> logger)
    {
        _context = context;
        _logger = logger;
    }

 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var contacts = await _context.clinic_billing_contacts
                .Select(c => new ClinicBillingContactResponseDto
                {
                    id = c.id,
                    billing_email = c.billing_email,
                    billing_contact_name = c.billing_contact_name,
                    billing_contact_number = c.billing_contact_number,
                    clinic_id = c.clinic_id,
                    created_by = c.created_by,
                    updated_by = c.updated_by,
                    created_at = c.created_at,
                    updated_at = c.updated_at
                })
                .ToListAsync();
            if (contacts == null || !contacts.Any())
            {
                _logger.LogError("No Clinic Billing Contacts found.");
                return NotFound(new APIResponse<List<ClinicBillingContactResponseDto>>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = "No Clinic Billing Contacts found.",
                    data = null
                });
            }


            _logger.LogInformation("Fetched all Clinic Billing Contact. ");

            var response = new APIResponse<List<ClinicBillingContactResponseDto>>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data = contacts
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching ClinicBillingContacts: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }

  


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var contact = await _context.clinic_billing_contacts.FindAsync(id);

            if (contact == null)
            {
                _logger.LogWarning($"ClinicBillingContact with ID {id} not found.");
                return NotFound(new { Message = "ClinicBillingContact not found." });
            }

            var response = new ClinicBillingContactResponseDto
            {            

                id = contact.id,
                billing_email = contact.billing_email,
                billing_contact_name = contact.billing_contact_name,
                billing_contact_number = contact.billing_contact_number,
                clinic_id = contact.clinic_id,
                created_by = contact.created_by,
                updated_by = contact.updated_by,
                created_at = contact.created_at,
                updated_at = contact.updated_at
            };

            return Ok(new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data = response
            });

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching ClinicBillingContact with ID {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }

   


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClinicBillingContactRequestDto contactDto)
    {
        try
        {
            if (contactDto == null)
            {
                _logger.LogError("ClinicBillingContact creation failed. Invalid request.");
                return BadRequest(new APIResponse<ClinicResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status400BadRequest,
                    errorMessage = "Invalid request. Please check the input fields.",
                    data = null
                });
            }

            bool clinicExists = await _context.clinics.AsNoTracking()
                    .AnyAsync(a => a.id == contactDto.clinic_id);

            if (!clinicExists)
            {
                _logger.LogError("Clinic ID '{ClinicId}' does not exist.", contactDto.clinic_id);
                return NotFound(new APIResponse<ClinicAdminContactResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"Clinic ID '{contactDto.clinic_id}' does not exist.",
                    data = null
                });
            }

            var contact = new ClinicBillingContact
            {
                billing_email = contactDto.billing_email,
                billing_contact_name = contactDto.billing_contact_name,
                billing_contact_number = contactDto.billing_contact_number,
                clinic_id = contactDto.clinic_id,
                created_by = contactDto.created_by,                
                created_at = DateTime.UtcNow,
               
            };

            _context.clinic_billing_contacts.Add(contact);
            await _context.SaveChangesAsync();

            var response = new ClinicBillingContactResponseDto
            {
                id = contact.id,
                billing_email = contact.billing_email,
                billing_contact_name = contact.billing_contact_name,
                billing_contact_number = contact.billing_contact_number,
                clinic_id = contact.clinic_id,
                created_by = contact.created_by,
                updated_by = contact.updated_by,
                created_at = contact.created_at,
                updated_at = contact.updated_at
            };

            _logger.LogInformation($"ClinicBillingContact created with ID {contact.id}");
            return CreatedAtAction(nameof(GetById), new { id = contact.id }, new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status201Created,
                errorMessage = null,
                data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating ClinicBillingContact: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }

    


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClinicBillingContactUpdateDto updatedDto)
    {
        try
        {
            if (updatedDto == null)
            {
                _logger.LogWarning("Invalid ClinicBillingContact update request.");
                return BadRequest(new { Message = "Invalid data." });
            }

            var existingContact = await _context.clinic_billing_contacts.FindAsync(id);
            if (existingContact == null)
            {
                _logger.LogWarning($"ClinicBillingContact with ID {id} not found.");
                return NotFound(new { Message = "ClinicBillingContact not found." });
            }
            
            existingContact.clinic_id = updatedDto.clinic_id;
            existingContact.billing_email = updatedDto.billing_email;
            existingContact.billing_contact_name = updatedDto.billing_contact_name;
            existingContact.billing_contact_number = updatedDto.billing_contact_number;
            existingContact.updated_by = updatedDto.updated_by;           
            existingContact.updated_at = DateTime.UtcNow;

            _context.clinic_billing_contacts.Update(existingContact);
            await _context.SaveChangesAsync();

            var response = new ClinicBillingContactResponseDto
            {
                id = existingContact.id,
                billing_email = existingContact.billing_email,
                billing_contact_name = existingContact.billing_contact_name,
                billing_contact_number = existingContact.billing_contact_number,
                clinic_id = existingContact.clinic_id,
                created_by = existingContact.created_by,
                updated_by = existingContact.updated_by,
                created_at = existingContact.created_at,
                updated_at = existingContact.updated_at
            };

            _logger.LogInformation($"ClinicBillingContact with ID {id} updated.");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating ClinicBillingContact with ID {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<ClinicBillingContactResponseDto>
            {
                isError = true,
                statusCode = StatusCodes.Status500InternalServerError,
                errorMessage = "An unexpected error occurred. Please try again later.",
                data = null
            });
        }
    }

   


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var contact = await _context.clinic_billing_contacts.FindAsync(id);
            if (contact == null)
            {
                _logger.LogWarning($"ClinicBillingContact with ID {id} not found.");
                return NotFound(new APIResponse<SuccessResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status404NotFound,
                    errorMessage = $"ClinicBillingContact with ID {id} not found.",
                    data = null
                });
            }

            _context.clinic_billing_contacts.Remove(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"ClinicBillingContact with ID {id} deleted.");
            
            return Ok(new APIResponse<SuccessResponseDto>
            {
                isError = false,
                statusCode = StatusCodes.Status200OK,
                errorMessage = null,
                data =
                {
                    message = $"ClinicBillingContact with ID {id} deleted."
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting ClinicBillingContact with ID {id}: {ex.Message}");
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
