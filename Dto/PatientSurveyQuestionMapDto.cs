using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Dto
{
    public class PatientSurveyQuestionMapDto
    {

        public int symptom_id { get; set; }
        public int question_id { get; set; }
        public int survey_id { get; set; }
        public int order_no { get; set; }


    }
}
