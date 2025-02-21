using System.ComponentModel.DataAnnotations.Schema;

namespace TrudoseAdminPortalAPI.Dtos
{
    public class UpdateClinicUserProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? created_by { get; set; }

        public DateTime created_at { get; set; } 

        public DateTime updated_at { get; set; } 
        public int? updated_by { get; set; }
        
    }
}
