using Microsoft.EntityFrameworkCore;

namespace ClinicalWebApplication.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Study> Study { get; set; }
        public DbSet<InvestigatorSites> InvestigatorSites { get; set; }
        public DbSet<StaffInvestigatorSite> StaffInvestigatorSites { get; set; }
        public DbSet<ProtocolDeviations> ProtocolDeviations { get; set; }
        public DbSet<Actions> Actions { get; set; }
        public DbSet<Decisions> Decisions { get; set; }
        public DbSet<Informativs> Informativs { get; set; }
        public DbSet<Monitoringvisit> Monitoringvisits { get; set; }
        public DbSet<Attendies> Attendies { get; set; }
        public DbSet<Findings> Findings { get; set; }
        public DbSet<GeneralFindings> GeneralFindings { get; set; }
 

      

        //Relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Organizations>()
           .HasOne<User>(e => e.Creater)
           .WithMany(d => d.Organizations)
           .HasForeignKey(e => e.CreatedBy)
           .IsRequired(true); 
            
            modelBuilder.Entity<Study>()
           .HasOne<Organizations>(e => e.Organization)
           .WithMany(d => d.Studies)
           .HasForeignKey(e => e.OrganizationId)
           .IsRequired(true);

            modelBuilder.Entity<InvestigatorSites>()
           .HasOne<Study>(e => e.Study)
           .WithMany(d => d.InvestigatorSites)
           .HasForeignKey(e => e.StudyId)
           .IsRequired(true);

            modelBuilder.Entity<StaffInvestigatorSite>()
          .HasOne<InvestigatorSites>(e => e.InvestigatorSite)
          .WithMany(d => d.StaffInvestigatorSites)
          .HasForeignKey(e => e.InvestigatorSiteId)
          .IsRequired(false);

            modelBuilder.Entity<ProtocolDeviations>()
            .HasOne<InvestigatorSites>(e => e.InvestigatorSite)
            .WithMany(d => d.ProtocolDeviations)
            .HasForeignKey(e => e.InvestigatorSiteId)
            .IsRequired(true);


            //Junction Table Creating
            //  modelBuilder.Entity<Tags>()
            //  .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            //  .Map(t => t.MapLeftKey("CourseID")
            //.MapRightKey("InstructorID")
            //.ToTable("CourseInstructor"));
        }
    }
}
