namespace ClinicalWebApplication.Models
{
    public class Monitoringvisit
    {
        public int Id { get; set; }

        public DateTime? VisitDate { get; set; }
        public string? VisitType { get; set; }
        public string? MonitoringVisitTitle { get; set; }
        public string? Status { get; set; }

        public int? OrganizationId { get; set; }
        public int? InvestiGatorSiteStaffId { get; set; }
        public int? InvestiGatorSitefId { get; set; }
        public int? StudyId { get; set; }


        public int IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? Createdby { get; set; }


    }
}
