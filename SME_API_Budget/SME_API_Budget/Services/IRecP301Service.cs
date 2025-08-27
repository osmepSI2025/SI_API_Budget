using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IRecP301Service
    {
        Task<IEnumerable<RecPR>> GetAllRecPRsAsync();
        Task<RecPR> GetRecPRsByIdAsync(int id);
        Task AddRecPRsAsync(RecPR rec);
        Task UpdateRecPRsAsync(RecPR rec);
        Task DeleteRecPRsAsync(int id);
        Task<ApiRecP301ResponseModel> SendDataAsync(RecP301Models data);
    }
}
