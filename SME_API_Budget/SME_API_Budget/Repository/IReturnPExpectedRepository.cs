using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IReturnPExpectedRepository
    {
        //Task<IEnumerable<ReturnPExpected>> GetAllAsync(string? year, string? projectcode);
        Task<ReturnPExpected> GetByIdAsync(int id);
        Task AddAsync(ReturnPExpected entity);
        Task UpdateAsync(ReturnPExpected entity);
        Task DeleteAsync(int id);
        Task<Dictionary<int, ReturnPExpectedApiResponse>> GetAllAsync(string year, string projectcode);
        Task AddRangeAsync(List<ReturnPExpected> projects);
        Task AddSubRangeAsync(List<ReturnPExpectedSub> projects);
        Task<List<int>> GetAllKeyIdsAsync();
    }
}
