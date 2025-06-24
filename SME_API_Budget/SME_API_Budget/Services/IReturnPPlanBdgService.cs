using SME_API_Budget.Entities;

namespace SME_API_Budget.Services
{
    public interface IReturnPPlanBdgService
    {
        Task<IEnumerable<ReturnPPlanBdg>> GetAllAsync(string? year, string? projectcode);
        Task<ReturnPPlanBdg> GetByIdAsync(int id);
        Task AddAsync(ReturnPPlanBdg entity);
        Task UpdateAsync(ReturnPPlanBdg entity);
        Task DeleteAsync(int id);
    }
}
