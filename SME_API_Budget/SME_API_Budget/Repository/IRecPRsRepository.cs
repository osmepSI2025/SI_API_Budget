using SME_API_Budget.Entities;

namespace SME_API_Budget.Repository
{
    public interface IRecPRsRepository
    {
        Task<IEnumerable<RecPR>> GetAllAsync();
        Task<RecPR> GetByIdAsync(int id);
        Task AddAsync(RecPR rec);
        Task UpdateAsync(RecPR rec);
        Task DeleteAsync(int id);
    }
}
