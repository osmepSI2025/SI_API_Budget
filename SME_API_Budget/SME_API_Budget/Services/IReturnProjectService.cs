using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IReturnProjectService
    {
      
        Task<Dictionary<int, APIResponseReturnProjectModels>> GetAllAsync(string? year, string? projectCode);
        Task<ReturnProject> GetByIdAsync(int id);
        Task<int> BatchAllAsync(string year);
        Task AddAsync(ReturnProject entity);
        Task UpdateAsync(ReturnProject entity);
        Task DeleteAsync(int id);
    }
}
