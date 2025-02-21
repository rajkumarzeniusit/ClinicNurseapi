using Microsoft.AspNetCore.Mvc;
using TruDoseAPI.Models;
using Microsoft.EntityFrameworkCore;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Data;

namespace TruDoseAPI.Controllers
{
    //[Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accountsList = await _dbContext.accounts
                    .Select(a => new AccountResponseDto
                    {
                        id = a.id,
                        account_name = a.account_name,
                        address1 = a.address1,
                        address2 = a.address2,
                        city = a.city,
                        state = a.state,
                        zip_code = a.zip_code,
                        country = a.country,
                        primary_contact_name = a.primary_contact_name,
                        primary_contact_number = a.primary_contact_number,
                        email = a.email,
                        specialization = a.specialization,
                        description = a.description,
                        is_deleted = a.is_deleted,
                        created_at = a.created_at,
                        created_by = a.created_by,
                        updated_at = a.updated_at,
                        updated_by = a.updated_by
                    })
                    .Where(accountsList => !accountsList.is_deleted)
                    .AsNoTracking()
                    .ToListAsync();


                if (accountsList == null)
                {
                    _logger.LogError($"Accounts not found in database.");
                    return NotFound(new APIResponse<AccountResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Accounts not found in database.",
                        data = null
                    });
                }



                _logger.LogInformation("Fetched all accounts.");
                var response = new APIResponse<List<AccountResponseDto>>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,                    
                    data = accountsList
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving accounts: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var account = await _dbContext.accounts
                    .Where(a => a.id == id)
                    .Select(a => new AccountResponseDto
                    {
                        id = a.id,
                        account_name = a.account_name,
                        address1 = a.address1,
                        address2 = a.address2,
                        city = a.city,
                        state = a.state,
                        zip_code = a.zip_code,
                        country = a.country,
                        primary_contact_name = a.primary_contact_name,
                        primary_contact_number = a.primary_contact_number,
                        email = a.email,
                        specialization = a.specialization,
                        description = a.description,
                        is_deleted = a.is_deleted,
                        created_at = a.created_at,
                        created_by = a.created_by,
                        updated_at = a.updated_at,
                        updated_by = a.updated_by
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(account => account.id == id && !account.is_deleted);

                if (account == null)
                {
                    _logger.LogError($"Account with id {id} not found.");
                    return NotFound(new APIResponse<AccountResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Account with id {id} not found.",
                        data = null
                    });
                }

                _logger.LogInformation($"Fetched account with id {id}.");
                return Ok(new APIResponse<AccountResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = account
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching account: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequestDto newAccountDto)
        {
            try
            {



                var newAccount = new Account
                {
                    account_name = newAccountDto.account_name,
                    address1 = newAccountDto.address1,
                    address2 = newAccountDto.address2,
                    city = newAccountDto.city,
                    state = newAccountDto.state,
                    zip_code = newAccountDto.zip_code,
                    country = newAccountDto.country,
                    primary_contact_name = newAccountDto.primary_contact_name,
                    primary_contact_number = newAccountDto.primary_contact_number,
                    email = newAccountDto.email,
                    specialization = newAccountDto.specialization,
                    description = newAccountDto.description,
                    is_deleted = false,
                    created_at = DateTime.UtcNow,
                    created_by = newAccountDto.created_by,                  
                };

                _dbContext.accounts.Add(newAccount);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Account {newAccount.account_name} created successfully.");

                return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.id }, new APIResponse<AccountResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status201Created,
                    errorMessage = null,
                    data = new AccountResponseDto
                    {
                        id = newAccount.id,
                        account_name = newAccount.account_name,
                        address1 = newAccount.address1,
                        address2 = newAccount.address2,
                        city = newAccount.city,
                        state = newAccount.state,
                        zip_code = newAccount.zip_code,
                        country = newAccount.country,
                        primary_contact_name = newAccount.primary_contact_name,
                        primary_contact_number = newAccount.primary_contact_number,
                        email = newAccount.email,
                        specialization = newAccount.specialization,
                        description = newAccount.description,
                        is_deleted = newAccount.is_deleted,
                        created_at = newAccount.created_at,
                        created_by = newAccount.created_by,
                        updated_at = newAccount.updated_at,
                        updated_by = newAccount.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                // Extract MySQL-specific error details
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database update failed: {ErrorMessage}", innerException.Message);

                    return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<AccountResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    });
                }

                _logger.LogError(dbEx, "An error occurred while creating a new clinic.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected database error occurred. Please try again later.",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new clinic.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected error occurred. Please try again later.",
                    data = null
                });
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountUpdateDto updatedAccountDto)
        {
            try
            {
                var account = await _dbContext.accounts.FindAsync(id);
                if (account == null)
                {
                    _logger.LogError($"Account with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Account with id {id} not found.",
                        data = null
                    });
                }

                account.account_name = updatedAccountDto.account_name;
                account.address1 = updatedAccountDto.address1;
                account.address2 = updatedAccountDto.address2;
                account.city = updatedAccountDto.city;
                account.state = updatedAccountDto.state;
                account.zip_code = updatedAccountDto.zip_code;
                account.country = updatedAccountDto.country;
                account.primary_contact_name = updatedAccountDto.primary_contact_name;
                account.primary_contact_number = updatedAccountDto.primary_contact_number;
                account.email = updatedAccountDto.email;
                account.specialization = updatedAccountDto.specialization;
                account.description = updatedAccountDto.description;
                //account.is_deleted = updatedAccountDto.is_deleted;
                account.updated_by = updatedAccountDto.updated_by;                
                account.updated_at = DateTime.UtcNow;
                


                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Account {id} updated successfully.");
                return Ok(new APIResponse<AccountResponseDto>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = new AccountResponseDto
                    {
                        id = account.id,
                        account_name = account.account_name,
                        address1 = account.address1,
                        address2 = account.address2,
                        city = account.city,
                        state = account.state,
                        zip_code = account.zip_code,
                        country = account.country,
                        primary_contact_name = account.primary_contact_name,
                        primary_contact_number = account.primary_contact_number,
                        email = account.email,
                        specialization = account.specialization,
                        description = account.description,
                        is_deleted = account.is_deleted,
                        created_at = account.created_at,
                        created_by = account.created_by,
                        updated_at = account.updated_at,
                        updated_by = account.updated_by
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                // Extract MySQL-specific error details
                var innerException = dbEx.InnerException as MySqlConnector.MySqlException;
                if (innerException != null)
                {
                    _logger.LogError("Database update failed: {ErrorMessage}", innerException.Message);

                    return StatusCode(StatusCodes.Status400BadRequest, new APIResponse<AccountResponseDto>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status400BadRequest,
                        errorMessage = $"Database error: {innerException.Message}",
                        data = null
                    });
                }

                _logger.LogError(dbEx, "An error occurred while creating a new clinic.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
                {
                    isError = true,
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = "An unexpected database error occurred. Please try again later.",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new clinic.");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<AccountResponseDto>
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
                var account = await _dbContext.accounts.FirstOrDefaultAsync(u => u.id == id && u.is_deleted == false);
                if (account == null)
                {
                    _logger.LogError($"Account with id {id} not found.");
                    return NotFound(new APIResponse<string>
                    {
                        isError = true,
                        statusCode = StatusCodes.Status404NotFound,
                        errorMessage = $"Account with id {id} not found.",
                        data = null
                    });
                }

                account.is_deleted = true;
                account.updated_at = DateTime.UtcNow;
              
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Account {id} deleted successfully.");
                return Ok(new APIResponse<string>
                {
                    isError = false,
                    statusCode = StatusCodes.Status200OK,
                    errorMessage = null,
                    data = "Account deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting account: {ex.Message}");
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
