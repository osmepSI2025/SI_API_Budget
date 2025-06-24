using SME_API_Budget.Entities;

namespace SME_API_Budget.Repository
{
    public interface IReturnPPlanBdgRepository
    {
        Task<IEnumerable<ReturnPPlanBdg>> GetAllAsync(string? year, string? projectcode);
        Task AddRangeAsync(List<ReturnPPlanBdg> projects);
        Task AddRangeAsyncSub(List<ReturnPPlanBdgSub> projectsSub);
        Task<List<int>> GetAllKeyIdsAsync();
        Task<ReturnPPlanBdg> GetByIdAsync(int id);
        Task AddAsync(ReturnPPlanBdg entity);
        Task UpdateAsync(ReturnPPlanBdg entity);
        Task DeleteAsync(int id);
    }
}
