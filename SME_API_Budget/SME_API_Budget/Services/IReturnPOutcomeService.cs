using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPOutcomeService
    {
        Task<Dictionary<int, APIResponseDataReturnPOutComeModels>> GetAllAsync(string year, string projectCode);
        Task<ReturnPOutcome> GetByIdAsync(int id);
        Task AddAsync(ReturnPOutcome entity);
        Task UpdateAsync(ReturnPOutcome entity);
        Task DeleteAsync(int id);
    }
}
