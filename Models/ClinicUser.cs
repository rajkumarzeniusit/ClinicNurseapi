using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StackExchange.Redis;

namespace TrudoseAdminPortalAPI.Models
{
    public class ClinicUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }
        public string phone_number { get; set; }
        public string status { get; set; }

        [ForeignKey("Clinic")]
        public int clinic_id { get; set; }
        public Clinic Clinic { get; set; }

        [ForeignKey("Role")]
        public int role_id { get; set; }
        public Role Role { get; set; }

        public bool is_deleted { get; set; } = false;
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        [ForeignKey("created_by")]
        public TrudoseUsers CreatedByUser { get; set; }

        [ForeignKey("updated_by")]
        public TrudoseUsers UpdatedByUser { get; set; }
    }
}
