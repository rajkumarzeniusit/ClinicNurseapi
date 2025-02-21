using TrudoseAdminPortalAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrudoseAdminPortalAPI.Dto
{
    public class ClinicBillingContactRequestDto
    {
        public string billing_email { get; set; }
        public string billing_contact_name { get; set; }
        public string billing_contact_number { get; set; }
      
        public int clinic_id { get; set; }
        public int? created_by { get; set; }
       
        public DateTime created_at { get; set; } 
      
    }

    public class ClinicBillingContactResponseDto
    {
        public int id { get; set; }
        public string billing_email { get; set; }
        public string billing_contact_name { get; set; }
        public string billing_contact_number { get; set; }

        public int clinic_id { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class ClinicBillingContactUpdateDto
    {
        public string billing_email { get; set; }
        public string billing_contact_name { get; set; }
        public string billing_contact_number { get; set; }

        public int clinic_id { get; set; }        
        public int? updated_by { get; set; }
       
        public DateTime updated_at { get; set; }
    }

}
