namespace SME_API_Budget.Services
{
    public interface IJobService
    {
        Task RunJobAsync(string jobName);
    }
}
