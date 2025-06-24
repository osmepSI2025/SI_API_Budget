using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPAreaRepository
    {
        // Task<IEnumerable<ReturnPArea>> GetAllAsync(string? year, string? projectcode);
        Task<APIResponseDataReturnPAreaModels?> GetAllAsync(string? year, string? projectcode);
        Task AddRangeAsync(List<ReturnPArea> projects);
        Task<ReturnPArea> GetByIdAsync(int id);
        Task AddAsync(ReturnPArea entity);
        Task UpdateAsync(ReturnPArea entity);
        Task DeleteAsync(int id);
    }
}
