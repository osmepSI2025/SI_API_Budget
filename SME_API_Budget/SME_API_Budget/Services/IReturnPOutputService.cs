using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnPOutputService
    {
        Task<Dictionary<int, APIResponseDataReturnPOutputModels>> GetAllAsync(string year,string projectCode);     
        Task<ReturnPOutput> GetByIdAsync(int id);
        Task AddAsync(ReturnPOutput entity);
        Task UpdateAsync(ReturnPOutput entity);
        Task DeleteAsync(int id);
    }
}
