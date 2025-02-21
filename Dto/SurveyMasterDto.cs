using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Dto
{
    public class SurveyMasterDto
    {
        public String survey_name { get; set; }
  
        public String survey_description { get; set; }

        public bool is_mandatory { get; set; }
    }
}
