using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Dto
{
    public class AccountRequestDto
    {
        [Required]
        public string account_name { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }

        public string primary_contact_name { get; set; }
        public string primary_contact_number { get; set; }

        [Required, EmailAddress]
        public string email { get; set; }

        public string specialization { get; set; }
        public string description { get; set; }
        //public bool is_deleted { get; set; } = false;
        public int? created_by { get; set; }
       
        public DateTime created_at { get; set; }
       
    }
}
