namespace ClinicalWebApplication.Models
{
    public class GeneralFindingsDto
    {
        public int Id { get; set; }


        public List<string>? Subject { get; set; }

        public List<string>? Dateoccur { get; set; }
       // public List<DateTime>? Dateoccur { get; set; }

        public List<string>? Status { get; set; }
        public List<string>? Significance { get; set; }
        public List<string>? Category { get; set; }

        public List<string>? Description { get; set; }
        public List<string>? Action_resolution { get; set; }
        public List<string>? Questionstatus { get; set; }
        public List<string>? Question { get; set; }

        public int? MonitoringVisitId { get; set; }
        public int? StudyId { get; set; }
        public int? OrganizationId { get; set; }
        public int? InvestiGatorSitefId { get; set; }

        public string? ReviewDate { get; set; }
        public string? FindingStatus { get; set; }
        public string? ReviewComments { get; set; }

        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? Createdby { get; set; }


    }
}
