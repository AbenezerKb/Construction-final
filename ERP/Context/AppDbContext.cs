using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) :base(opt)
        {

        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<SubContractor> SubContractors { get; set; }        
        public DbSet<BID> BIDs { get; set; }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Project> Projects { get; set; }
     //   public DbSet<Site> Sites { get; set; }
        public DbSet<AssignedWorkForce> AssignedWorkForces { get; set; }
        public DbSet<WorkForce> WorkForces { get; set; }        
        public DbSet<AllocatedResources> AllocatedResources { get; set; }
        public DbSet<AllocatedBudget> AllocatedBudgets { get; set; }        
        public DbSet<TimeCard> TimeCards { get; set; }
        public DbSet<SubContractWork> SubContractWorks { get; set; }
        public DbSet<WeeklyRequirement> WeeklyRequirements { get; set; }
        public DbSet<ApprovedWorkList> ApprovedWorkLists { get; set; }

        public DbSet<DefectsCorrectionlist> DefectsCorrectionlists { get; set; }
        
        public DbSet<WeeklyEquipment> WeeklyEquipments { get; set; }

        public DbSet<WeeklyMaterial> WeeklyMaterials { get; set; }
        public DbSet<WeeklyLabor> Labors { get; set; }

        public DbSet<Grander> Granders { get; set; }
        public DbSet<LaborDetail> LaborDetails { get; set; }
        public DbSet<DailyLabor> DailyLabors { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Client> Clients { get; set; }        
        public DbSet<DeclinedWorkList> DeclinedWorkLists { get; set; }
        public DbSet<SubContractingWork> SubcontractingWorks { get; set; }
        public DbSet<WorkForcePlan> WorkForcePlans { get; set; }
        public DbSet<ResourcePlan> ResourcePlans { get; set; }
        public DbSet<SubcontractingPlan> SubcontractingPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /*
            * modelBuilder.Entity<WeeklyRequirement>()
                .HasMany(m => m.material)                               
                .HasForeignKey(f => f.WeeklyRequirementFK)
                .OnDelete(DeleteBehavior.Cascade); ;
            //.WithOne(c => c.WeeklyRequirement)
            modelBuilder.Entity<Labor>()
              .HasOne(c => c.WeeklyRequirement)
              .WithMany(e => e.labor)
              .HasForeignKey(f => f.WeeklyRequirementFK)
              .OnDelete(DeleteBehavior.Cascade); ;
            modelBuilder.Entity<Equipment>()
               .HasOne(c => c.WeeklyRequirement)
               .WithMany(e => e.equipment)
               .HasForeignKey(f => f.WeeklyRequirementFK)
               .OnDelete(DeleteBehavior.Cascade); ;*/

           /*
            modelBuilder.Entity<WorkForcePlan>()
              .HasOne(c => c.Grander)
              .WithMany(e => e.WorkForcePlan)
              .HasForeignKey(f => f.GranderFK)
              .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<ResourcePlan>()
             .HasOne(c => c.Grander)
             .WithMany(e => e.ResourcePlan)
             .HasForeignKey(f => f.GranderFK)
             .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<SubcontractingPlan>()
             .HasOne(c => c.Grander)
             .WithMany(e => e.SubcontractingPlan)
             .HasForeignKey(f => f.GranderFK)
             .OnDelete(DeleteBehavior.Cascade); ;
           */
        }

    }
}


  
