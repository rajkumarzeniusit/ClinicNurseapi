using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class FirmwareUpdate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string firmware_update_name { get; set; }
        public string firmware_update_path { get; set; }
        public string firmware_update_version { get; set; }
        public string device_type { get; set; }
        public string device_applicable_version { get; set; }
        public string description { get; set; }
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
