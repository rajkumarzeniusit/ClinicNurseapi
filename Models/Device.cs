using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TruDoseAPI.Models;
using System.Security.Principal;

namespace TrudoseAdminPortalAPI.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string device_id { get; set; }
        public string device_type { get; set; }

        [ForeignKey("Account")]
        public int? account_id { get; set; }
        public Account Account { get; set; }

        [ForeignKey("Clinic")]
        public int? clinic_id { get; set; }
        public Clinic Clinic { get; set; }

        public string description { get; set; }
        public string device_status { get; set; } = "Active";
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
