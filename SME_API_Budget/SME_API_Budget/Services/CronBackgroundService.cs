using Microsoft.EntityFrameworkCore;
using SME_API_SMEBudget.Entities;
using System.Globalization;

namespace SME_API_Budget.Services
{
    public class CronBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CronBackgroundService> _logger;

        public CronBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CronBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        using var scope = _scopeFactory.CreateScope();
        //        var dbContext = scope.ServiceProvider.GetRequiredService<SMEBudgetDBContext>();

        //        var now = DateTime.Now;
        //        var scheduledJobs = await dbContext.MscheduledJobs
        //            .Where(j => j.RunHour == now.Hour && j.RunMinute == now.Minute && j.IsActive==true)
        //            .ToListAsync(stoppingToken);

        //        foreach (var job in scheduledJobs)
        //        {
        //            _logger.LogInformation($"Executing job: {job.JobName} at {now}");

        //            if (job.JobName == "Return_Project")
        //            {
        //                var thaiCulture = new CultureInfo("th-TH");
        //                string thaiDate = DateTime.Now.ToString("yyyy", thaiCulture);
        //                var projectService = scope.ServiceProvider.GetRequiredService<IReturnProjectService>();
        //                await projectService.BatchAllAsync(thaiDate);
        //            }
        //        }

        //        // ✅ รอ 1 นาที ก่อนตรวจสอบใหม่
        //        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        //    }
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SMEBudgetDBContext>();

                var now = DateTime.Now;
                var scheduledJobs = await dbContext.MscheduledJobs
                    .Where(j => j.RunHour == now.Hour && j.RunMinute == now.Minute && j.IsActive == true)
                    .ToListAsync(stoppingToken);

                foreach (var job in scheduledJobs)
                {
                    _logger.LogInformation($"⏳ Running Job: {job.JobName} at {now}");

                    var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
                    await jobService.RunJobAsync(job.JobName);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // ✅ รอ 1 นาที ก่อนตรวจสอบใหม่
            }
        }
    }

}
