namespace TrudoseAdminPortalAPI.Dtos
{
    public class ChangePasswordWithOtpDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
