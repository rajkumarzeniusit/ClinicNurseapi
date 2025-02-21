namespace TrudoseAdminPortalAPI.Dto
{
    public class DeviceRequestDto
    {
        public string device_id { get; set; }
        public string device_type { get; set; }
        public int? account_id { get; set; }
        public int? clinic_id { get; set; }
        public string description { get; set; }
        public string device_status { get; set; } = "Active";
        
        public int? created_by { get; set; }
        public DateTime created_at { get; set; } 
        
    }


    public class DeviceResponseDto
    {
        public int id { get; set; }
        public string device_id { get; set; }
        public string device_type { get; set; }
        public int? account_id { get; set; }
        public int? clinic_id { get; set; }
        public string description { get; set; }
        public string device_status { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created_at { get; set; }
        public int? created_by { get; set; }
        public DateTime updated_at { get; set; }
        public int? updated_by { get; set; }
    }


    public class DeviceUpdateDto
    {
        public string device_id { get; set; }
        public string device_type { get; set; }
        public int? account_id { get; set; }
        public int? clinic_id { get; set; }
        public string description { get; set; }
        public string device_status { get; set; } = "Active";
       
        public int? updated_by { get; set; }
    
        public DateTime updated_at { get; set; }
    }

}
