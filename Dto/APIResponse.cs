namespace TrudoseAdminPortalAPI.Dto
{
    public class APIResponse<T>
    {
        public bool isError { get; set; }
        public int statusCode { get; set; }
        public string? errorMessage { get; set; }
        public T? data { get; set; } 
    }


    public class SuccessResponseDto
    {
        public string message { get; set; }
    }

    public class TokenResponse
    {
        public int userId { get; set; }
        public bool isForgotPassword { get; set; }
        public string? message { get; set; }
        public string? status { get; set; }
        public int roleId { get; set; }
    }
}

