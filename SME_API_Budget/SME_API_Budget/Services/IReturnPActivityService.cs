using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPActivityService
    {
        Task<IEnumerable<ReturnPActivity>> GetAllAsync(string year, string? projectCode = null);
        Task<ReturnPActivity?> GetByIdAsync(int? refCode);
        Task AddAsync(ReturnPActivity activity);
        Task UpdateAsync(ReturnPActivity activity);
        Task DeleteAsync(int? refCode);
    }
}
