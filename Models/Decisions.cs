using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalWebApplication.Models
{
    public class Decisions
    {

        public int Id { get; set; }

        public string? Description { get; set; }
        public Nullable<DateTime> Decisions_Date { get; set; }

        public string? Study { get; set; }
        public string? Risk_Impact { get; set; }
        public string? Risk_Description { get; set; }

        public Nullable<int> StudyId { get; set; }

        public Nullable<int> OrganizationId { get; set; }

        public int IsActive { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }


        [NotMapped]
        public IFormFile Decision_Attachment { get; set; }
        public string? DecisionAttachment_path { get; set; }  //documents upload path




    }
}
