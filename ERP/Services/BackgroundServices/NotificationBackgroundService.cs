using ERP.Models;
using ERP.Services.SettingService;
using ERP.Services.SubTaskService;

namespace ERP.Services.BackgroundServices
{
    public class NotificationBackgroundService : IHostedService
    {
        Timer? timer;

        private readonly IServiceScopeFactory scopeFactory;

        public NotificationBackgroundService(IServiceScopeFactory factory)
        {
            scopeFactory = factory;

        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
            var firstInterval = nextRunTime.Subtract(DateTime.Now);

            Task.Run(() =>
            {
                var t1 = Task.Delay(firstInterval);
                t1.Wait();
                CheckForTaskDeadlineAsync(null);
                timer = new Timer(
                    CheckForTaskDeadlineAsync,
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromHours(24)
                );
            });
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private async void CheckForTaskDeadlineAsync(object? state)
        {
            List<SubTask> upComingTasks;
            //TODO:Remove
            Console.WriteLine("Checking for deadlines");
            using (var scope = scopeFactory.CreateScope())
            {
                var subTaskService = scope.ServiceProvider.GetRequiredService<ISubTaskService>();
                var settingsService = scope.ServiceProvider.GetRequiredService<ISettingsService>();

                var setting = await settingsService.GetByName("DeadlineNotificationDay");
                upComingTasks = await subTaskService.GetUpComming(int.Parse(setting.Value));
            }

            //Check for tasks closer to deadline

            var now = DateTime.Now;
            upComingTasks.ForEach(st =>
            {
                //TODO add the following message to notifications table
                Console.WriteLine($"Deadline Alert: Only {st.EndDate.Subtract(now).Days} Day(s) left for the task '{st.Name}' to be completed");
            });
        }
    }
}