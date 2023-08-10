namespace ClinicalWebApplication.Models
{
    public class FindingsDataDto
    {
        public int Id { get; set; }

        public string? Significance { get; set; }
        public string? Subject { get; set; }
        public DateTime? DateOccure { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? ActionResolution { get; set; }
        public string? Question { get; set; }

        public string? MonitoringVisit { get; set; }
        public string? QuestionStatus { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? ReviewDate { get; set; }
        public string? FindingStatus { get; set; }
        public string? ReviewComments { get; set; }

        public int? StudyId { get; set; }
        public int? OrganizationId { get; set; }
        public int? InvestiGatorSitefId { get; set; }

        public int? MonitoringVisitId { get; set; }


        public string? Createdby { get; set; }



    }
}
