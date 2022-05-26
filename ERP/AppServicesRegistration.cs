
using ERP.Services.ReportServices;
using ERP.Services.AssetNumberServices;
using ERP.Services.BorrowServices;
using ERP.Services.BuyServices;
using ERP.Services.EquipmentCategoryServices;
using ERP.Services.IssueServices;
using ERP.Services.ItemServices;
using ERP.Services.MaintenanceServices;
using ERP.Services.NotificationServices;
using ERP.Services.PurchaseServices;
using ERP.Services.BulkPurchaseServices;
using ERP.Services.ReceiveServices;
using ERP.Services.ReturnServices;
using ERP.Services.SiteServices;
using ERP.Services.TransferServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.EquipmentAssetServices;
using ERP.Services.MiscServices;
using ERP.Services.FileServices;
using ERP.Services.DamageServices;
using ERP.Services.ProjectService;
using ERP.Services.ProjectTaskService;
using ERP.Services.SubTaskService;
using ERP.Services.WeeklyResultService;
using ERP.Services.ProjectManagementReportService;
using ERP.Services.ProjectManagementAnalyticsService;
using ERP.Services.SettingService;
using ERP.Services.BackgroundServices;
using ERP.Services.PerformanceSheetService;
using ERP.Services.WeeklyPlanService;


namespace ERP
{
    public static class AppServiceRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IReturnService, ReturnService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IEquipmentCategoryService, EquipmentCategoryService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAssetNumberService, AssetNumberService>();
            services.AddScoped<IItemSiteQtyService, ItemSiteQtyService>();
            services.AddScoped<IEquipmentAssetService, EquipmentAssetService>();
            services.AddScoped<IGeneralReportService, GeneralReportService>();
            services.AddScoped<IMiscService, MiscService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IBuyService, BuyService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IBulkPurchaseService, BulkPurchaseService>();
            services.AddScoped<IReceiveService, ReceiveService>();
            services.AddScoped<IDamageService, DamageService>();
            #region TaskManagement Services
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectTaskService, ProjectTaskService>();
            services.AddScoped<ISubTaskService, SubTaskService>();
            services.AddScoped<IWeeklyPlanService, WeeklyPlanService>();
            services.AddScoped<IWeeklyResultService, WeeklyResultService>();
            services.AddScoped<IPerformanceSheetService, PerformanceSheetService>();
            services.AddScoped<IProjectManagementReportService, ProjectManagementReportService>();
            services.AddScoped<IProjectManagementAnalyticsService, ProjectManagementAnalyticsService>();
            services.AddScoped<ISettingsService, SettingsService>();

            services.AddHostedService<NotificationBackgroundService>();


            #endregion




            services.AddScoped<Services.IContractRepo, Services.ContractRepo>();
            services.AddScoped<Services.IFileRepo, Services.FileRepo>();
            services.AddScoped<Services.IBIDRepo, Services.BIDRepo>();
            services.AddScoped<Services.ISubContractorRepo, Services.SubContractorRepo>();
            services.AddScoped<Services.ISubContractWorkRepo, Services.SubContractWorkRepo>();
            services.AddScoped<Services.IIncidentRepo, Services.IncidentRepo>();            
            services.AddScoped<Services.IAllocatedBudgetRepo, Services.AllocatedBudgetRepo>();
            services.AddScoped<Services.IAllocatedResourcesRepo, Services.AllocatedResourcesRepo>();
            services.AddScoped<Services.IAssignedWorkForceRepo, Services.AssignedWorkForceRepo>();
            services.AddScoped<Services.ITimeCardRepo, Services.TimeCardRepo>();
            services.AddScoped<Services.IWeeklyRequirementRepo, Services.WeeklyRequirementRepo>();
            services.AddScoped<Services.IGranderRepo, Services.GranderRepo>();
            services.AddScoped<Services.IDailyLaborRepo, Services.DailyLaborRepo>();
            services.AddScoped<Services.ILaborDetailRepo, Services.LaborDetailRepo>();
            services.AddScoped<Services.IClientRepo, Services.ClientRepo>();
            services.AddScoped<Services.IConsultantRepo, Services.ConsultantRepo>();

        }
    }
}