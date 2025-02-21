namespace TrudoseAdminPortalAPI.Dtos
{
    public class SetPasswordDto
    {

        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
