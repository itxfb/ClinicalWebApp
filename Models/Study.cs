namespace ClinicalWebApplication.Models
{
    public class Study
    {

        public int Id { get; set; }

        public string Protocol_Title { get; set; }
     
        public string Condition_Or_Disease { get; set; }

        //List pulled from https://www.nhsinform.scot/illnesses-and-conditions/a-to-z
        public string Intervention_treatment{ get; set; }
        public string Phase { get; set; }
       // Phase 1, Phase 2, Phase 3, Phase 4
        public string Study_Type{ get; set; }
       // Interventional, Observational, Expanded Access
        public string Enrollment { get; set; }
        public string Allocation { get; set; }
        //public string Randomized { get; set; }
        public string Intervention_Model{ get; set; }
        //public string Parallel_Assignement{ get; set; }
        public string Masking { get; set; }
        public string Primary_Purpose { get; set; }
        //public string Treatment { get; set; }
        public DateTime Actual_Study_Start_Date { get; set; }
        public DateTime Estimated_Primary_Completion_Date { get; set; }
        public DateTime Estimated_Study_Completion_Date { get; set; }
        public string Visit_Frequency { get; set; }
        public string NCT_No { get; set; }

        //(number-field) once created creates a URL to: https://clinicaltrials.gov/ct2/show/record/XXXXXXX
        public int CreatedBy { get; set; }
        public int OrganizationId { get; set; }

        public int IsActive { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual List<ProtocolDeviations> ProtocolDeviations { get; set; }
        public virtual List<InvestigatorSites> InvestigatorSites { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
      
    }
}
