namespace TrudoseAdminPortalAPI.Dto
{
    public class ClinicResponseDto
    {
        public int id { get; set; }
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
        public bool is_deleted { get; set; }
        public string email { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

}
