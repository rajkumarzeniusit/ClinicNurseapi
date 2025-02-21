using System.ComponentModel.DataAnnotations.Schema;
using TrudoseAdminPortalAPI.Models;


namespace TruDoseAPI.Models
{
    public class Account
    {

        public int id { get; set; }
        public string account_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string primary_contact_name { get; set; }
        public string primary_contact_number { get; set; }
        public string email { get; set; }
        public string specialization { get; set; }
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
