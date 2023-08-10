namespace ClinicalWebApplication.Models
{
    public class GeneralFindings
    {
        public int Id { get; set; }

        
        public string? Findings { get; set; }

        public string? Question { get; set; }

        public string? QuestionStatus { get; set; }

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
