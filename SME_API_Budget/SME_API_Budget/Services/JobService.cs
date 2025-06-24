using System.Globalization;

namespace SME_API_Budget.Services
{
    public class JobService : IJobService
    {
        private readonly ILogger<JobService> _logger;
        private readonly IReturnProjectService _projectService;

        public JobService(ILogger<JobService> logger, IReturnProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public async Task RunJobAsync(string jobName)
        {
            _logger.LogInformation($"✅ Running Job: {jobName}");
            var thaiCulture = new CultureInfo("th-TH");
            string thaiDate = DateTime.Now.ToString("yyyy", thaiCulture);
            switch (jobName)
            {
                case "Return_Project":
                    await _projectService.BatchAllAsync(thaiDate);
                    break;
                case "Return_P_Area":
                  //  await _projectService.SaveReturnPAreaAsync();
                    break;
                case "Return_P_output":
                  //  await _projectService.SaveReturnPOutputAsync();
                    break;
                case "Return_P_Outcome":
                   // await _projectService.SaveReturnPOutcomeAsync();
                    break;
                case "Return_P_Expected":
                    //await _projectService.SaveReturnPExpectedAsync();
                    break;
                case "Return_P_Activity":
                   // await _projectService.SaveReturnPActivityAsync();
                    break;
                case "Return_P_Plan_Bdg":
                   // await _projectService.SaveReturnPPlanBdgAsync();
                    break;
                case "Return_P_Risk":
                   // await _projectService.SaveReturnPRiskAsync();
                    break;
                case "Return_P_Pay":
                  //  await _projectService.SaveReturnPPayAsync();
                    break;
                default:
                    _logger.LogWarning($"⚠️ Job not found: {jobName}");
                    break;
            }
        }
    }
}
