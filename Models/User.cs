using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalWebApplication.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? Organization { get; set; }

        [Required]
        public int Role { get; set; }//1 SuperAdmin //2 Admin //3 Study Member //4 Auditor  // 5 Clinical Research Associate (CRA)

        [Required]
        public int CreatedBy { get; set; } //admin Id// superadmin Id

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; } //for specific organization
        public virtual List<Organizations> Organizations { get; set; }
        public int IsActive { get; internal set; }
        public int Status { get;  set; }

        public string? StudyIds { get; set; } //optional// for study member // would be saved after searlized
        public DateTime CreatedAt { get;  set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime DeletedAt { get; set; }


    }
}