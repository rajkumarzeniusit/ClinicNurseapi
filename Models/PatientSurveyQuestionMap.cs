using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrudoseAdminPortalAPI.Models;

namespace Admin.Models
{
    public class PatientSurveyQuestionMap
    {
        [Key]
        public int id;



        [Required]
        public int symptom_id { get; set; }

        [Required]
        public int question_id { get; set; }

        [Required]
        public int survey_id { get; set; }

        [Required]
        public int order_no { get; set; }



        [ForeignKey("symptom_id")]
        public SymptomsMaster Symptom { get; set; }

        [ForeignKey("question_id")]
        public QuestionsMaster Question { get; set; }

        [ForeignKey("survey_id")]
        public SurveyMaster Survey { get; set; }



    }
}
