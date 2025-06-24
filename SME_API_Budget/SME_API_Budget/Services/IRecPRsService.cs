using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IRecPRsService
    {
        Task<IEnumerable<RecPR>> GetAllRecPRsAsync();
        Task<RecPR> GetRecPRsByIdAsync(int id);
        Task AddRecPRsAsync(RecPR rec);
        Task UpdateRecPRsAsync(RecPR rec);
        Task DeleteRecPRsAsync(int id);
        Task<ApiRecPRResponseModel> SendDataAsync(RecPRSubDataModel data, string year, string projectcode,string? ref_code);
    }
}
