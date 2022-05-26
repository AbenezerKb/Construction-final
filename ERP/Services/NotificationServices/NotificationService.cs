using ERP.Context;
using ERP.Models;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private Notification notification;

        public NotificationService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            var notifications = await _context.Notifications
                    .Where(noti => noti.IsCleared == false)
                    .Include(noti => noti.Site)
                    .Include(noti => noti.Employee)
                    .ToListAsync();

            return notifications.OrderByDescending(n => n.Date).ToList();
        }

        public async Task<Notification> GetNotification(int id)
        {
            var notification = await _context.Notifications
                .Where(noti => noti.NotificationId == id)
                .FirstOrDefaultAsync();

            return notification;
        }

        public async Task<List<Notification>> GetUserNotifications()
        {

            List<Notification> notifications = new();

            if (_userService.UserRole == null || _userService.Employee.EmployeeSite == null) 
                return notifications;

            int siteId = (int)_userService.Employee.EmployeeSiteId;
            int employeeId = (int)_userService.Employee.EmployeeId;

            notifications.AddRange(await GetTransferNotifications(siteId, employeeId));
            notifications.AddRange(await GetIssueNotifications(siteId, employeeId));
            notifications.AddRange(await GetPurchaseNotifications(siteId, employeeId));
            notifications.AddRange(await GetBulkPurchaseNotifications(siteId, employeeId));
            notifications.AddRange(await GetBorrowNotifications(siteId, employeeId));
            notifications.AddRange(await GetReceiveNotifications(siteId, employeeId));
            notifications.AddRange(await GetMaintenanceNotifications(siteId, employeeId));
            notifications.AddRange(await GetBuyNotifications(siteId, employeeId));
            notifications.AddRange(await GetStockNotifications(siteId, employeeId));
            notifications.AddRange(await GetReturnNotifications(siteId, employeeId));

            return notifications.OrderByDescending(n => n.Date).ToList();
        }
        private async Task<List<Notification>> GetStockNotifications(int siteId, int employeeId)
        {
            List<Notification> notifications = new();

            if (_userService.UserRole.CanGetStockNotification)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        (noti.Type == NOTIFICATIONTYPE.MINEQUIPMENT || noti.Type == NOTIFICATIONTYPE.MINMATERIAL) &&
                        noti.SiteId == siteId)
                    .ToListAsync());


            return notifications;

        }
        
        private async Task<List<Notification>> GetMaintenanceNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestMaintenance)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.MAINTENANCE &&
                        (noti.Status == MAINTENANCESTATUS.FIXED || noti.Status == MAINTENANCESTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveMaintenance)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.MAINTENANCE&&
                        noti.Status == MAINTENANCESTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanFixMaintenance)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.MAINTENANCE &&
                        noti.Status == MAINTENANCESTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetReturnNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.RETURN &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync();

            return notifications;
        }

        private async Task<List<Notification>> GetTransferNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestTransfer)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.TRANSFER &&
                        (noti.Status == TRANSFERSTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveTransfer)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.TRANSFER &&
                        noti.Status == TRANSFERSTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanSendTransfer)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.TRANSFER &&
                        noti.Status == TRANSFERSTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanReceiveTransfer)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.TRANSFER &&
                        noti.Status == TRANSFERSTATUS.SENT &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetBorrowNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestBorrow)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BORROW &&
                        (noti.Status == BORROWSTATUS.HANDED || noti.Status == BORROWSTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveBorrow)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BORROW &&
                        noti.Status == BORROWSTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanHandBorrow)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BORROW &&
                        noti.Status == BORROWSTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetIssueNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestIssue)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.ISSUE &&
                        (noti.Status == ISSUESTATUS.HANDED || noti.Status == ISSUESTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveIssue)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.ISSUE &&
                        noti.Status == ISSUESTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanHandIssue)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.ISSUE &&
                        noti.Status == ISSUESTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetReceiveNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanReceive)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.RECEIVE &&
                        (noti.Status == RECEIVESTATUS.PURCHASED ) &&
                        noti.SiteId == siteId)
                    .ToListAsync());

             if (_userService.UserRole.CanReceive)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.RECEIVE &&
                        (noti.Status == RECEIVESTATUS.APPROVED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveReceive)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.RECEIVE &&
                        noti.Status == RECEIVESTATUS.RECEIVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetBuyNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestBuy)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BUY &&
                        (noti.Status == BUYSTATUS.BOUGHT || noti.Status == BUYSTATUS.QUEUED || noti.Status == BUYSTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanCheckBuy)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BUY &&
                        noti.Status == BUYSTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());
            
            if (_userService.UserRole.CanApproveBuy)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BUY &&
                        noti.Status == BUYSTATUS.CHECKED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanConfirmBuy)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BUY &&
                        noti.Status == BUYSTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetPurchaseNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.PURCHASE &&
                        (noti.Status == PURCHASESTATUS.BULKQUEUED || noti.Status == PURCHASESTATUS.PURCHASED || noti.Status == PURCHASESTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanRequestPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.PURCHASE &&
                        noti.Status == PURCHASESTATUS.QUEUED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanCheckPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.PURCHASE &&
                        noti.Status == PURCHASESTATUS.REQUESTED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanApprovePurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.PURCHASE &&
                        noti.Status == PURCHASESTATUS.CHECKED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            if (_userService.UserRole.CanConfirmPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.PURCHASE &&
                        noti.Status == PURCHASESTATUS.APPROVED &&
                        noti.SiteId == siteId)
                    .ToListAsync());

            return notifications;
        }

        private async Task<List<Notification>> GetBulkPurchaseNotifications(int siteId, int employeeId)
        {

            List<Notification> notifications = new();

            if (_userService.UserRole.CanRequestBulkPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BULKPURCHASE &&
                        (noti.Status == BULKPURCHASESTATUS.DECLINED) &&
                        noti.EmployeeId == employeeId)
                    .ToListAsync());

            if (_userService.UserRole.CanRequestBulkPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BULKPURCHASE &&
                        noti.Status == BULKPURCHASESTATUS.QUEUED || noti.Status == BULKPURCHASESTATUS.PURCHASED)
                    .ToListAsync());

            if (_userService.UserRole.CanApproveBulkPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BULKPURCHASE &&
                        noti.Status == BULKPURCHASESTATUS.REQUESTED)
                    .ToListAsync());

            if (_userService.UserRole.CanConfirmPurchase)
                notifications.AddRange(await _context.Notifications
                    .Where(noti => noti.IsCleared == false &&
                        noti.Type == NOTIFICATIONTYPE.BULKPURCHASE &&
                        noti.Status == BULKPURCHASESTATUS.APPROVED)
                    .ToListAsync());

            return notifications;
        }

        public async Task<bool> Add(string type, int status, int actionId, int siteId, int? employeeId)
        {
            //If there are previous notifications for the same transfer/issue/receive
            await Clear(type, actionId);

            notification = new();
            notification.Type = type;
            notification.Status = status;
            notification.ActionId = actionId;
            notification.SiteId = siteId;
            notification.EmployeeId = employeeId;
            await MakeContent(type, status, actionId);

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task Clear(string type, int actionId)
        {
            List<Notification> notifications = await _context.Notifications
                .Where(n => n.Type == type && n.ActionId == actionId)
                .ToListAsync();

           if (notifications.Count == 0) return;

            foreach (var notification in notifications)
            {
                notification.IsCleared = true;
            }

            await _context.SaveChangesAsync();

        }

        public async Task<bool> Clear(int notificationId)
        {
            var notification = await _context.Notifications
                .Where(n => n.NotificationId == notificationId)
                .FirstOrDefaultAsync();

            if (notification != null)
            {
                notification.IsCleared = true;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<Notification>> Get(string type, int status, int siteId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.Type == type && n.Status == status && n.SiteId == siteId && n.IsCleared == false)
                .ToListAsync();

            return notifications;
        }

        private async Task MakeContent(string type, int status, int actionId)
        {
            switch (type)
            {
                case NOTIFICATIONTYPE.TRANSFER:
                    await MakeContentTransfer(status, actionId);
                    break;

                case NOTIFICATIONTYPE.ISSUE:
                    await MakeContentIssue(status, actionId);
                    break;

                case NOTIFICATIONTYPE.PURCHASE:
                    await MakeContentPurchase(status, actionId);
                    break;

                case NOTIFICATIONTYPE.BULKPURCHASE:
                    await MakeContentBulkPurchase(status, actionId);
                    break;

                case NOTIFICATIONTYPE.BORROW:
                    await MakeContentBorrow(status, actionId);
                    break;

                case NOTIFICATIONTYPE.RECEIVE:
                    await MakeContentReceive(status, actionId);
                    break;

                case NOTIFICATIONTYPE.MAINTENANCE:
                    await MakeContentMaintainance(status, actionId);
                    break;

                case NOTIFICATIONTYPE.RETURN:
                    await MakeContentReturn(status, actionId);
                    break;

                case NOTIFICATIONTYPE.BUY:
                    await MakeContentBuy(status, actionId);
                    break;

                case NOTIFICATIONTYPE.MINEQUIPMENT:
                    await MakeContentMinEquipment(status, actionId);
                    break;

                case NOTIFICATIONTYPE.MINMATERIAL:
                    await MakeContentMinMaterial(status, actionId);
                    break;


            }
        }

        private async Task MakeContentMinMaterial(int status, int actionId)
        {
            var item = await _context.Items.Where(i => i.ItemId == actionId).FirstOrDefaultAsync();

            string itemName = item == null ? "A Material" : $"The {item.Name}";

            var site = await _context.Sites.Where(i => i.SiteId == notification.SiteId).FirstOrDefaultAsync();
            string siteName = site == null ? "A Site." : $"The {site.Name} Site.";

            notification.Title = "Running Out of Stock";
            notification.Content = $"{itemName} Available in Stock Is Less Than The Minimum Stock At {siteName}";
        }

        private async Task MakeContentMinEquipment(int status, int actionId)
        {
            var model = await _context.EquipmentModels.Where(em => em.EquipmentModelId == actionId)
                .Include(em => em.Equipment)
                .ThenInclude(e => e.Item)
                .FirstOrDefaultAsync();

            string itemName = model == null ? "An Equipment Model": $"The {model.Equipment.Item.Name} Model {model.Name}";

            var site = await _context.Sites.Where(i=> i.SiteId == notification.SiteId).FirstOrDefaultAsync();
            string siteName = site == null ? "A Site." : $"The {site.Name} Site.";

            notification.Title = "Stock Running Out";
            notification.Content = $"{itemName} Available in Stock Is Less Than The Minimum Stock At {siteName}";
        }

        private async Task MakeContentTransfer(int status, int actionId)
        {

            switch (status)
            {
                case TRANSFERSTATUS.REQUESTED:
                    notification.Title = "Approve Transfer Request";
                    notification.Content = $"New Transfer Request With Transfer No: {actionId}";
                    break;

                case TRANSFERSTATUS.DECLINED:
                    notification.Title = "Declined Transfer Request";
                    notification.Content = $"Transfer Request {actionId} Has Been Declined";
                    break;

                case TRANSFERSTATUS.APPROVED:
                    notification.Title = "Transfer Out Items";
                    notification.Content = $"Transfer Request {actionId} Has Been Approved";
                    break;

                case TRANSFERSTATUS.SENT:
                    notification.Title = "Transfer In Items";
                    notification.Content = $"Transfer Request {actionId} Items Has Been Sent";
                    break;

                case TRANSFERSTATUS.RECEIVED:
                    notification.Title = "Items Transferred In";
                    notification.Content = $"Transfer Request {actionId} Items Has Been Received";
                    break;

            }
        }

        private async Task MakeContentIssue(int status, int actionId)
        {
            switch (status)
            {
                case ISSUESTATUS.REQUESTED:
                    notification.Title = "Approve Issue";
                    notification.Content = $"New Issue Request With Issue No: {actionId}";
                    break;

                case ISSUESTATUS.APPROVED:
                    notification.Title = "Hand Over Issued Items";
                    notification.Content = $"Issue Request {actionId} Has Been Approved";
                    break;

                case ISSUESTATUS.HANDED:
                    notification.Title = "Issued Items Handed Over";
                    notification.Content = $"Issue Request {actionId} Items Has Been Handed Over";
                    break;

                case ISSUESTATUS.DECLINED:
                    notification.Title = "Declined Transfer Request";
                    notification.Content = $"Issue Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentPurchase(int status, int actionId)
        {
            switch (status)
            {
                case PURCHASESTATUS.QUEUED:
                    notification.Title = "Request Queued Purchase";
                    notification.Content = $"Request Queued Purchase With Request No: {actionId}";
                    break;
                
                case PURCHASESTATUS.REQUESTED:
                    notification.Title = "Check Purchase Request";
                    notification.Content = $"New Purchase Request With Request No: {actionId}";
                    break;

                case PURCHASESTATUS.CHECKED:
                    notification.Title = "Approve Purchased Items";
                    notification.Content = $"Approve Purchase Request With Request No: {actionId}";
                    break;

                case PURCHASESTATUS.APPROVED:
                    notification.Title = "Confirm Purchased Items";
                    notification.Content = $"Confirm Purchase Request With Request No: {actionId}";
                    break;

                case PURCHASESTATUS.BULKQUEUED:
                    notification.Title = "Queued Purchase Request";
                    notification.Content = $"Purchase Request {actionId} Has Been Queued For Bulk Purchase";
                    break;
                
                case PURCHASESTATUS.DECLINED:
                    notification.Title = "Declined Purchase Request";
                    notification.Content = $"Purchase Request {actionId} Has Been Declined";
                    break;
                
                case PURCHASESTATUS.PURCHASED:
                    notification.Title = "Confirmed Purchase Request";
                    notification.Content = $"Purchase Request {actionId} Has Been Purchased";
                    break;

            }
        }

        private async Task MakeContentBulkPurchase(int status, int actionId)
        {
            switch (status)
            {
                case BULKPURCHASESTATUS.QUEUED:
                    notification.Title = "Request Bulk Purchase";
                    notification.Content = $"Request Queued Bulk Purchase With Request No: {actionId}";
                    break;

                case BULKPURCHASESTATUS.REQUESTED:
                    notification.Title = "Approve Bulk Purchase Request";
                    notification.Content = $"Approve Bulk Purchase Request With Request No: {actionId}";
                    break;
                
                case BULKPURCHASESTATUS.APPROVED:
                    notification.Title = "Confirm Bulk Purchase Request";
                    notification.Content = $"Confirm Bulk Purchase Request With Request No: {actionId}";
                    break;

                case BULKPURCHASESTATUS.PURCHASED:
                    notification.Title = "Confirmed Bulk Purchase Request";
                    notification.Content = $"Bulk Purchase Request {actionId} Has Been Purchased";
                    break;

                case BULKPURCHASESTATUS.DECLINED:
                    notification.Title = "Declined Bulk Purchase Request";
                    notification.Content = $"Bulk Purchase Request {actionId} Has Been Declined";
                    break;
            }
        }

        private async Task MakeContentBuy(int status, int actionId)
        {
            switch (status)
            {
                case BUYSTATUS.REQUESTED:
                    notification.Title = "Check Buy Request";
                    notification.Content = $"New Buy Request With Request No: {actionId}";
                    break;

                case BUYSTATUS.CHECKED:
                    notification.Title = "Approve Buy Request";
                    notification.Content = $"Approve Buy Request With Request No: {actionId}";
                    break;

                case BUYSTATUS.APPROVED:
                    notification.Title = "Confirm Bought Items";
                    notification.Content = $"Confirm Buy Request With Request No: {actionId}";
                    break;

                case BUYSTATUS.DECLINED:
                    notification.Title = "Declined Buy Request";
                    notification.Content = $"Buy Request {actionId} Has Been Declined";
                    break;

                case BUYSTATUS.QUEUED:
                    notification.Title = "Queued Buy Request";
                    notification.Content = $"Buy Request {actionId} Has Been Queued For Purchase";
                    break;

                case BUYSTATUS.BOUGHT:
                    notification.Title = "Bought Buy Request";
                    notification.Content = $"Buy Request {actionId} Has Been Bought";
                    break;

            }
        }

        private async Task MakeContentReceive(int status, int actionId)
        {
            switch (status)
            {
                case RECEIVESTATUS.PURCHASED:
                    notification.Title = "Receive Purchased Items";
                    notification.Content = $"New Receive Order With Request No: {actionId}";
                    break;
                
                case RECEIVESTATUS.RECEIVED:
                    notification.Title = "Check Received Items";
                    notification.Content = $"Check Received Request With Request No: {actionId}";
                    break;

                case RECEIVESTATUS.APPROVED:
                    notification.Title = "Received Items Checked";
                    notification.Content = $"Items of Receive No. {actionId} Has Been Checked";
                    break;

            }
        }

        private async Task MakeContentBorrow(int status, int actionId)
        {
            switch (status)
            {
                case BORROWSTATUS.REQUESTED:
                    notification.Title = "Approve Borrow Request";
                    notification.Content = $"New Borrow Request With Borrow No: {actionId}";
                    break;

                case BORROWSTATUS.APPROVED:
                    notification.Title = "Hand Over Borrow Request Items";
                    notification.Content = $"Borrow Request {actionId} Has Been Approved";
                    break;

                case BORROWSTATUS.HANDED:
                    notification.Title = "Borrowed Items Handed Over";
                    notification.Content = $"Borrow Request {actionId} Items Has Been Handed Over";
                    break;

                case BORROWSTATUS.DECLINED:
                    notification.Title = "Declined Borrow Request";
                    notification.Content = $"Borrow Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentMaintainance(int status, int actionId)
        {
            switch (status)
            {
                case MAINTENANCESTATUS.REQUESTED:
                    notification.Title = "Approve Maintenance Request";
                    notification.Content = $"New Maintainance Request With Request No: {actionId}";
                    break;

                case MAINTENANCESTATUS.APPROVED:
                    notification.Title = "Maintenance Request Approved";
                    notification.Content = $"Item on Maintenance Request {actionId} Has Been Approved";
                    break;

                case MAINTENANCESTATUS.FIXED:
                    notification.Title = "Maintenance Request Item Fixed";
                    notification.Content = $"Item on Maintenance Request {actionId} Has Been Fixed";
                    break;

                case MAINTENANCESTATUS.DECLINED:
                    notification.Title = "Declined Mainenance Request";
                    notification.Content = $"Maintenance Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentReturn(int status, int actionId)
        {
            notification.Title = "Borrowed Items Returned";
            notification.Content = $"Borrowed Equipments Are Returned With Return No: {actionId}";
        }
    }
}
