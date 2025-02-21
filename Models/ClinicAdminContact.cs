using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class ClinicAdminContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string admin_email { get; set; }
        public string admin_contact_name { get; set; }
        public string admin_contact_number { get; set; }

        [ForeignKey("Clinic")]
        public int clinic_id { get; set; }
        public Clinic Clinic { get; set; }

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
