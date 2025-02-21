namespace TrudoseAdminPortalAPI.Dto
{
    public class SurveyQuestionMapResponseDto
    {
        public int id { get; set; }
        public int symptom_id { get; set; }
        public string symptom_name { get; set; }
        public int question_id { get; set; }

        public string question_name { get; set; }
        public int survey_id { get; set; }
        public string survey_name { get; set; }
        public int order_no { get; set; }
    }

    public class SurveyQuestionMapDeleteResponseDto
    {
        public int id { get; set; }
        public int symptom_id { get; set; }
       
        public int question_id { get; set; }
      
        public int survey_id { get; set; }
      
        public int order_no { get; set; }
    }
}
