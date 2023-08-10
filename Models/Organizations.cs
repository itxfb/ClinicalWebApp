namespace ClinicalWebApplication.Models
{
    public class Organizations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }

        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public virtual List<Study> Studies { get; set; }
        public virtual User Creater { get; set; }


    }
}