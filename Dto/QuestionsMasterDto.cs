using TrudoseAdminPortalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Dto
{
    public class QuestionsMasterDto
    {
        public string question_name { get; set; } = string.Empty;

 
        public String question_type { get; set; }

        public List<string> question_choices { get; set; } // JSON string for multiple choices
    }
}
