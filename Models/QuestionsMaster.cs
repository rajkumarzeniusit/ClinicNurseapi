using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TrudoseAdminPortalAPI.Models
{
    public class QuestionsMaster
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string question_name { get; set; } = string.Empty;

        [Required]
        public String question_type { get; set; }

        [Column(TypeName = "json")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> question_choices { get; set; } // Stored as JSON in DB, handled as a string in C#
    
}

 
}
