using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPRiskRepository
    {
        Task<Dictionary<int, APIResponseDataReturnPRiskModels>> GetAllAsync(string year, string projectcode);
        Task AddRangeAsync(List<ReturnPRisk> projects);
        Task<List<int>> GetAllKeyIdsAsync();
        Task<ReturnPRisk> GetByIdAsync(int id);
        Task AddAsync(ReturnPRisk entity);
        Task UpdateAsync(ReturnPRisk entity);
        Task DeleteAsync(int id);
    }
}
