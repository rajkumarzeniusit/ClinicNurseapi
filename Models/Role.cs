using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class Role
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string role_name { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(50)]
        public string application { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;


        public DateTime updated_at { get; set; } = DateTime.Now;
    }
}
