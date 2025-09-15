using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.Matchers;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;

using SME_API_Budget.Services;
using SME_API_SMEBudget.Entities;

public class ScheduledJobPuller : IJob
{
    private readonly SMEBudgetDBContext _dbContext;
    private readonly ILogger<ScheduledJobPuller> _logger;

    private readonly IReturnProjectService _returnProjectService;
    private readonly IReturnPAreaService _returnPAreaService;
    private readonly IReturnPOutputService _returnPOutputService;

    private readonly IReturnPOutcomeService _returnPOutcomeService;
    private readonly IReturnPExpectedService  _returnPExpectedService;
    private readonly IReturnPActivityService _returnPActivityService;

    private readonly IReturnPPlanBdgService _returnPPlanBdgService;
    private readonly IReturnPRiskService  _returnPRiskService;
    private readonly IReturnPPayService  _returnPPayService;
    private readonly IServiceProvider _serviceProvider;
    public ScheduledJobPuller(
        SMEBudgetDBContext dbContext,
        ILogger<ScheduledJobPuller> logger,
        IServiceProvider serviceProvider,
        IReturnProjectService returnProjectService,
        IReturnPAreaService returnPAreaService,
        IReturnPOutputService returnPOutputService
        ,
        IReturnPOutcomeService returnPOutcomeService
        ,
        IReturnPExpectedService returnPExpectedService
        ,
        IReturnPActivityService returnPActivityService,
        IReturnPPlanBdgService returnPPlanBdgService,
         
        IReturnPRiskService returnPRiskService
,
        IReturnPPayService returnPPayService


        )
    {
        _dbContext = dbContext;
        _logger = logger;

        _logger.LogInformation("ScheduledJobPuller started.");
        _returnProjectService = returnProjectService;
        _returnPAreaService = returnPAreaService;
        _returnPOutputService = returnPOutputService;
        _returnPOutcomeService = returnPOutcomeService;
        _returnPExpectedService = returnPExpectedService;
        _returnPActivityService = returnPActivityService;
        _returnPPlanBdgService = returnPPlanBdgService;
        _returnPRiskService = returnPRiskService;
        _returnPPayService = returnPPayService;
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // สร้าง scope ใหม่สำหรับ Job นี้
        using (var scope = _serviceProvider.CreateScope())
        {
            // ดึงค่า jobName จาก JobDataMap
            var jobName = context.JobDetail.JobDataMap.GetString("JobName");
            _logger.LogInformation($"Executing job: {jobName}");

            try
            {
                var serviceProvider = scope.ServiceProvider;
                switch (jobName)
                {
                    case "Return_Project":
                        int currentYear = DateTime.Now.Year;
                        int currentYearTh;
                        if (currentYear < 2500)
                        {
                            currentYearTh = currentYear + 543;
                        }
                        else
                        {
                            currentYearTh = currentYear;
                        }
                        int currentYearThto  = currentYearTh+1;
                        // วนลูปตั้งแต่ currentYearTh ถึง currentYearTh+1
                        for (int year = currentYearTh; year <= currentYearThto; year++)
                        {
                            await _returnProjectService.BatchAllAsync(year.ToString());
                        }
                       
                        break;
                    case "Return_P_Area":
                        await _returnPAreaService.BatchP_Area();
                        break;
                    case "Return_P_output":
                        await _returnPOutputService.Batch_Return_Output();
                        break;
                    case "Return_P_Outcome":
                        await _returnPOutcomeService.BatchReturn_Outcome();
                        break;
                    case "Return_P_Expected":
                        await _returnPExpectedService.BatchReturn_Expected();
                        break;
                    case "Return_P_Activity":
                        await _returnPActivityService.BatchReturn_Activity();
                        break;
                    case "Return_P_Plan_Bdg":
                        await _returnPPlanBdgService.BatchReturn_PlanBdg();
                        break;
                    case "Return_P_Risk":
                        await _returnPRiskService.BatchReturn_Risk();
                        break;
                    case "Return_P_Pay":
                        await _returnPPayService.BatchReturn_Pay();
                        break;
                    default:
                        // Optionally log unknown job
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing job {jobName}.");
            }
        }
    }

    private async Task RunJobAsync(string jobName, CancellationToken cancellationToken)
    {
        switch (jobName)
        {
            case "Return_Project":
                string yearth = DateTime.Now.Year.ToString();
                await _returnProjectService.BatchAllAsync("2568");
                break;
            case "Return_P_Area":
                await _returnPAreaService.BatchP_Area();
                break;
            case "Return_P_output":
                await _returnPOutputService.Batch_Return_Output();
                break;
            case "Return_P_Outcome":
                await _returnPOutcomeService.BatchReturn_Outcome();
                break;
            case "Return_P_Expected":
                await _returnPExpectedService.BatchReturn_Expected();
                break;
            case "Return_P_Activity":
                await _returnPActivityService.BatchReturn_Activity();
                break;
            case "Return_P_Plan_Bdg":
                await _returnPPlanBdgService.BatchReturn_PlanBdg();
                break;
            case "Return_P_Risk":
                await _returnPRiskService.BatchReturn_Risk();
                break;
            case "Return_P_Pay":
                await _returnPPayService.BatchReturn_Pay();
                break;
            default:
                // Optionally log unknown job
                break;
        }
    }
}

public class JobSchedulerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobSchedulerService> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public JobSchedulerService(IServiceProvider serviceProvider, ILogger<JobSchedulerService> logger, ISchedulerFactory schedulerFactory)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("JobSchedulerService is starting.");
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SMEBudgetDBContext>();
            var jobs = await dbContext.MscheduledJobs.Where(j => j.IsActive == true).ToListAsync(cancellationToken);

            // Clear all triggers in the "dynamic" group before scheduling
            var allScheduledJobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("dynamic"));
            foreach (var key in allScheduledJobKeys)
            {
                var triggers = await scheduler.GetTriggersOfJob(key, cancellationToken);
                foreach (var trigger in triggers)
                {
                    await scheduler.UnscheduleJob(trigger.Key, cancellationToken);
                    _logger.LogInformation($"Trigger '{trigger.Key.Name}' for job '{key.Name}' deleted.");
                }
            }

            foreach (var job in jobs)
            {
                // แก้ไข: เพิ่มการตรวจสอบค่าว่างเปล่า (whitespace)
                if (!int.TryParse(job.RunMinute.ToString(), out _) || !int.TryParse(job.RunHour.ToString(), out _))
                {
                    _logger.LogError($"Job '{job.JobName}' has invalid RunMinute or RunHour. Skipping.");
                    continue;
                }
                string cron = $"0 {job.RunMinute} {job.RunHour} * * ?";
                var jobKey = new JobKey(job.JobName, "dynamic");

                // ตรวจสอบว่า Job มีอยู่แล้วหรือไม่
                if (await scheduler.CheckExists(jobKey, cancellationToken))
                {
                    _logger.LogInformation($"Job '{job.JobName}' already exists. Rescheduling with new trigger.");

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"{job.JobName}-trigger", "dynamic")
                        .WithCronSchedule(cron)
                        .Build();

                    await scheduler.RescheduleJob(trigger.Key, trigger, cancellationToken);
                }
                else
                {
                    _logger.LogInformation($"Job '{job.JobName}' does not exist. Creating a new one.");

                    var jobDetail = JobBuilder.Create<ScheduledJobPuller>()
                        .WithIdentity(jobKey)
                        .UsingJobData("JobName", job.JobName)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"{job.JobName}-trigger", "dynamic")
                        .WithCronSchedule(cron)
                        .Build();

                    await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("JobSchedulerService is stopping.");
        return Task.CompletedTask;
    }
}