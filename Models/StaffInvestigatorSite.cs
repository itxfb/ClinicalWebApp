namespace ClinicalWebApplication.Models
{
    public class StaffInvestigatorSite
    {

        //Staff Category Div
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string? Full_Name { get; set; }
        public string? Email { get; set; }
        public int Role { get; set; }   /*(drop-down) - Principal Investigator, Sub-Investigator , Study Coordinator, Clinical Rater*/
        public int InvestigatorSiteId { get; set; }
        public InvestigatorSites InvestigatorSite { get; set; }



    }
}
