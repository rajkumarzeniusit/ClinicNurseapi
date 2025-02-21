namespace TrudoseAdminPortalAPI.Dto
{
    public class ClinicAdminContactRequestDto
    {
        public string admin_email { get; set; }
        public string admin_contact_name { get; set; }
        public string admin_contact_number { get; set; }
        public int clinic_id { get; set; }
        public int? created_by { get; set; }      
        public DateTime created_at { get; set; }
      
    }

    public class ClinicAdminContactResponseDto
    {
        public int id { get; set; }
        public string admin_email { get; set; }
        public string admin_contact_name { get; set; }
        public string admin_contact_number { get; set; }
        public int clinic_id { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }


    public class ClinicAdminContactUpdateDto
    {
        public string admin_email { get; set; }
        public string admin_contact_name { get; set; }
        public string admin_contact_number { get; set; }
        public int clinic_id { get; set; }       
        public int? updated_by { get; set; }       
        public DateTime updated_at { get; set; }
    }
}
