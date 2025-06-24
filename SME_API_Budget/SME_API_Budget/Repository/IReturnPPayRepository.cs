using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPPayRepository
    {
        Task<APIResponseDataReturnPPayModels> GetAllAsync(string? year, string? projectcode);
        Task AddRangeAsync(List<ReturnPPay> projects);
        Task<ReturnPPay> GetByIdAsync(int id);
        Task AddAsync(ReturnPPay entity);
        Task UpdateAsync(ReturnPPay entity);
        Task DeleteAsync(int id);
    }
}
