using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnProjectRepository
    {
        // Task<List<ReturnProjectModels>> GetAllAsync(string? year, string? projectCode);
        Task<Dictionary<int, APIResponseReturnProjectModels>> GetAllAsync(string? year, string? projectCode);
       // Task<ReturnProject?> GetByKeyIdAsync(string KeyId);
        Task<List<int>> GetAllKeyIdsAsync();
        Task AddRangeAsync(List<ReturnProject> projects);
        Task<ReturnProject> GetByIdAsync(int id);
        Task AddAsync(ReturnProject entity);
        Task UpdateAsync(ReturnProject entity);
        Task DeleteAsync(int id);
    }
}
