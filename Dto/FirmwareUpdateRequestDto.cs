namespace TrudoseAdminPortalAPI.Data
{
    public class FirmwareUpdateRequestDto
    {
        public string firmware_update_name { get; set; }
        public string firmware_update_path { get; set; }
        public string firmware_update_version { get; set; }
        public string device_type { get; set; }
        public string device_applicable_version { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public int? created_by { get; set; }
      
    }


    public class FirmwareUpdateResponseDto
    {
        public int id { get; set; }
        public string firmware_update_name { get; set; }
        public string firmware_update_path { get; set; }
        public string firmware_update_version { get; set; }
        public string device_type { get; set; }
        public string device_applicable_version { get; set; }
        public string description { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created_at { get; set; }
        public int? created_by { get; set; }
        public DateTime updated_at { get; set; }
        public int? updated_by { get; set; }
    }

    public class FirmwareUpdateDto
    {
        public string firmware_update_name { get; set; }
        public string firmware_update_path { get; set; }
        public string firmware_update_version { get; set; }
        public string device_type { get; set; }
        public string device_applicable_version { get; set; }
        public string description { get; set; }
       
        public DateTime updated_at { get; set; }
        public int? updated_by { get; set; }
    }
}
