using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class SurveyMaster
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public String survey_name { get; set; }
        [Required]
        public String survey_description { get; set; }

        public bool is_mandatory { get; set; } =false;
    }
}
