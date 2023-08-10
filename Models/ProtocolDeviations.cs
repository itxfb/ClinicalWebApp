using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalWebApplication.Models
{
    public class ProtocolDeviations
    {
        public int Id { get; set; }
        public string? Primary_CRA { get; set; }
        public double? Subject { get; set; }
        public string? Subject_Status { get; set; } //dropdown

        public string? protocol_type { get; set; }//dropdown

        public string? Description { get; set; } //text-area

        public string? Action_Resolution { get; set; } //text-area

        public string? Subject_Visit { get; set; } //text field

        public string? Significance { get; set; } //dropdown

        public int CreatedBy { get; set; }

        public Nullable<DateTime> Sponsor_Notified { get; set; }

        //public int? Reportable_to_Ethics { get; set; }/*(Yes or No)*/ //dropdown

        //public int? Ethics_Notified { get; set; } /*(Yes or No)*/ //dropdown

        //public int? Reported_by_Investigator_Site { get; set; } //yes or no //dropdown

        public string? Reportable_to_Ethics { get; set; }/*(Yes or No)*/ //dropdown

        public string? Ethics_Notified { get; set; } /*(Yes or No)*/ //dropdown
        public Nullable<DateTime> Ethics_Notified_Date { get; set; } /*(date)*/ //

        public Nullable<int> OrganizationId { get; set; }
        public string? Reported_by_Investigator_Site { get; set; } //yes or no //dropdown

        public Nullable<int> StudyId { get; set; }
        public int InvestigatorSiteId { get; set; }
        public InvestigatorSites InvestigatorSite { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }


    }
}
