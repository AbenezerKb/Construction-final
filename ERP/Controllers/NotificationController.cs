using ERP.Context;
using ERP.DTOs.Others;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        [HttpGet("ProjectManagement")]
        public async Task<ActionResult<CustomApiResponse>> GetNotificationsByType([FromQuery] int siteId)
        {
            if (!_userService.UserRole.IsAdmin)
                return Forbid();

            List<Notification> deadlineNotififcations = await _notificationService.Get(NOTIFICATIONTYPE.TaskDeadline, -1, siteId);
            List<Notification> weeklyPlanNotifications = await _notificationService.Get(NOTIFICATIONTYPE.WeeklyTaskPlanSent, -1, siteId);
            List<Notification> taskCompletionNotifications = await _notificationService.Get(NOTIFICATIONTYPE.MainTaskCompletion, -1, siteId);


            List<Notification> allNotifications = new();
            allNotifications.AddRange(deadlineNotififcations);
            allNotifications.AddRange(weeklyPlanNotifications);
            allNotifications.AddRange(taskCompletionNotifications);

            return Ok(new CustomApiResponse
            {
                Message = "Success",
                Data = allNotifications.OrderByDescending(n => n.Date).ToList()
            });

        }
        [HttpGet("all")]
        public async Task<ActionResult<List<Notification>>> GetAll()
        {
            if (!_userService.UserRole.IsAdmin)
                return Forbid();

            List<Notification> notifications = await _notificationService.GetAllNotifications();

            return Ok(notifications);
        }



        [HttpGet]
        public async Task<ActionResult<List<Notification>>> Get()
        {
            List<Notification> notifications = await _notificationService.GetUserNotifications();

            return Ok(notifications);
        }

        [HttpGet("clear/{notificationId}")]
        public async Task<ActionResult<bool>> Clear(int notificationId)
        {
            var notification = await _notificationService.GetNotification(notificationId);

            if (_userService.UserRole.IsAdmin ||
                    _userService.Employee.EmployeeSiteId == notification.SiteId ||
                    _userService.Employee.EmployeeId == notification.EmployeeId)
                return Ok(await _notificationService.Clear(notificationId));

            return Forbid();

        }

        [HttpGet("now")]
        public async Task<ActionResult<string>> Now()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}
