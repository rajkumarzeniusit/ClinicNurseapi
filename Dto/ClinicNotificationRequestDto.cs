namespace TrudoseAdminPortalAPI.Dto
{
    public class ClinicNotificationRequestDto
    {
        public DateTime notification_datetime { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int? clinic_id { get; set; }
        public DateTime created_at { get; set; }
        public int? created_by { get; set; }
    
    }

    public class ClinicNotificationResponseDto
    {
        public int id { get; set; }
        public DateTime notification_datetime { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int? clinic_id { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created_at { get; set; }
        public int? created_by { get; set; }
        public DateTime updated_at { get; set; }
        public int? updated_by { get; set; }
    }

    public class ClinicNotificationUpdateDto
    {
        public DateTime notification_datetime { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int? clinic_id { get; set; }
       
        public DateTime updated_at { get; set; }
        public int? updated_by { get; set; }
    }
}
