using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TruDoseAPI.Models;
using System.Security.Principal;

namespace TrudoseAdminPortalAPI.Models
{
    public class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("Account")]
        public int account_id { get; set; }
        public string clinic_name { get; set; }
        public string clinic_type { get; set; }
        public string clinic_status { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string primary_contact_name { get; set; }
        public string primary_contact_number { get; set; }
        public string secondary_contact_number { get; set; }
        public string email { get; set; }
        public bool is_deleted { get; set; } = false;
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        public Account Account { get; set; }

        [ForeignKey("created_by")]
        public TrudoseUsers CreatedByUser { get; set; }

        [ForeignKey("updated_by")]
        public TrudoseUsers UpdatedByUser { get; set; }

        //public ICollection<clinic_admin_contact>? ClinicAdminContacts { get; set; }
        //public ICollection<clinic_billing_contact>? ClinicBillingContacts { get; set; }
        //public ICollection<clinic_user>? ClinicUsers { get; set; }
    }

}
