using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrudoseAdminPortalAPI.Models
{
    public class TrudoseUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string? firstname { get; set; }

        [Required]
        [StringLength(100)]
        public string? lastname { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string? email { get; set; }

        [Required]
        [StringLength(255)]
        public string? password_hash { get; set; }


        [Required]
        [StringLength(20)]
        public string? phone_number { get; set; }
        [Required]
        [StringLength(10)]
        public string? status { get; set; }

        [Required]

        public int role_id { get; set; }

        public bool is_deleted { get; set; } = false;

        [ForeignKey("created_by")]
        public int? created_by { get; set; }

        [ForeignKey("updated_by")]
        public int? updated_by { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;


        [ForeignKey("role_id")]
        public Role? Role { get; set; }

        [ForeignKey("created_by")]
        public TrudoseUsers? CreatedByUser { get; set; }

        [ForeignKey("updated_by")]
        public TrudoseUsers? UpdatedByUser { get; set; }
    }
}
