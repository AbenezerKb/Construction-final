using ERP.Models;
using ERP.Models.Others;
using Microsoft.EntityFrameworkCore;

namespace ERP.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Materials> Materials { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<IssueItem> IssueItems { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<Buy> Buys { get; set; }

        public DbSet<BuyItem> BuyItems { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        public DbSet<BulkPurchase> BulkPurchases { get; set; }

        public DbSet<BulkPurchaseItem> BulkPurchaseItems { get; set; }

        public DbSet<PurchaseItemEmployee> PurchaseItemEmployees { get; set; }

        public DbSet<Receive> Receives { get; set; }

        public DbSet<ReceiveItem> ReceiveItems { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<TransferItem> TransferItems { get; set; }

        public DbSet<TransferItemEquipmentAsset> TransferItemEquipmentAssets { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<MaterialSiteQty> MaterialSiteQties { get; set; }

        public DbSet<EquipmentSiteQty> EquipmentSiteQties { get; set; }

        public DbSet<EquipmentCategory> EquipmentCategories { get; set; }

        public DbSet<EquipmentModel> EquipmentModels { get; set; }

        public DbSet<EquipmentAsset> EquipmentAssets { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Return> Returns { get; set; }

        public DbSet<BorrowItem> BorrowItems { get; set; }

        public DbSet<BorrowItemEquipmentAsset> BorrowItemEquipmentAssets { get; set; }

        public DbSet<AssetDamage> AssetDamages { get; set; }

        public DbSet<AssetNumberId> AssetNumberIds { get; set; }

        public DbSet<Miscellaneous> Miscellaneouses { get; set; }

        #region TaskManagement DbSets
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<WeeklyPlan> WeeklyPlans { get; set; }
        public DbSet<WeeklyResult> WeeklyResults { get; set; }
        public DbSet<WeeklyResultValue> WeeklyResultValues { get; set; }
        public DbSet<PerformanceSheet> PerformanceSheets { get; set; }
        public DbSet<WeeklyPlanValue> WeeklyPlanValues { get; set; }
        public DbSet<Setting> Settings { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Set Composite keys
            modelBuilder.Entity<MaterialSiteQty>()
                .HasKey(o => new { o.ItemId, o.SiteId });

            modelBuilder.Entity<EquipmentSiteQty>()
                .HasKey(o => new { o.EquipmentModelId, o.SiteId });

            modelBuilder.Entity<TransferItem>()
                .HasKey(o => new { o.TransferId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<TransferItemEquipmentAsset>()
                .HasKey(o => new { o.TransferId, o.ItemId, o.EquipmentModelId, o.EquipmentAssetId });

            modelBuilder.Entity<BorrowItem>()
                .HasKey(o => new { o.BorrowId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<BorrowItemEquipmentAsset>()
                .HasKey(o => new { o.BorrowId, o.ItemId, o.EquipmentModelId, o.EquipmentAssetId });

            modelBuilder.Entity<IssueItem>()
                .HasKey(o => new { o.IssueId, o.ItemId });

            modelBuilder.Entity<PurchaseItem>()
                .HasKey(o => new { o.PurchaseId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<BulkPurchaseItem>()
                .HasKey(o => new { o.BulkPurchaseId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<PurchaseItemEmployee>()
                .HasKey(o => new { o.PurchaseId, o.ItemId, o.EquipmentModelId, o.RequestedById });

            modelBuilder.Entity<ReceiveItem>()
                .HasKey(o => new { o.ReceiveId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<BuyItem>()
                .HasKey(o => new { o.BuyId, o.ItemId });

            //Transfer
            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceiveSite)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.SendSite)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceivedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            /*

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.DeliveredBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            */

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.SentBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            //Maintenance
            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.FixedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.EquipmentAsset)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.EquipmentModel)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //Issue
            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.HandedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            //Borrow
            modelBuilder.Entity<Borrow>()
                    .HasOne(i => i.HandedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Borrow>()
                    .HasOne(i => i.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                    .HasOne(i => i.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            //Purchase
            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.CheckedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.ReceivingSite)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            //BulkPurchase
            modelBuilder.Entity<BulkPurchase>()
                  .HasOne(i => i.RequestedBy)
                  .WithMany()
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BulkPurchase>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            //Receive
            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.Purchase)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.DeliveredBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ReceivedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ReceivingSite)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            //Buy
            modelBuilder.Entity<Buy>()
                   .HasOne(i => i.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Buy>()
                   .HasOne(i => i.CheckedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Buy>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Buy>()
                   .HasOne(i => i.Purchase)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Buy>()
                   .HasOne(i => i.BuySite)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            //AssetNumberIds
            modelBuilder.Entity<AssetNumberId>()
                .HasOne(a => a.Item)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            //TransferItem-EquipmentAsset
            modelBuilder.Entity<TransferItemEquipmentAsset>()
                .HasOne(o => o.TransferItem)
                .WithMany(o => o.TransferEquipmentAssets)
                .OnDelete(DeleteBehavior.Restrict);

            //BorrowItem-EquipmentAsset
            modelBuilder.Entity<BorrowItemEquipmentAsset>()
                .HasOne(o => o.BorrowItem)
                .WithMany(o => o.BorrowEquipmentAssets)
                .OnDelete(DeleteBehavior.Restrict);

            #region TaskManagement Config 
            modelBuilder.Entity<Project>()
            .Property(p => p.Status)
            .HasConversion(v => v.ToString(),
            v => (Models.Others.Status)Enum.Parse(typeof(Models.Others.Status), v));

            // modelBuilder.Entity<WeeklyResult>()
            // .Property(wr => wr.Staus)
            // .HasConversion(v => v.ToString(),
            // v => (Models.Others.Status)Enum.Parse(typeof(Models.Others.Status), v));

            modelBuilder
            .Entity<ProjectTask>()
            .HasOne<Project>(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

            modelBuilder
            .Entity<SubTask>()
            .HasOne(sub => sub.ProjectTask)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(sub => sub.TaskId);

            modelBuilder.Entity<ProjectTask>().Ignore(p => p.Progress);

            // modelBuilder.Entity<WeeklyPlan>().HasKey(wp => new { wp.WeekNo, wp.Year });
            // modelBuilder.Entity<WeeklyResult>().HasKey(wr => new { wr.WeekNo, wr.Year });


            modelBuilder.Entity<WeeklyPlan>()
            .HasMany(wp => wp.PlanValues)
            .WithOne(wpv => wpv.WeeklyPlan)
            .HasForeignKey(w => w.WeeklyPlanId);

            modelBuilder.Entity<WeeklyPlan>()
            .HasOne(wp => wp.WeeklyResult)
            .WithOne(wr => wr.WeeklyPlan)
            .HasForeignKey<WeeklyResult>(wr => wr.WeeklyPlanId);


            modelBuilder
                .Entity<WeeklyPlan>()
                .HasOne(wp => wp.Project)
                .WithMany()
                .HasForeignKey(wp => wp.ProjectId);

            modelBuilder
                        .Entity<WeeklyResultValue>()
                        .HasOne(wrv => wrv.WeeklyResult)
                        .WithMany(wr => wr.Results)
                        .HasForeignKey(wrv => wrv.WeeklyResultId);

            modelBuilder.Entity<WeeklyResultValue>()
                      .HasOne(wrv => wrv.SubTask)
                      .WithMany(s => s.WeeklyResultValues)
                      .HasForeignKey(wrv => wrv.SubTaskId);

            modelBuilder.Entity<PerformanceSheet>()
                        .HasOne(ps => ps.Project)
                        .WithMany()
                        .HasForeignKey(ps => ps.ProjectId);


            //Seeding Data

            modelBuilder.Entity<Setting>().HasData(new Setting
            {
                Id = 1,
                Name = "DeadlineNotificationDay",
                Description = "Before how many days should a deadline notification be sent",
                Value = "10"
            });
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                RoleId = 1,
                Role = "Admin",
                CanEditUser = true,

                CanRequestPurchase = true,

                CanApprovePurchase = true,

                CanCheckPurchase = true,


                CanViewPurchase = true,

                CanConfirmPurchase = true,


                CanViewBulkPurchase = true,

                CanRequestBulkPurchase = true,

                CanApproveBulkPurchase = true,

                CanConfirmBulkPurchase = true,

                CanRequestBuy = true,

                CanApproveBuy = true,

                CanCheckBuy = true,

                CanViewBuy = true,

                CanConfirmBuy = true,

                CanReceive = true,

                CanApproveReceive = true,


                CanViewReceive = true,

                CanRequestIssue = true,

                CanApproveIssue = true,


                CanHandIssue = true,


                CanViewIssue = true,


                CanRequestBorrow = true,

                CanApproveBorrow = true,


                CanHandBorrow = true,


                CanViewBorrow = true,

                CanReturnBorrow = true,

                CanRequestTransfer = true,

                CanApproveTransfer = true,

                CanSendTransfer = true,


                CanReceiveTransfer = true,

                CanViewTransfer = true,

                CanRequestMaintenance = true,

                CanApproveMaintenance = true,


                CanFixMaintenance = true,


                CanViewMaintenance = true,

                CanGetStockNotification = true


            });
            Console.WriteLine("Data Seeded");



            #endregion
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var added = ChangeTracker.Entries<IAuditableEntity>().Where(E => E.State == EntityState.Added).ToList();
            var now = DateTime.Now;
            added.ForEach(E =>
            {
                E.Property(x => x.CreatedAt).CurrentValue = now;
                E.Property(x => x.UpdatedAt).CurrentValue = now;

            });

            var modified = ChangeTracker.Entries<IAuditableEntity>().Where(E => E.State == EntityState.Modified).ToList();

            modified.ForEach(E =>
            {
                E.Property(x => x.UpdatedAt).CurrentValue = now;

            });

            return base.SaveChangesAsync();
        }
    }
}
