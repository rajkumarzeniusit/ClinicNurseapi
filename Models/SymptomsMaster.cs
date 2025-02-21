using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class SymptomsMaster
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public String symptom_name { get; set; }
        [Required]
        public String symptom_description { get; set; }
   
    }
}
