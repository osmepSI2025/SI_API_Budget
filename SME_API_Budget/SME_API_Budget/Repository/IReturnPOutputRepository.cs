using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPOutputRepository
    {
        // Task<IEnumerable<ReturnPOutput>> GetAllAsync(string year,string pjcode);
        Task<Dictionary<int, APIResponseDataReturnPOutputModels>> GetAllAsync(string year, string projectcode);
        Task AddRangeAsync(List<ReturnPOutput> projects);
        Task<List<int>> GetAllKeyIdsAsync();
        Task<ReturnPOutput> GetByIdAsync(int id);
    
        Task AddAsync(ReturnPOutput entity);
        Task UpdateAsync(ReturnPOutput entity);
        Task DeleteAsync(int id);
    }
}
