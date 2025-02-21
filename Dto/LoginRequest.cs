namespace TrudoseAdminPortalAPI.Dtos
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }        
        public bool IsForgotPassword { get; set; }
    }
}
