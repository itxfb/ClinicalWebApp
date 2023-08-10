using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalWebApplication.Models
{
    public class InvestigatorSites
    {

        public int Id { get; set; }
        public string? Facility_Institution_Name { get; set; }
        public double? Site_No { get; set; }  //number
        public string? Address { get; set; }
        
        public string? Clinical_Trial_Agreements { get; set; }  //documents upload path
        public Nullable<DateTime> Qualification_Date { get; set; }
        public double? Recruitment_Target { get; set; }  //number
        public int? Monitoring_Frequency { get; set; } //number
        public int CreatedBy { get; set; }
        public int StudyId { get; set; }

        public Nullable<int> OrganizationId { get; set; }
        public Study Study { get; set; }
        public List<ProtocolDeviations> ProtocolDeviations { get; set; }
        public List<StaffInvestigatorSite> StaffInvestigatorSites { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        [NotMapped]
        public IFormFile Clinical_Trial { get; set; }

    }
}
