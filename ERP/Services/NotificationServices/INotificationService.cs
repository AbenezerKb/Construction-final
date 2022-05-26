using ERP.Models;

namespace ERP.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task<bool> Add(string type, int status, int actionId, int siteId, int? employeeId);
        
        public Task<bool> Clear(int notificationId);
        Task Clear(string type, int actionId);
        public Task<List<Notification>> Get(string type, int status, int siteId);
        Task<List<Notification>> GetAllNotifications();
        Task<Notification> GetNotification(int id);
        Task<List<Notification>> GetUserNotifications();
    }
}
