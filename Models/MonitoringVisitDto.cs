namespace ClinicalWebApplication.Models
{
    public class MonitoringVisitDto
    {
        public int Id { get; set; }

        public DateTime? VisitDate { get; set; }
        public string? VisitType { get; set; }
        public string? MonitoringVisitTitle { get; set; }
        public string? Status { get; set; }

        public string? Organization{ get; set; }
        public string? InvestiGatorSiteStaffId { get; set; }
        public string? InvestiGatorSitefId { get; set; }
        public string? Study { get; set; }


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? Createdby { get; set; }


    }
}
