using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Dtos
{
    public class CreateRoleDto
    {

        public string? role_name { get; set; }
       
        public string? description { get; set; }
       
        public string? application { get; set; }
    }
}
