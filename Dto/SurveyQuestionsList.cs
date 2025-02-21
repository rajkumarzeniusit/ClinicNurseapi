namespace TrudoseAdminPortalAPI.Dto
{
    //public class SurveyQuestionSymptomDto
    //{
    //    public int SurveyId { get; set; }
    //    public string SurveyName { get; set; }
    //    public int QuestionId { get; set; }
    //    public string QuestionName { get; set; }
    //    public string QuestionType { get; set; }
    //    public string Choices { get; set; }
    //    public string SymptomIds { get; set; }
    //    public string SymptomNames { get; set; }
    //}

    public class QuestionDto
    {
        public int question_id { get; set; }
        public string question_name { get; set; }
        public string question_type { get; set; }
        public string question_choices { get; set; }
        public int order_no { get; set; }
    }

    public class SurveyResponseDto
    {
        public int id { get; set; }
        public string survey_name { get; set; }
        public List<QuestionDto> questions { get; set; } = new List<QuestionDto>();
    }
}
