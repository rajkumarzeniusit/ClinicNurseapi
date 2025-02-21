namespace TrudoseAdminPortalAPI.Dtos
{
    public class OTPRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }

        public bool IsForgotPassword { get; set; }
    }
}
