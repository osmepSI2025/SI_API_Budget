using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPExpectedService
    {
  
        Task<Dictionary<int, ReturnPExpectedApiResponse>> GetAllAsync(string year, string projectCode);
        Task<ReturnPExpected> GetByIdAsync(int id);
        Task AddAsync(ReturnPExpected entity);
        Task UpdateAsync(ReturnPExpected entity);
        Task DeleteAsync(int id);
    }
}
