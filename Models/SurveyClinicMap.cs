using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrudoseAdminPortalAPI.Models
{
    public class SurveyClinicMap
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int survey_id { get; set; }
        [Required]
        public int clinic_id { get; set; }

        [ForeignKey("survey_id")]
        public SurveyMaster surveymaster { get; set; }

        [ForeignKey("clinic_id")]
        public Clinic Clinic { get; set; }
    }
}
