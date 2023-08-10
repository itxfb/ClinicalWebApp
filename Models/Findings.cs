namespace ClinicalWebApplication.Models
{
    public class Findings
    {
        public int Id { get; set; }

        public DateTime? DateOccure { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? ActionResolution { get; set; }
        public string? Question { get; set; }

        public int?  MonitoringVisitId { get; set; }
        public string?  QuestionStatus { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? Createdby { get; set; }

    }
}
