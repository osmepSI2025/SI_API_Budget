using SME_API_Budget.Entities;

namespace SME_API_Budget.Repository
{
    public interface IReturnPActivityRepository
    {
        Task<IEnumerable<ReturnPActivity>> GetAllAsync(string? year, string? projectcode);

        Task AddRangeAsync(List<ReturnPActivity> projects);
        Task AddRangeAsyncSub(List<ReturnPActivitySub> projectsSub);
        Task<List<int>> GetAllKeyIdsAsync();
        Task<ReturnPActivity> GetByIdAsync(int id);
        Task AddAsync(ReturnPActivity entity);
        Task UpdateAsync(ReturnPActivity entity);
        Task DeleteAsync(int id);
    }
}
