namespace ClinicalWebApplication.Models
{
    public class Informativs
    {
        public int Id { get; set; }

        public string? Informative_Details { get; set; }  //documents upload path
        public Nullable<DateTime> Informative_Date { get; set; }
        public Nullable<DateTime> Targeted_Close_Date { get; set; }
        public Nullable<DateTime> Actual_Close_Date { get; set; }
        public string? Study { get; set; }
        public string Status { get; set; }

        public Nullable<int> StudyId { get; set; }
        public Nullable<int> OrganizationId { get; set; }


        public int IsActive { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

    }
}
