using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME_API_Budget.Services;
using SME_API_SMEBudget.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ScheduledJobService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduledJobService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SMEBudgetDBContext>();
                var now = DateTime.Now;

                var jobs = await db.MscheduledJobs
                   .Where(j => j.IsActive == true && j.RunHour == now.Hour && j.RunMinute == now.Minute)
                    .ToListAsync(stoppingToken);

                foreach (var job in jobs)
                {
                    _ = RunJobAsync(job.JobName, scope.ServiceProvider);
                }
            }

            // Wait until the next minute
            var delay = 60000 - (DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);
            await Task.Delay(delay, stoppingToken);
        }

    }

    private async Task RunJobAsync(string jobName, IServiceProvider serviceProvider)
    {
        switch (jobName)
        {
            case "Return_Project":
                await serviceProvider.GetRequiredService<ReturnProjectService>().BatchEndOfDayAllAsync();
                break;
            case "Return_P_Area":
                await serviceProvider.GetRequiredService<ReturnPAreaService>().BatchP_Area();
                break;
            case "Return_P_output":
                await serviceProvider.GetRequiredService<ReturnPOutputService>().Batch_Return_Output();
                break;
            case "Return_P_Outcome":
                await serviceProvider.GetRequiredService<ReturnPOutcomeService>().BatchReturn_Outcome();
                break;
            case "Return_P_Expected":
                await serviceProvider.GetRequiredService<ReturnPExpectedService>().BatchReturn_Outcome();
                break;
            case "Return_P_Activity":
                await serviceProvider.GetRequiredService<ReturnPActivityService>().BatchReturn_Activity();
                break;
            case "Return_P_Plan_Bdg":
                await serviceProvider.GetRequiredService<ReturnPPlanBdgService>().BatchReturn_PlanBdg();
                break;
            case "Return_P_Risk":
                await serviceProvider.GetRequiredService<ReturnPRiskService>().BatchReturn_Risk();
                break;
            case "Return_P_Pay":
                await serviceProvider.GetRequiredService<ReturnPPayService>().BatchReturn_Pay();
                break;
            default:
                // Optionally log unknown job
                break;
        }
    }

}