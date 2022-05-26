using ERP.Context;
using ERP.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ReportServices
{
    public class GeneralReportService: IGeneralReportService
    {
        private readonly DataContext _context;

        public GeneralReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetPurchaseReport(GeneralReportDTO reportDTO)
        {
            var purchaseItems = await _context.PurchaseItems
                .Where(ii => ii.Purchase.Status >= PURCHASESTATUS.PURCHASED &&
                //date
                (reportDTO.DateFrom == null || ii.Purchase.PurchaseDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Purchase.PurchaseDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Purchase.ReceivingSiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.Item.Type != ITEMTYPE.EQUIPMENT || ii.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Purchase.CheckedById == reportDTO.EmployeeId)//here
                )
                .Include(ii => ii.Purchase)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .Include(ii => ii.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Purchase.CheckedBy)//here
                .Include(ii => ii.Purchase.ReceivingSite)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedPurchase = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyPurchased),
                            Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyPurchased * ii.Item.Material.Cost :
                                ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.Item.Equipment != null ? ii.Item.Equipment.EquipmentCategoryId : 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyPurchased),
                            Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyPurchased * ii.Item.Material.Cost :
                                ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" :
                                $"{s.First().Item.Name}, " +
                                $"{s.First().Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => ii.QtyPurchased),
                            Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyPurchased * ii.Item.Material.Cost :
                                ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.Purchase.PurchaseDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Purchase.PurchaseDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyPurchased),
                           Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyPurchased * ii.Item.Material.Cost :
                               ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedPurchase = purchaseItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Purchase.PurchaseDate.Value.DayOfYear / 7,
                        Year = ii.Purchase.PurchaseDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Purchase.PurchaseDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyPurchased),
                           Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyPurchased * ii.Item.Material.Cost :
                               ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedPurchase = purchaseItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Purchase.PurchaseDate.Value.Month,
                        Year = ii.Purchase.PurchaseDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Purchase.PurchaseDate.Value.Date,
                           Label = $"{s.First().Purchase.PurchaseDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyPurchased),
                           Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyPurchased * ii.Item.Material.Cost :
                               ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.Purchase.PurchaseDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Purchase.PurchaseDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyPurchased),
                           Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyPurchased * ii.Item.Material.Cost :
                               ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.Purchase.CheckedById ?? 0)//here
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Purchase.CheckedBy.FName} {s.First().Purchase.CheckedBy.MName}",//here
                            Qty = s.Sum(ii => ii.QtyPurchased),
                            Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyPurchased * ii.Item.Material.Cost :
                                ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedPurchase = purchaseItems.GroupBy(ii => ii.Purchase.ReceivingSiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Purchase.ReceivingSite.Name,
                            Qty = s.Sum(ii => ii.QtyPurchased),
                            Cost = s.Sum(ii => ii.QtyPurchased * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyPurchased * ii.Item.Material.Cost :
                                ii.QtyPurchased * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedPurchase;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetReceiveReport(GeneralReportDTO reportDTO)
        {
            var receiveItems = await _context.ReceiveItems
                .Where(ii => ii.Receive.Status >= RECEIVESTATUS.RECEIVED &&
                //date
                (reportDTO.DateFrom == null || ii.Receive.ReceiveDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Receive.ReceiveDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Receive.ReceivingSiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.Item.Type != ITEMTYPE.EQUIPMENT || ii.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Receive.ReceivedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Receive)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .Include(ii => ii.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Receive.ReceivedBy)
                .Include(ii => ii.Receive.ReceivingSite)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedReceive = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedReceive = receiveItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyReceived),
                            Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyReceived * ii.Item.Material.Cost :
                                ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedReceive = receiveItems.GroupBy(ii => ii.Item.Equipment != null ? ii.Item.Equipment.EquipmentCategoryId : 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyReceived),
                            Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyReceived * ii.Item.Material.Cost :
                                ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedReceive = receiveItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" :
                                $"{s.First().Item.Name}, " +
                                $"{s.First().Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => ii.QtyReceived),
                            Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyReceived * ii.Item.Material.Cost :
                                ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedReceive = receiveItems.GroupBy(ii => ii.Receive.ReceiveDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Receive.ReceiveDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyReceived),
                           Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyReceived * ii.Item.Material.Cost :
                               ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedReceive = receiveItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Receive.ReceiveDate.Value.DayOfYear / 7,
                        Year = ii.Receive.ReceiveDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Receive.ReceiveDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyReceived),
                           Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyReceived * ii.Item.Material.Cost :
                               ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedReceive = receiveItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Receive.ReceiveDate.Value.Month,
                        Year = ii.Receive.ReceiveDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Receive.ReceiveDate.Value.Date,
                           Label = $"{s.First().Receive.ReceiveDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyReceived),
                           Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyReceived * ii.Item.Material.Cost :
                               ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedReceive = receiveItems.GroupBy(ii => ii.Receive.ReceiveDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Receive.ReceiveDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyReceived),
                           Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyReceived * ii.Item.Material.Cost :
                               ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedReceive = receiveItems.GroupBy(ii => ii.Receive.ReceivedById ?? 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Receive.ReceivedBy.FName} {s.First().Receive.ReceivedBy.MName}",
                            Qty = s.Sum(ii => ii.QtyReceived),
                            Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyReceived * ii.Item.Material.Cost :
                                ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedReceive = receiveItems.GroupBy(ii => ii.Receive.ReceivingSiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Receive.ReceivingSite.Name,
                            Qty = s.Sum(ii => ii.QtyReceived),
                            Cost = s.Sum(ii => ii.QtyReceived * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyReceived * ii.Item.Material.Cost :
                                ii.QtyReceived * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedReceive;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetMinStockReport(GeneralReportDTO reportDTO)
        {
            var materialItems = await _context.MaterialSiteQties
                .Where(ii =>
                //site
                (reportDTO.SiteId == -1 || ii.SiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId)
                )
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Site)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedMaterial = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedMaterial = materialItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedMaterial = materialItems.GroupBy(ii => ii.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Site.Name,
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedMaterial = materialItems.GroupBy(ii => 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = "Material",
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.Item.Material.Cost)
                        });
                    break;
            }

            /////////////////////////////////////////////////////////////////////////////Equipments
            var transferOutItems = await _context.EquipmentSiteQties
               .Where(ii =>
               //site
               (reportDTO.SiteId == -1 || ii.SiteId == reportDTO.SiteId) &&
               //item type
               (reportDTO.ItemType == -1 || ii.EquipmentModel.Equipment.Item.Type == reportDTO.ItemType) &&
               //equipment category
               (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.EquipmentModel.Equipment.Item.Type != ITEMTYPE.EQUIPMENT ||
                    ii.EquipmentModel.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
               //item
               (reportDTO.ItemId == -1 || ii.EquipmentModel.ItemId == reportDTO.ItemId)
               )
               .Include(ii => ii.EquipmentModel.Equipment.Item)
               .Include(ii => ii.Site)
               .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedEquipment = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModel.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().EquipmentModel.Equipment.Item.Name,
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.EquipmentModel.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModel.Equipment.EquipmentCategoryId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().EquipmentModel.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.EquipmentModel.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = $"{s.First().EquipmentModel.Equipment.Item.Name}, {s.First().EquipmentModel.Name}",
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.EquipmentModel.Cost)
                        });
                    break;


                case GENERALREPORTGROUPBY.SITE:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Site.Name,
                            Qty = s.Sum(ii => ii.MinimumQty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.MinimumQty * ii.EquipmentModel.Cost)
                        });
                    break;
            }

            List<ReportSingleItem> result = new();
            result.AddRange(groupedEquipment);
            result.AddRange(groupedMaterial);

            return result;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetDamageReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.BorrowItemEquipmentAssets
                .Where(ii => ii.ReturnId != null && ii.AssetDamageId != null &&
                //date
                (reportDTO.DateFrom == null || ii.Return.ReturnDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Return.ReturnDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Return.SiteId == reportDTO.SiteId) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.BorrowItem.Item.Type != ITEMTYPE.EQUIPMENT || ii.BorrowItem.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Borrow.RequestedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Return)
                .Include(ii => ii.BorrowItem.Item.Equipment.EquipmentModels)
                .Include(ii => ii.BorrowItem.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Return)
                .Include(ii => ii.Borrow.RequestedBy)
                .Include(ii => ii.Return.Site)
                .Include(ii => ii.AssetDamage)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().BorrowItem.Item.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.BorrowItem.Item.Equipment.EquipmentCategoryId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().BorrowItem.Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = $"{s.First().BorrowItem.Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.ReturnDate.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Return.ReturnDate.DayOfYear / 7,
                        Year = ii.Return.ReturnDate.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Return.ReturnDate.Month,
                        Year = ii.Return.ReturnDate.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"{s.First().Return.ReturnDate.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.ReturnDate.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.RequestedById)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Borrow.RequestedBy.FName} {s.First().Borrow.RequestedBy.MName}",
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Return.Site.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost * ii.AssetDamage.PenalityPercentage),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost * ii.AssetDamage.PenalityPercentage)
                        });
                    break;
            }

            return groupedTransfer;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetStockReport(GeneralReportDTO reportDTO)
        {
            var materialItems = await _context.MaterialSiteQties
                .Where(ii => 
                //site
                (reportDTO.SiteId == -1 || ii.SiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) 
                )
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Site)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedMaterial = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedMaterial = materialItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedMaterial = materialItems.GroupBy(ii => ii.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Site.Name,
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedMaterial = materialItems.GroupBy(ii => 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = "Material",
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().Item.Material.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.Item.Material.Cost)
                        });
                    break;
            }

            /////////////////////////////////////////////////////////////////////////////Equipments
            var transferOutItems = await _context.EquipmentSiteQties
               .Where(ii => 
               //site
               (reportDTO.SiteId == -1 || ii.SiteId == reportDTO.SiteId) &&
               //item type
               (reportDTO.ItemType == -1 || ii.EquipmentModel.Equipment.Item.Type == reportDTO.ItemType) &&
               //equipment category
               (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.EquipmentModel.Equipment.Item.Type != ITEMTYPE.EQUIPMENT || 
                    ii.EquipmentModel.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
               //item
               (reportDTO.ItemId == -1 || ii.EquipmentModel.ItemId == reportDTO.ItemId)
               )
               .Include(ii => ii.EquipmentModel.Equipment.Item)
               .Include(ii => ii.Site)
               .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedEquipment = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModel.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().EquipmentModel.Equipment.Item.Name,
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.EquipmentModel.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModel.Equipment.EquipmentCategoryId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().EquipmentModel.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.EquipmentModel.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = $"{s.First().EquipmentModel.Equipment.Item.Name}, {s.First().EquipmentModel.Name}",
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.EquipmentModel.Cost)
                        });
                    break;
                                   

                case GENERALREPORTGROUPBY.SITE:
                    groupedEquipment = transferOutItems.GroupBy(ii => ii.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Site.Name,
                            Qty = s.Sum(ii => ii.Qty),
                            Cost = s.First().EquipmentModel.Cost,
                            CurrentValue = s.Sum(ii => ii.Qty * ii.EquipmentModel.Cost)
                        });
                    break;
            }

            List<ReportSingleItem> result = new();
            result.AddRange(groupedEquipment);
            result.AddRange(groupedMaterial);

            return result;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetReturnReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.BorrowItemEquipmentAssets
                .Where(ii => ii.ReturnId != null &&
                //date
                (reportDTO.DateFrom == null || ii.Return.ReturnDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Return.ReturnDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Return.SiteId == reportDTO.SiteId) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.BorrowItem.Item.Type != ITEMTYPE.EQUIPMENT || ii.BorrowItem.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Borrow.RequestedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Return)
                .Include(ii => ii.BorrowItem.Item.Equipment.EquipmentModels)
                .Include(ii => ii.BorrowItem.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Return)
                .Include(ii => ii.Borrow.RequestedBy)
                .Include(ii => ii.Return.Site)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().BorrowItem.Item.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.BorrowItem.Item.Equipment.EquipmentCategoryId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label =  s.First().BorrowItem.Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = $"{s.First().BorrowItem.Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.ReturnDate.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Return.ReturnDate.DayOfYear / 7,
                        Year = ii.Return.ReturnDate.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Return.ReturnDate.Month,
                        Year = ii.Return.ReturnDate.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"{s.First().Return.ReturnDate.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.ReturnDate.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Return.ReturnDate.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => 1),
                           Cost = s.Sum(ii => ii.BorrowItem.Cost),
                           CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.RequestedById)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Borrow.RequestedBy.FName} {s.First().Borrow.RequestedBy.MName}",
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Return.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Return.Site.Name,
                            Qty = s.Sum(ii => 1),
                            Cost = s.Sum(ii => ii.BorrowItem.Cost),
                            CurrentValue = s.Sum(ii => ii.BorrowItem.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedTransfer;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetBorrowReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.BorrowItems
                .Where(ii => ii.Borrow.Status >= BORROWSTATUS.HANDED &&
                //date
                (reportDTO.DateFrom == null || ii.Borrow.HandDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Borrow.HandDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Borrow.SiteId == reportDTO.SiteId) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.Item.Type != ITEMTYPE.EQUIPMENT || ii.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Borrow.RequestedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Borrow)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .Include(ii => ii.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Borrow.RequestedBy)
                .Include(ii => ii.Borrow.Site)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii =>  ii.Item.Equipment.EquipmentCategoryId )
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" :
                                $"{s.First().Item.Name}, " +
                                $"{s.First().Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.HandDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Borrow.HandDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Borrow.HandDate.Value.DayOfYear / 7,
                        Year = ii.Borrow.HandDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Borrow.HandDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Borrow.HandDate.Value.Month,
                        Year = ii.Borrow.HandDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Borrow.HandDate.Value.Date,
                           Label = $"{s.First().Borrow.HandDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.HandDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Borrow.HandDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.RequestedById)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Borrow.RequestedBy.FName} {s.First().Borrow.RequestedBy.MName}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Borrow.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Borrow.Site.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedTransfer;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetIssueReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.IssueItems
                .Where(ii => ii.Issue.Status >= ISSUESTATUS.HANDED &&
                //date
                (reportDTO.DateFrom == null || ii.Issue.HandDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Issue.HandDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Issue.SiteId == reportDTO.SiteId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Issue.RequestedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Issue)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Issue.RequestedBy)
                .Include(ii => ii.Issue.Site)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Item.Equipment != null ? ii.Item.Equipment.EquipmentCategoryId : 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = "Materials",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Issue.HandDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Issue.HandDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Issue.HandDate.Value.DayOfYear / 7,
                        Year = ii.Issue.HandDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Issue.HandDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Issue.HandDate.Value.Month,
                        Year = ii.Issue.HandDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Issue.HandDate.Value.Date,
                           Label = $"{s.First().Issue.HandDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Issue.HandDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Issue.HandDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Issue.RequestedById)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Issue.RequestedBy.FName} {s.First().Issue.RequestedBy.MName}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Issue.SiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Issue.Site.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                        });
                    break;
            }

            return groupedTransfer;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetTransferOutReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.TransferItems
                .Where(ii => ii.Transfer.Status >= TRANSFERSTATUS.SENT &&
                //date
                (reportDTO.DateFrom == null || ii.Transfer.SendDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Transfer.SendDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Transfer.SendSiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.Item.Type != ITEMTYPE.EQUIPMENT || ii.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Transfer.SentById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Transfer)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .Include(ii => ii.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Transfer.SentBy)
                .Include(ii => ii.Transfer.SendSite)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Item.Equipment != null ? ii.Item.Equipment.EquipmentCategoryId : 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" :
                                $"{s.First().Item.Name}, " +
                                $"{s.First().Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.SendDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.SendDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Transfer.SendDate.Value.DayOfYear / 7,
                        Year = ii.Transfer.SendDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.SendDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Transfer.SendDate.Value.Month,
                        Year = ii.Transfer.SendDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.SendDate.Value.Date,
                           Label = $"{s.First().Transfer.SendDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.SendDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.SendDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.SentById ?? 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Transfer.SentBy.FName} {s.First().Transfer.SentBy.MName}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.SendSiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Transfer.SendSite.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedTransfer;
        }

        public async Task<IEnumerable<ReportSingleItem>> GetTransferInReport(GeneralReportDTO reportDTO)
        {
            var transferOutItems = await _context.TransferItems
                .Where(ii => ii.Transfer.Status >= TRANSFERSTATUS.RECEIVED &&
                //date
                (reportDTO.DateFrom == null || ii.Transfer.ReceiveDate >= reportDTO.DateFrom) &&
                (reportDTO.DateTo == null || ii.Transfer.ReceiveDate <= reportDTO.DateTo) &&
                //site
                (reportDTO.SiteId == -1 || ii.Transfer.ReceiveSiteId == reportDTO.SiteId) &&
                //item type
                (reportDTO.ItemType == -1 || ii.Item.Type == reportDTO.ItemType) &&
                //equipment category
                (reportDTO.ItemType != ITEMTYPE.EQUIPMENT || reportDTO.EquipmentCategoryId == -1 || ii.Item.Type != ITEMTYPE.EQUIPMENT || ii.Item.Equipment.EquipmentCategoryId == reportDTO.EquipmentCategoryId) &&
                //item
                (reportDTO.ItemId == -1 || ii.ItemId == reportDTO.ItemId) &&
                //employee
                (reportDTO.EmployeeId == -1 || ii.Transfer.ReceivedById == reportDTO.EmployeeId)
                )
                .Include(ii => ii.Transfer)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .Include(ii => ii.Item.Equipment.EquipmentCategory)
                .Include(ii => ii.Transfer.ReceivedBy)
                .Include(ii => ii.Transfer.ReceiveSite)
                .ToListAsync();

            IEnumerable<ReportSingleItem>? groupedTransfer = new List<ReportSingleItem>();

            switch (reportDTO.GroupBy)
            {
                case GENERALREPORTGROUPBY.ITEM:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.ItemId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Item.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.CATEGORY:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Item.Equipment != null ? ii.Item.Equipment.EquipmentCategoryId : 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" : s.First().Item.Equipment.EquipmentCategory.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.MODEL:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.EquipmentModelId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "Materials" :
                                $"{s.First().Item.Name}, " +
                                $"{s.First().Item.Equipment.EquipmentModels.Where(em => em.EquipmentModelId == s.First().EquipmentModelId).First().Name}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;



                case GENERALREPORTGROUPBY.DATE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.ReceiveDate.Value.Date)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.ReceiveDate.Value.Date,
                           Label = s.Key.ToShortDateString(),
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.WEEK:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Week = ii.Transfer.ReceiveDate.Value.DayOfYear / 7,
                        Year = ii.Transfer.ReceiveDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.ReceiveDate.Value.Date,
                           Label = $"Week{s.Key.Week}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.MONTH:
                    groupedTransfer = transferOutItems.GroupBy(ii =>
                    new
                    {
                        Month = ii.Transfer.ReceiveDate.Value.Month,
                        Year = ii.Transfer.ReceiveDate.Value.Year
                    })
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.ReceiveDate.Value.Date,
                           Label = $"{s.First().Transfer.ReceiveDate.Value.ToString("MMMM")}, {s.Key.Year}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.YEAR:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.ReceiveDate.Value.Year)
                       .Select(s => new ReportSingleItem
                       {
                           Key = s.First().Transfer.ReceiveDate.Value.Date,
                           Label = $"{s.Key}",
                           Qty = s.Sum(ii => ii.QtyApproved),
                           Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                           CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                               ii.QtyApproved * ii.Item.Material.Cost :
                               ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                   .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                       });
                    break;

                case GENERALREPORTGROUPBY.EMPLOYEE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.ReceivedById ?? 0)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.Key == 0 ? "N/A" :
                                $"{s.First().Transfer.ReceivedBy.FName} {s.First().Transfer.ReceivedBy.MName}",
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;

                case GENERALREPORTGROUPBY.SITE:
                    groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.ReceiveSiteId)
                        .Select(s => new ReportSingleItem
                        {
                            Key = s.Key,
                            Label = s.First().Transfer.ReceiveSite.Name,
                            Qty = s.Sum(ii => ii.QtyApproved),
                            Cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                            CurrentValue = s.Sum(ii => ii.Item.Equipment == null ?
                                ii.QtyApproved * ii.Item.Material.Cost :
                                ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                                    .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)
                        });
                    break;
            }

            return groupedTransfer;
        }
    }
}
