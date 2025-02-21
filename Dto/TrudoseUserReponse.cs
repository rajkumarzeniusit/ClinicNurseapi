namespace TrudoseAdminPortalAPI.Dtos
{
    public class TrudoseUserReponse
    {
        public int id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
        public string? password_hash { get; set; } 
        public string? phone_number { get; set; }
        public int role_id { get; set; }
        public int clinic_id { get; set; }
        public bool is_deleted { get; set; } 
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public string? status { get; set; }
        public DateTime created_at { get; set; } 

        public DateTime updated_at { get; set; } 


    }
}
