using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Report;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;
        private readonly IGeneralReportService _generalReportService;

        public ReportService(DataContext context, IGeneralReportService generalReportService)
        {
            _context = context;
            _generalReportService = generalReportService;
        }

        public async Task<ReportReturnDTO<Purchase>> GetPurchaseReport(PurchaseReportDTO purchaseDTO)
        {
            purchaseDTO.SetDates(); // parse request data to set what date is given
            purchaseDTO.SetEmployees();

            var purchases = _context.Purchases
                .Where(purchase =>
                //dates
                (purchaseDTO.RequestDateFrom == null || purchase.RequestDate >= purchaseDTO.RequestDateFrom) &&
                (purchaseDTO.RequestDateTo == null || purchase.RequestDate <= purchaseDTO.RequestDateTo) &&
                (purchaseDTO.ApproveDateFrom == null || purchase.ApproveDate >= purchaseDTO.ApproveDateFrom) &&
                (purchaseDTO.ApproveDateTo == null || purchase.ApproveDate <= purchaseDTO.ApproveDateTo) &&
                (purchaseDTO.CheckDateFrom == null || purchase.CheckDate >= purchaseDTO.CheckDateFrom) &&
                (purchaseDTO.CheckDateTo == null || purchase.CheckDate <= purchaseDTO.CheckDateTo) &&
                (purchaseDTO.PurchaseDateFrom == null || purchase.PurchaseDate >= purchaseDTO.PurchaseDateFrom) &&
                (purchaseDTO.PurchaseDateTo == null || purchase.PurchaseDate <= purchaseDTO.PurchaseDateTo) &&
                //sites
                (purchaseDTO.SiteId == -1 || purchase.ReceivingSiteId == purchaseDTO.SiteId) &&
                //status
                (purchaseDTO.Status == -1 || purchase.Status == purchaseDTO.Status) &&
                //single role one user
                (purchaseDTO.EmployeeRole == -1 || purchaseDTO.RequestedById == null || purchase.RequestedById == purchaseDTO.RequestedById) &&
                (purchaseDTO.EmployeeRole == -1 || purchaseDTO.ApprovedById == null || purchase.ApprovedById == purchaseDTO.ApprovedById) &&
                (purchaseDTO.EmployeeRole == -1 || purchaseDTO.CheckedById == null || purchase.CheckedById == purchaseDTO.CheckedById) &&
                (purchaseDTO.EmployeeRole == -1 || purchaseDTO.PurchasedById == null || purchase.RequestedById == purchaseDTO.PurchasedById) &&//here
                //multiple role one user
                (purchaseDTO.EmployeeRole != -1 || purchaseDTO.EmployeeId == -1 || purchase.RequestedById == purchaseDTO.RequestedById || purchase.ApprovedById == purchaseDTO.ApprovedById || purchase.RequestedById == purchaseDTO.PurchasedById/*here*/ || purchase.CheckedById == purchaseDTO.CheckedById) &&
                //item type
                (purchaseDTO.ItemType == -1 || purchase.PurchaseItems.Any(ti => ti.Item.Type == purchaseDTO.ItemType)) &&
                //equipment category
                (purchaseDTO.ItemType != ITEMTYPE.EQUIPMENT || purchaseDTO.EquipmentCategoryId == -1 || purchase.PurchaseItems.Any(ti => ti.Item.Equipment.EquipmentCategoryId == purchaseDTO.EquipmentCategoryId)) &&
                //item
                (purchaseDTO.ItemId == -1 || purchase.PurchaseItems.Any(ti => ti.ItemId == purchaseDTO.ItemId))
                )
                .Include(transfer => transfer.ReceivingSite)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.CheckedBy)
                //here uncomment .Include(transfer => transfer.PurchasedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (purchaseDTO.GroupBy)
            {
                //status
                case PURCHASEREPORTGROUPBY.STATUS:
                    summary = purchases.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case PURCHASEREPORTGROUPBY.REQUESTDATE:
                    summary = purchases.GroupBy(transfer => transfer.RequestDate.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestDate.ToShortDateString() });
                    break;

                case PURCHASEREPORTGROUPBY.APPROVEDATE:
                    summary = purchases.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case PURCHASEREPORTGROUPBY.CHECKDATE:
                    summary = purchases.GroupBy(transfer => transfer.CheckDate == null ? DateTime.MinValue.Date : transfer.CheckDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().CheckDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case PURCHASEREPORTGROUPBY.PURCHASEDATE:
                    summary = purchases.GroupBy(transfer => transfer.PurchaseDate == null ? DateTime.MinValue.Date : transfer.PurchaseDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().PurchaseDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case PURCHASEREPORTGROUPBY.REQUESTWEEK:
                    summary = purchases.GroupBy(transfer => new { Week = transfer.RequestDate.DayOfYear / 7, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"Week {s.Key.Week}, {s.Key.Year}" });
                    break;

                case PURCHASEREPORTGROUPBY.APPROVEWEEK:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case PURCHASEREPORTGROUPBY.CHECKWEEK:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.CheckDate.Value == null ? -1 : transfer.CheckDate.Value.DayOfYear / 7,
                                Year = transfer.CheckDate.Value == null ? -1 : transfer.CheckDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().CheckDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case PURCHASEREPORTGROUPBY.PURCHASEWEEK:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.DayOfYear / 7,
                                Year = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().PurchaseDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case PURCHASEREPORTGROUPBY.REQUESTMONTH:
                    summary = purchases.GroupBy(transfer => new { Month = transfer.RequestDate.Month, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.First().RequestDate.ToString("MMMM")}, {s.Key.Year}" });
                    break;

                case PURCHASEREPORTGROUPBY.APPROVEMONTH:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case PURCHASEREPORTGROUPBY.CHECKMONTH:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.CheckDate.Value == null ? -1 : transfer.CheckDate.Value.Month,
                                Year = transfer.CheckDate.Value == null ? -1 : transfer.CheckDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().CheckDate.Value == null ? "N/A" : $"{s.First().CheckDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case PURCHASEREPORTGROUPBY.PURCHASEMONTH:
                    summary = purchases.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Month,
                                Year = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().PurchaseDate.Value == null ? "N/A" : $"{s.First().PurchaseDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case PURCHASEREPORTGROUPBY.REQUESTYEAR:
                    summary = purchases.GroupBy(transfer => transfer.RequestDate.Year)
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.Key}" });
                    break;

                case PURCHASEREPORTGROUPBY.APPROVEYEAR:
                    summary = purchases.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case PURCHASEREPORTGROUPBY.CHECKYEAR:
                    summary = purchases.GroupBy(transfer =>
                            transfer.CheckDate.Value == null ? -1 : transfer.CheckDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().CheckDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case PURCHASEREPORTGROUPBY.PURCHASEYEAR:
                    summary = purchases.GroupBy(transfer =>
                            transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().PurchaseDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case PURCHASEREPORTGROUPBY.REQUESTEDBY:
                    summary = purchases.GroupBy(transfer => transfer.RequestedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });
                    break;

                case PURCHASEREPORTGROUPBY.APPROVEDBY:
                    summary = purchases.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case PURCHASEREPORTGROUPBY.CHECKEDBY:
                    summary = purchases.GroupBy(transfer => transfer.CheckedById == null ? 0 : transfer.CheckedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().CheckedBy });
                    break;

                case PURCHASEREPORTGROUPBY.PURCHASEDBY:
                    summary = purchases.GroupBy(transfer => transfer.RequestedById == null ? 0 : transfer.RequestedById)//here
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });//here
                    break;

                //site
                case PURCHASEREPORTGROUPBY.SITE:
                    summary = purchases.GroupBy(transfer => transfer.ReceivingSiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceivingSite });
                    break;

            }

            var result = new ReportReturnDTO<Purchase>();

            result.Summary = summary;
            result.Data = await purchases.ToListAsync();

            return result;
        }

        public async Task<ReportReturnDTO<Receive>> GetReceiveReport(ReceiveReportDTO receiveDTO)
        {
            receiveDTO.SetDates(); // parse request data to set what date is given
            receiveDTO.SetEmployees();

            var receives = _context.Receives
                .Where(receive =>
                //dates
                (receiveDTO.PurchaseDateFrom == null || receive.PurchaseDate >= receiveDTO.PurchaseDateFrom) &&
                (receiveDTO.PurchaseDateTo == null || receive.PurchaseDate <= receiveDTO.PurchaseDateTo) &&
                (receiveDTO.ApproveDateFrom == null || receive.ApproveDate >= receiveDTO.ApproveDateFrom) &&
                (receiveDTO.ApproveDateTo == null || receive.ApproveDate <= receiveDTO.ApproveDateTo) &&
                (receiveDTO.ReceiveDateFrom == null || receive.ReceiveDate >= receiveDTO.ReceiveDateFrom) &&
                (receiveDTO.ReceiveDateTo == null || receive.ReceiveDate <= receiveDTO.ReceiveDateTo) &&
                //sites
                (receiveDTO.SiteId == -1 || receive.ReceivingSiteId == receiveDTO.SiteId) &&
                //status
                (receiveDTO.Status == -1 || receive.Status == receiveDTO.Status) &&
                //single role one user
                (receiveDTO.EmployeeRole == -1 || receiveDTO.PurchasedById == null || receive.Purchase.RequestedById == receiveDTO.PurchasedById) && //here
                (receiveDTO.EmployeeRole == -1 || receiveDTO.ApprovedById == null || receive.ApprovedById == receiveDTO.ApprovedById) &&
                (receiveDTO.EmployeeRole == -1 || receiveDTO.ReceivedById == null || receive.ReceivedById == receiveDTO.ReceivedById) &&
                //multiple role one user
                (receiveDTO.EmployeeRole != -1 || receiveDTO.EmployeeId == -1 || receive.Purchase.RequestedById == receiveDTO.PurchasedById /*here*/|| receive.ApprovedById == receiveDTO.ApprovedById || receive.ReceivedById == receiveDTO.ReceivedById) &&
                //item type
                (receiveDTO.ItemType == -1 || receive.ReceiveItems.Any(ri => ri.Item.Type == receiveDTO.ItemType)) &&
                //equipment category
                (receiveDTO.ItemType != ITEMTYPE.EQUIPMENT || receiveDTO.EquipmentCategoryId == -1 || receive.ReceiveItems.Any(ri => ri.Item.Equipment.EquipmentCategoryId == receiveDTO.EquipmentCategoryId)) &&
                //item
                (receiveDTO.ItemId == -1 || receive.ReceiveItems.Any(ri => ri.ItemId == receiveDTO.ItemId))
                )
                .Include(transfer => transfer.ReceivingSite)
                .Include(transfer => transfer.Purchase.RequestedBy)//here
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (receiveDTO.GroupBy)
            {
                //status
                case RECEIVEREPORTGROUPBY.STATUS:
                    summary = receives.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case RECEIVEREPORTGROUPBY.PURCHASEDATE:
                    summary = receives.GroupBy(transfer => transfer.PurchaseDate == null ? DateTime.MinValue.Date : transfer.PurchaseDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().PurchaseDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case RECEIVEREPORTGROUPBY.APPROVEDATE:
                    summary = receives.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case RECEIVEREPORTGROUPBY.RECEIVEDATE:
                    summary = receives.GroupBy(transfer => transfer.ReceiveDate == null ? DateTime.MinValue.Date : transfer.ReceiveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceiveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case RECEIVEREPORTGROUPBY.PURCHASEWEEK:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.DayOfYear / 7,
                                Year = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().PurchaseDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case RECEIVEREPORTGROUPBY.APPROVEWEEK:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case RECEIVEREPORTGROUPBY.RECEIVEWEEK:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.DayOfYear / 7,
                                Year = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ReceiveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case RECEIVEREPORTGROUPBY.PURCHASEMONTH:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Month,
                                Year = transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().PurchaseDate.Value == null ? "N/A" : $"{s.First().PurchaseDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case RECEIVEREPORTGROUPBY.APPROVEMONTH:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case RECEIVEREPORTGROUPBY.RECEIVEMONTH:
                    summary = receives.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Month,
                                Year = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ReceiveDate.Value == null ? "N/A" : $"{s.First().ReceiveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case RECEIVEREPORTGROUPBY.PURCHASEYEAR:
                    summary = receives.GroupBy(transfer =>
                            transfer.PurchaseDate.Value == null ? -1 : transfer.PurchaseDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().PurchaseDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case RECEIVEREPORTGROUPBY.APPROVEYEAR:
                    summary = receives.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case RECEIVEREPORTGROUPBY.RECEIVEYEAR:
                    summary = receives.GroupBy(transfer =>
                            transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ReceiveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case RECEIVEREPORTGROUPBY.PURCHASEDBY:
                    summary = receives.GroupBy(transfer => transfer.Purchase.RequestedById)//here
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Purchase.RequestedBy });//here
                    break;

                case RECEIVEREPORTGROUPBY.APPROVEDBY:
                    summary = receives.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case RECEIVEREPORTGROUPBY.RECEIVEDBY:
                    summary = receives.GroupBy(transfer => transfer.ReceivedById == null ? 0 : transfer.ReceivedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceivedBy });
                    break;

                //site
                case RECEIVEREPORTGROUPBY.SITE:
                    summary = receives.GroupBy(transfer => transfer.ReceivingSiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceivingSite });
                    break;
            }

            var result = new ReportReturnDTO<Receive>();

            result.Summary = summary;
            result.Data = await receives.ToListAsync();

            return result;
        }

        public async Task<ReportReturnDTO<Issue>> GetIssueReport(IssueReportDTO issueDTO)
        {
            issueDTO.SetDates(); // parse request data to set what date is given
            issueDTO.SetEmployees();

            var issues = _context.Issues
                .Where(issue =>
                //dates
                (issueDTO.RequestDateFrom == null || issue.RequestDate >= issueDTO.RequestDateFrom) &&
                (issueDTO.RequestDateTo == null || issue.RequestDate <= issueDTO.RequestDateTo) &&
                (issueDTO.ApproveDateFrom == null || issue.ApproveDate >= issueDTO.ApproveDateFrom) &&
                (issueDTO.ApproveDateTo == null || issue.ApproveDate <= issueDTO.ApproveDateTo) &&
                (issueDTO.HandDateFrom == null || issue.HandDate >= issueDTO.HandDateFrom) &&
                (issueDTO.HandDateTo == null || issue.HandDate <= issueDTO.HandDateTo) &&
                //sites
                (issueDTO.SiteId == -1 || issue.SiteId == issueDTO.SiteId) &&
                //status
                (issueDTO.Status == -1 || issue.Status == issueDTO.Status) &&
                //single role one user
                (issueDTO.EmployeeRole == -1 || issueDTO.RequestedById == null || issue.RequestedById == issueDTO.RequestedById) &&
                (issueDTO.EmployeeRole == -1 || issueDTO.ApprovedById == null || issue.ApprovedById == issueDTO.ApprovedById) &&
                (issueDTO.EmployeeRole == -1 || issueDTO.HandedById == null || issue.HandedById == issueDTO.HandedById) &&
                //multiple role one user
                (issueDTO.EmployeeRole != -1 || issueDTO.EmployeeId == -1 || issue.RequestedById == issueDTO.RequestedById || issue.ApprovedById == issueDTO.ApprovedById || issue.HandedById == issueDTO.HandedById) &&
                //item
                (issueDTO.ItemId == -1 || issue.IssueItems.Any(ti => ti.ItemId == issueDTO.ItemId))
                )
                .Include(transfer => transfer.Site)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.HandedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (issueDTO.GroupBy)
            {
                //status
                case ISSUEREPORTGROUPBY.STATUS:
                    summary = issues.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case ISSUEREPORTGROUPBY.REQUESTDATE:
                    summary = issues.GroupBy(transfer => transfer.RequestDate.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestDate.ToShortDateString() });
                    break;

                case ISSUEREPORTGROUPBY.APPROVEDATE:
                    summary = issues.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case ISSUEREPORTGROUPBY.HANDDATE:
                    summary = issues.GroupBy(transfer => transfer.HandDate == null ? DateTime.MinValue.Date : transfer.HandDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().HandDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case ISSUEREPORTGROUPBY.REQUESTWEEK:
                    summary = issues.GroupBy(transfer => new { Week = transfer.RequestDate.DayOfYear / 7, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"Week {s.Key.Week}, {s.Key.Year}" });
                    break;

                case ISSUEREPORTGROUPBY.APPROVEWEEK:
                    summary = issues.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case ISSUEREPORTGROUPBY.HANDWEEK:
                    summary = issues.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.DayOfYear / 7,
                                Year = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().HandDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case ISSUEREPORTGROUPBY.REQUESTMONTH:
                    summary = issues.GroupBy(transfer => new { Month = transfer.RequestDate.Month, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.First().RequestDate.ToString("MMMM")}, {s.Key.Year}" });
                    break;

                case ISSUEREPORTGROUPBY.APPROVEMONTH:
                    summary = issues.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case ISSUEREPORTGROUPBY.HANDMONTH:
                    summary = issues.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Month,
                                Year = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().HandDate.Value == null ? "N/A" : $"{s.First().HandDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                //year
                case ISSUEREPORTGROUPBY.REQUESTYEAR:
                    summary = issues.GroupBy(transfer => transfer.RequestDate.Year)
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.Key}" });
                    break;

                case ISSUEREPORTGROUPBY.APPROVEYEAR:
                    summary = issues.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case ISSUEREPORTGROUPBY.HANDYEAR:
                    summary = issues.GroupBy(transfer =>
                            transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().HandDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case ISSUEREPORTGROUPBY.REQUESTEDBY:
                    summary = issues.GroupBy(transfer => transfer.RequestedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });
                    break;

                case ISSUEREPORTGROUPBY.APPROVEDBY:
                    summary = issues.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case ISSUEREPORTGROUPBY.HANDEDBY:
                    summary = issues.GroupBy(transfer => transfer.HandedById == null ? 0 : transfer.HandedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().HandedBy });
                    break;

                //site
                case ISSUEREPORTGROUPBY.SITE:
                    summary = issues.GroupBy(transfer => transfer.SiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Site });
                    break;
            }


            var result = new ReportReturnDTO<Issue>();

            result.Summary = summary;
            result.Data = await issues.ToListAsync();

            return result;
        }

        public async Task<ReportReturnDTO<Transfer>> GetTansferReport(TransferReportDTO transferDTO)
        {
            transferDTO.SetDates(); // parse request data to set what date is given
            transferDTO.SetEmployees();

            var transfers = _context.Transfers
                .Where(transfer =>
                //dates
                (transferDTO.RequestDateFrom == null || transfer.RequestDate >= transferDTO.RequestDateFrom) &&
                (transferDTO.RequestDateTo == null || transfer.RequestDate <= transferDTO.RequestDateTo) &&
                (transferDTO.ApproveDateFrom == null || transfer.ApproveDate >= transferDTO.ApproveDateFrom) &&
                (transferDTO.ApproveDateTo == null || transfer.ApproveDate <= transferDTO.ApproveDateTo) &&
                (transferDTO.SendDateFrom == null || transfer.SendDate >= transferDTO.SendDateFrom) &&
                (transferDTO.SendDateTo == null || transfer.SendDate <= transferDTO.SendDateTo) &&
                (transferDTO.ReceiveDateFrom == null || transfer.ReceiveDate >= transferDTO.ReceiveDateFrom) &&
                (transferDTO.ReceiveDateTo == null || transfer.ReceiveDate <= transferDTO.ReceiveDateTo) &&
                //sites
                (transferDTO.SendSiteId == -1 || transfer.SendSiteId == transferDTO.SendSiteId) &&
                (transferDTO.ReceiveSiteId == -1 || transfer.ReceiveSiteId == transferDTO.ReceiveSiteId) &&
                //status
                (transferDTO.Status == -1 || transfer.Status == transferDTO.Status) &&
                //single role one user
                (transferDTO.EmployeeRole == -1 || transferDTO.RequestedById == null || transfer.RequestedById == transferDTO.RequestedById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.ApprovedById == null || transfer.ApprovedById == transferDTO.ApprovedById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.SentById == null || transfer.SentById == transferDTO.SentById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.ReceivedById == null || transfer.ReceivedById == transferDTO.ReceivedById) &&
                //multiple role one user
                (transferDTO.EmployeeRole != -1 || transferDTO.EmployeeId == -1 || transfer.RequestedById == transferDTO.RequestedById || transfer.ApprovedById == transferDTO.ApprovedById || transfer.SentById == transferDTO.SentById || transfer.ReceivedById == transferDTO.ReceivedById) &&
                //item type
                (transferDTO.ItemType == -1 || transfer.TransferItems.Any(ti => ti.Item.Type == transferDTO.ItemType)) &&
                //equipment category
                (transferDTO.ItemType != ITEMTYPE.EQUIPMENT || transferDTO.EquipmentCategoryId == -1 || transfer.TransferItems.Any(ti => ti.Item.Equipment.EquipmentCategoryId == transferDTO.EquipmentCategoryId)) &&
                //item
                (transferDTO.ItemId == -1 || transfer.TransferItems.Any(ti => ti.ItemId == transferDTO.ItemId))
                )
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.SentBy)
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (transferDTO.GroupBy)
            {
                //status
                case TRANSFERREPORTGROUPBY.STATUS:
                    summary = transfers.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case TRANSFERREPORTGROUPBY.REQUESTDATE:
                    summary = transfers.GroupBy(transfer => transfer.RequestDate.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestDate.ToShortDateString() });
                    break;

                case TRANSFERREPORTGROUPBY.APPROVEDATE:
                    summary = transfers.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case TRANSFERREPORTGROUPBY.SENDDATE:
                    summary = transfers.GroupBy(transfer => transfer.SendDate == null ? DateTime.MinValue.Date : transfer.SendDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().SendDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVEDATE:
                    summary = transfers.GroupBy(transfer => transfer.ReceiveDate == null ? DateTime.MinValue.Date : transfer.ReceiveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceiveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case TRANSFERREPORTGROUPBY.REQUESTWEEK:
                    summary = transfers.GroupBy(transfer => new { Week = transfer.RequestDate.DayOfYear / 7, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"Week {s.Key.Week}, {s.Key.Year}" });
                    break;

                case TRANSFERREPORTGROUPBY.APPROVEWEEK:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case TRANSFERREPORTGROUPBY.SENDWEEK:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.SendDate.Value == null ? -1 : transfer.SendDate.Value.DayOfYear / 7,
                                Year = transfer.SendDate.Value == null ? -1 : transfer.SendDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().SendDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVEWEEK:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.DayOfYear / 7,
                                Year = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ReceiveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case TRANSFERREPORTGROUPBY.REQUESTMONTH:
                    summary = transfers.GroupBy(transfer => new { Month = transfer.RequestDate.Month, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.First().RequestDate.ToString("MMMM")}, {s.Key.Year}" });
                    break;

                case TRANSFERREPORTGROUPBY.APPROVEMONTH:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case TRANSFERREPORTGROUPBY.SENDMONTH:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.SendDate.Value == null ? -1 : transfer.SendDate.Value.Month,
                                Year = transfer.SendDate.Value == null ? -1 : transfer.SendDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().SendDate.Value == null ? "N/A" : $"{s.First().SendDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVEMONTH:
                    summary = transfers.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Month,
                                Year = transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ReceiveDate.Value == null ? "N/A" : $"{s.First().ReceiveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case TRANSFERREPORTGROUPBY.REQUESTYEAR:
                    summary = transfers.GroupBy(transfer => transfer.RequestDate.Year)
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.Key}" });
                    break;

                case TRANSFERREPORTGROUPBY.APPROVEYEAR:
                    summary = transfers.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case TRANSFERREPORTGROUPBY.SENDYEAR:
                    summary = transfers.GroupBy(transfer =>
                            transfer.SendDate.Value == null ? -1 : transfer.SendDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().SendDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVEYEAR:
                    summary = transfers.GroupBy(transfer =>
                            transfer.ReceiveDate.Value == null ? -1 : transfer.ReceiveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ReceiveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case TRANSFERREPORTGROUPBY.REQUESTEDBY:
                    summary = transfers.GroupBy(transfer => transfer.RequestedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });
                    break;

                case TRANSFERREPORTGROUPBY.APPROVEDBY:
                    summary = transfers.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case TRANSFERREPORTGROUPBY.SENTBY:
                    summary = transfers.GroupBy(transfer => transfer.SentById == null ? 0 : transfer.SentById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().SentBy });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVEDBY:
                    summary = transfers.GroupBy(transfer => transfer.ReceivedById == null ? 0 : transfer.ReceivedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceivedBy });
                    break;

                //site
                case TRANSFERREPORTGROUPBY.SENDSITE:
                    summary = transfers.GroupBy(transfer => transfer.SendSiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().SendSite });
                    break;

                case TRANSFERREPORTGROUPBY.RECEIVESITE:
                    summary = transfers.GroupBy(transfer => transfer.ReceiveSiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ReceiveSite });
                    break;
            }

            var result = new ReportReturnDTO<Transfer>();

            result.Summary = summary;
            result.Data = await transfers.ToListAsync();

            return result;
        }

        public async Task<ReportReturnDTO<Borrow>> GetBorrowReport(BorrowReportDTO borrowDTO)
        {
            borrowDTO.SetDates(); // parse request data to set what date is given
            borrowDTO.SetEmployees();

            var borrows = _context.Borrows
                .Where(borrow =>
                //dates
                (borrowDTO.RequestDateFrom == null || borrow.RequestDate >= borrowDTO.RequestDateFrom) &&
                (borrowDTO.RequestDateTo == null || borrow.RequestDate <= borrowDTO.RequestDateTo) &&
                (borrowDTO.ApproveDateFrom == null || borrow.ApproveDate >= borrowDTO.ApproveDateFrom) &&
                (borrowDTO.ApproveDateTo == null || borrow.ApproveDate <= borrowDTO.ApproveDateTo) &&
                (borrowDTO.HandDateFrom == null || borrow.HandDate >= borrowDTO.HandDateFrom) &&
                (borrowDTO.HandDateTo == null || borrow.HandDate <= borrowDTO.HandDateTo) &&
                //sites
                (borrowDTO.SiteId == -1 || borrow.SiteId == borrowDTO.SiteId) &&
                //status
                (borrowDTO.Status == -1 || borrow.Status == borrowDTO.Status) &&
                //single role one user
                (borrowDTO.EmployeeRole == -1 || borrowDTO.RequestedById == null || borrow.RequestedById == borrowDTO.RequestedById) &&
                (borrowDTO.EmployeeRole == -1 || borrowDTO.ApprovedById == null || borrow.ApprovedById == borrowDTO.ApprovedById) &&
                (borrowDTO.EmployeeRole == -1 || borrowDTO.HandedById == null || borrow.HandedById == borrowDTO.HandedById) &&
                //multiple role one user
                (borrowDTO.EmployeeRole != -1 || borrowDTO.EmployeeId == -1 || borrow.RequestedById == borrowDTO.RequestedById || borrow.ApprovedById == borrowDTO.ApprovedById || borrow.HandedById == borrowDTO.HandedById) &&
                //equipment category
                (borrowDTO.EquipmentCategoryId == -1 || borrow.BorrowItems.Any(ti => ti.Item.Equipment.EquipmentCategoryId == borrowDTO.EquipmentCategoryId)) &&
                //item
                (borrowDTO.ItemId == -1 || borrow.BorrowItems.Any(ti => ti.ItemId == borrowDTO.ItemId))
                )
                .Include(transfer => transfer.Site)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.HandedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (borrowDTO.GroupBy)
            {
                //status
                case BORROWREPORTGROUPBY.STATUS:
                    summary = borrows.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case BORROWREPORTGROUPBY.REQUESTDATE:
                    summary = borrows.GroupBy(transfer => transfer.RequestDate.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestDate.ToShortDateString() });
                    break;

                case BORROWREPORTGROUPBY.APPROVEDATE:
                    summary = borrows.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case BORROWREPORTGROUPBY.HANDDATE:
                    summary = borrows.GroupBy(transfer => transfer.HandDate == null ? DateTime.MinValue.Date : transfer.HandDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().HandDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case BORROWREPORTGROUPBY.REQUESTWEEK:
                    summary = borrows.GroupBy(transfer => new { Week = transfer.RequestDate.DayOfYear / 7, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"Week {s.Key.Week}, {s.Key.Year}" });
                    break;

                case BORROWREPORTGROUPBY.APPROVEWEEK:
                    summary = borrows.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case BORROWREPORTGROUPBY.HANDWEEK:
                    summary = borrows.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.DayOfYear / 7,
                                Year = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().HandDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case BORROWREPORTGROUPBY.REQUESTMONTH:
                    summary = borrows.GroupBy(transfer => new { Month = transfer.RequestDate.Month, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.First().RequestDate.ToString("MMMM")}, {s.Key.Year}" });
                    break;

                case BORROWREPORTGROUPBY.APPROVEMONTH:
                    summary = borrows.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case BORROWREPORTGROUPBY.HANDMONTH:
                    summary = borrows.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Month,
                                Year = transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().HandDate.Value == null ? "N/A" : $"{s.First().HandDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                //year
                case BORROWREPORTGROUPBY.REQUESTYEAR:
                    summary = borrows.GroupBy(transfer => transfer.RequestDate.Year)
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.Key}" });
                    break;

                case BORROWREPORTGROUPBY.APPROVEYEAR:
                    summary = borrows.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case BORROWREPORTGROUPBY.HANDYEAR:
                    summary = borrows.GroupBy(transfer =>
                            transfer.HandDate.Value == null ? -1 : transfer.HandDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().HandDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case BORROWREPORTGROUPBY.REQUESTEDBY:
                    summary = borrows.GroupBy(transfer => transfer.RequestedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });
                    break;

                case BORROWREPORTGROUPBY.APPROVEDBY:
                    summary = borrows.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case BORROWREPORTGROUPBY.HANDEDBY:
                    summary = borrows.GroupBy(transfer => transfer.HandedById == null ? 0 : transfer.HandedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().HandedBy });
                    break;

                //site
                case BORROWREPORTGROUPBY.SITE:
                    summary = borrows.GroupBy(transfer => transfer.SiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Site });
                    break;
            }

            var result = new ReportReturnDTO<Borrow>();

            result.Summary = summary;
            result.Data = await borrows.ToListAsync();

            return result;
        }

        public async Task<ReportReturnDTO<Maintenance>> GetMaintenanceReport(MaintenanceReportDTO maintenanceDTO)
        {
            maintenanceDTO.SetDates(); // parse request data to set what date is given
            maintenanceDTO.SetEmployees();

            var maintenances = _context.Maintenances
                .Where(maintenance =>
                //dates
                (maintenanceDTO.RequestDateFrom == null || maintenance.RequestDate >= maintenanceDTO.RequestDateFrom) &&
                (maintenanceDTO.RequestDateTo == null || maintenance.RequestDate <= maintenanceDTO.RequestDateTo) &&
                (maintenanceDTO.ApproveDateFrom == null || maintenance.ApproveDate >= maintenanceDTO.ApproveDateFrom) &&
                (maintenanceDTO.ApproveDateTo == null || maintenance.ApproveDate <= maintenanceDTO.ApproveDateTo) &&
                (maintenanceDTO.FixDateFrom == null || maintenance.FixDate >= maintenanceDTO.FixDateFrom) &&
                (maintenanceDTO.FixDateTo == null || maintenance.FixDate <= maintenanceDTO.FixDateTo) &&
                //sites
                (maintenanceDTO.SiteId == -1 || maintenance.SiteId == maintenanceDTO.SiteId) &&
                //status
                (maintenanceDTO.Status == -1 || maintenance.Status == maintenanceDTO.Status) &&
                //single role one user
                (maintenanceDTO.EmployeeRole == -1 || maintenanceDTO.RequestedById == null || maintenance.RequestedById == maintenanceDTO.RequestedById) &&
                (maintenanceDTO.EmployeeRole == -1 || maintenanceDTO.ApprovedById == null || maintenance.ApprovedById == maintenanceDTO.ApprovedById) &&
                (maintenanceDTO.EmployeeRole == -1 || maintenanceDTO.FixedById == null || maintenance.FixedById == maintenanceDTO.FixedById) &&
                //multiple role one user
                (maintenanceDTO.EmployeeRole != -1 || maintenanceDTO.EmployeeId == -1 || maintenance.RequestedById == maintenanceDTO.RequestedById || maintenance.ApprovedById == maintenanceDTO.ApprovedById || maintenance.FixedById == maintenanceDTO.FixedById) &&
                //equipment category
                (maintenanceDTO.EquipmentCategoryId == -1 || maintenance.Item.Equipment.EquipmentCategoryId == maintenanceDTO.EquipmentCategoryId) &&
                //item
                (maintenanceDTO.ItemId == -1 || maintenance.ItemId == maintenanceDTO.ItemId))
                .Include(transfer => transfer.Site)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.FixedBy)
                .Include(transfer => transfer.ApprovedBy);

            IQueryable? summary = null;

            switch (maintenanceDTO.GroupBy)
            {
                //status
                case MAINTENANCEREPORTGROUPBY.STATUS:
                    summary = maintenances.GroupBy(transfer => transfer.Status)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Status });
                    break;

                //date
                case MAINTENANCEREPORTGROUPBY.REQUESTDATE:
                    summary = maintenances.GroupBy(transfer => transfer.RequestDate.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestDate.ToShortDateString() });
                    break;

                case MAINTENANCEREPORTGROUPBY.APPROVEDATE:
                    summary = maintenances.GroupBy(transfer => transfer.ApproveDate == null ? DateTime.MinValue.Date : transfer.ApproveDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApproveDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                case MAINTENANCEREPORTGROUPBY.FIXDATE:
                    summary = maintenances.GroupBy(transfer => transfer.FixDate == null ? DateTime.MinValue.Date : transfer.FixDate.Value.Date)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().FixDate.Value.ToShortDateString() ?? "N/A" });
                    break;

                //week
                case MAINTENANCEREPORTGROUPBY.REQUESTWEEK:
                    summary = maintenances.GroupBy(transfer => new { Week = transfer.RequestDate.DayOfYear / 7, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"Week {s.Key.Week}, {s.Key.Year}" });
                    break;

                case MAINTENANCEREPORTGROUPBY.APPROVEWEEK:
                    summary = maintenances.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.DayOfYear / 7,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                case MAINTENANCEREPORTGROUPBY.FIXWEEK:
                    summary = maintenances.GroupBy(transfer =>
                            new
                            {
                                Week = transfer.FixDate.Value == null ? -1 : transfer.FixDate.Value.DayOfYear / 7,
                                Year = transfer.FixDate.Value == null ? -1 : transfer.FixDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().FixDate.Value == null ? "N/A" : $"Week {s.Key.Week}, {s.Key.Year}"
                         });
                    break;

                //month
                case MAINTENANCEREPORTGROUPBY.REQUESTMONTH:
                    summary = maintenances.GroupBy(transfer => new { Month = transfer.RequestDate.Month, transfer.RequestDate.Year })
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.First().RequestDate.ToString("MMMM")}, {s.Key.Year}" });
                    break;

                case MAINTENANCEREPORTGROUPBY.APPROVEMONTH:
                    summary = maintenances.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Month,
                                Year = transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.First().ApproveDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                case MAINTENANCEREPORTGROUPBY.FIXMONTH:
                    summary = maintenances.GroupBy(transfer =>
                            new
                            {
                                Month = transfer.FixDate.Value == null ? -1 : transfer.FixDate.Value.Month,
                                Year = transfer.FixDate.Value == null ? -1 : transfer.FixDate.Value.Year
                            })
                         .Select(s => new
                         {
                             key = s.Key,
                             count = s.Count(),
                             value = s.First().FixDate.Value == null ? "N/A" : $"{s.First().FixDate.Value.ToString("MMMM")}, {s.Key.Year}"
                         });
                    break;

                //year
                case MAINTENANCEREPORTGROUPBY.REQUESTYEAR:
                    summary = maintenances.GroupBy(transfer => transfer.RequestDate.Year)
                         .Select(s => new { key = s.Key, count = s.Count(), value = $"{s.Key}" });
                    break;

                case MAINTENANCEREPORTGROUPBY.APPROVEYEAR:
                    summary = maintenances.GroupBy(transfer =>
                            transfer.ApproveDate.Value == null ? -1 : transfer.ApproveDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().ApproveDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                case MAINTENANCEREPORTGROUPBY.FIXYEAR:
                    summary = maintenances.GroupBy(transfer =>
                            transfer.FixDate.Value == null ? -1 : transfer.FixDate.Value.Year)
                        .Select(s => new
                        {
                            key = s.Key,
                            count = s.Count(),
                            value = s.First().FixDate.Value == null ? "N/A" : $"{s.Key}"
                        });
                    break;

                //employee
                case MAINTENANCEREPORTGROUPBY.REQUESTEDBY:
                    summary = maintenances.GroupBy(transfer => transfer.RequestedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().RequestedBy });
                    break;

                case MAINTENANCEREPORTGROUPBY.APPROVEDBY:
                    summary = maintenances.GroupBy(transfer => transfer.ApprovedById == null ? 0 : transfer.ApprovedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().ApprovedBy });
                    break;

                case MAINTENANCEREPORTGROUPBY.FIXEDBY:
                    summary = maintenances.GroupBy(transfer => transfer.FixedById == null ? 0 : transfer.FixedById)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().FixedBy });
                    break;

                //site
                case BORROWREPORTGROUPBY.SITE:
                    summary = maintenances.GroupBy(transfer => transfer.SiteId)
                         .Select(s => new { key = s.Key, count = s.Count(), value = s.First().Site });
                    break;
            }

            var result = new ReportReturnDTO<Maintenance>();

            result.Summary = summary;
            result.Data = await maintenances.ToListAsync();

            return result;
        }

        public async Task<GeneralReportReturnDTO> GetGeneralReport(GeneralReportDTO reportDTO)
        {
            reportDTO.SetDates();

            GeneralReportReturnDTO result = new();

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.RECEIVED))
            {
                var receiveSummary = await _generalReportService.GetReceiveReport(reportDTO);
                foreach (var x in receiveSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in receiveSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.ReceiveSummary = receiveSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.PURCHASED))
            {
                var purchaseSummary = await _generalReportService.GetPurchaseReport(reportDTO);
                foreach (var x in purchaseSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in purchaseSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.PurchaseSummary = purchaseSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.TRANSFERREDOUT))
            {
                var transferOutSummary = await _generalReportService.GetTransferOutReport(reportDTO);
                foreach (var x in transferOutSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in transferOutSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.TransferOutSummary = transferOutSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.TRANSFERREDIN))
            {
                var transferInSummary = await _generalReportService.GetTransferInReport(reportDTO);
                foreach (var x in transferInSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in transferInSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.TransferInSummary = transferInSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.ISSUED))
            {
                var issueSummary = await _generalReportService.GetIssueReport(reportDTO);
                foreach (var x in issueSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in issueSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.IssueSummary = issueSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.BORROWED))
            {
                var borrowSummary = await _generalReportService.GetBorrowReport(reportDTO);
                foreach (var x in borrowSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in borrowSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.BorrowSummary = borrowSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.RETURNED))
            {
                var returnSummary = await _generalReportService.GetReturnReport(reportDTO);
                foreach (var x in returnSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in returnSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.ReturnSummary = returnSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.DAMAGED))
            {
                var damageSummary = await _generalReportService.GetDamageReport(reportDTO);
                foreach (var x in damageSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in damageSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.DamageSummary = damageSummary.ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.INSTOCK))
            {
                var stockSummary = await _generalReportService.GetStockReport(reportDTO);
                foreach (var x in stockSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in stockSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.StockSummary = stockSummary.GroupBy(x => x.Key).Select( s => new ReportSingleItem
                {
                    Key = s.Key,
                    Label = s.First().Label,
                    Qty = s.Sum( i => i.Qty),
                    Cost = s.Sum( i => i.Cost),
                    CurrentValue = s.Sum(i => i.CurrentValue)

                }).ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            if (reportDTO.Include.Contains(GENERALREPORTSELECTION.MINSTOCK))
            {
                var minStockSummary = await _generalReportService.GetMinStockReport(reportDTO);
                foreach (var x in minStockSummary) result.Keys.Add(x.Key.ToString());
                foreach (var x in minStockSummary) result.Labels.TryAdd(x.Key.ToString(), x.Label);
                result.MinStockSummary = minStockSummary.GroupBy(x => x.Key).Select(s => new ReportSingleItem
                {
                    Key = s.Key,
                    Label = s.First().Label,
                    Qty = s.Sum(i => i.Qty),
                    Cost = s.Sum(i => i.Cost),
                    CurrentValue = s.Sum(i => i.CurrentValue)

                }).ToDictionary(d => (string)d.Key.ToString(), d => d);
            }

            result.Keys = result.Keys.OrderBy(x => x).ToHashSet();

            return result;
        }


    }
}
