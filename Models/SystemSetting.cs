using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrudoseAdminPortalAPI.Model
{
    public class SystemSetting
    {
        [Key]
        public int id { get; set; }
        public string? config_name { get; set; }

        
        public string? config_value { get; set; }
    }
}
