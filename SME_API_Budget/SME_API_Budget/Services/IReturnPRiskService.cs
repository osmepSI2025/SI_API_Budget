using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPRiskService
    {
        Task<Dictionary<int, APIResponseDataReturnPRiskModels>> GetAllAsync(string year, string projectCode);
        Task<ReturnPRisk> GetByIdAsync(int id);
        Task AddAsync(ReturnPRisk entity);
        Task UpdateAsync(ReturnPRisk entity);
        Task DeleteAsync(int id);
        Task<int> BatchReturn_Risk();
    }
}
