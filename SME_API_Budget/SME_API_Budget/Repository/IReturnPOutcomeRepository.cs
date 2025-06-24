using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPOutcomeRepository
    {
        //Task<IEnumerable<APIResponseDataReturnPOutComeModels>> GetAllAsync(string? year, string? projectcode);
        Task<Dictionary<int, APIResponseDataReturnPOutComeModels>> GetAllAsync(string year, string projectcode);
        Task AddRangeAsync(List<ReturnPOutcome> projects);
        Task<List<int>> GetAllKeyIdsAsync();
        Task<ReturnPOutcome> GetByIdAsync(int id);
        Task AddAsync(ReturnPOutcome entity);
        Task UpdateAsync(ReturnPOutcome entity);
        Task DeleteAsync(int id);
    }
}
