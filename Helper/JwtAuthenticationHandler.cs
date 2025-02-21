using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace TrudoseAdminPortalAPI.Helpers
{
    public class JwtAuthenticationHandler
    {
        private readonly ILogger<JwtAuthenticationHandler> _logger;
        private readonly ApplicationDbContext _dbContext;


        public JwtAuthenticationHandler(ILogger<JwtAuthenticationHandler> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task OnMessageReceived(MessageReceivedContext context)
        {
            try
            {
                string token = context.Request.Cookies["access_token"];

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var accessTokenDecrypt = CryptLogic.Decrypt(token);
                        token = OAuthUtil.Unzip(Convert.FromBase64String(accessTokenDecrypt));

                        _logger.LogInformation("Decrypted and unzipped JWT Token successfully.");
                    }
                    catch (Exception decryptionError)
                    {
                        _logger.LogError("Error while decrypting JWT token: {Error}", decryptionError.Message);
                    }
                }
                else
                {
                    _logger.LogError("No Access Token found in cookies.");
                }

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                        if (jsonToken == null)
                        {
                            _logger.LogError("Invalid Access token format.");
                            return;
                        }

                        context.Token = token;
                    }
                    catch (Exception jwtError)
                    {
                        _logger.LogError("Error while parsing JWT Token: {Error}", jwtError.Message);
                    }
                }
                else
                {
                    _logger.LogError("No valid Access Token found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error in JWT authentication: {Error}", ex.Message);
            }
        }

        public async Task OnTokenValidated(TokenValidatedContext context)
        {
            try
            {
                var claims = context.Principal.Claims;
                var roleIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (roleIdClaim != null && int.TryParse(roleIdClaim.Value, out int roleId))
                {
                    string? roleName = await GetRoleNameFromRoleId(roleId);

                    if (!string.IsNullOrEmpty(roleName))
                    {
                        var roleClaim = new Claim(ClaimTypes.Role, roleName);
                        context.Principal.AddIdentity(new ClaimsIdentity(new[] { roleClaim }));
                        _logger.LogInformation($"User authorized as {roleName}.");
                    }
                    else
                    {
                        _logger.LogError($"User with RoleId {roleId} has no associated role.");
                        context.Fail("Unauthorized");
                    }
                }
                else
                {
                    _logger.LogError("RoleId claim not found or invalid.");
                    context.Fail("Unauthorized");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authorization.");
                context.Fail(ex);
            }
        }

        private async Task<string?> GetRoleNameFromRoleId(int roleId)
        {
            var role = await _dbContext.roles
                .Where(r => r.id == roleId)
                .Select(r => r.role_name)
                .FirstOrDefaultAsync();

            return role;
        }

     
    }
}
