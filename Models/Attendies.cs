namespace ClinicalWebApplication.Models
{
    public class Attendies
    {


        public int Id { get; set; }

        public  int? InvestiGatorSiteStaffId { get; set; }
        public  int? MonitoringVisitId { get; set; }
        
        public int IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? Createdby { get; set; }


    }
}
