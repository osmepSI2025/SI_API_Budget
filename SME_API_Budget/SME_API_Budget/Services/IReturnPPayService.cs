using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPPayService
    {
        Task<APIResponseDataReturnPPayModels> GetAllAsync(string? year, string? projectcode);
        Task<ReturnPPay> GetByIdAsync(int id);
        Task AddAsync(ReturnPPay entity);
        Task UpdateAsync(ReturnPPay entity);
        Task DeleteAsync(int id);
    }
}
