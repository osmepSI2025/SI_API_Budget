using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Repository
{
    public interface IApiInformationRepository
    {
        Task<IEnumerable<MapiInformation>> GetAllAsync(MapiInformationModels param);
        Task<MapiInformation> GetByIdAsync(int id);
        Task AddAsync(MapiInformation service);
        Task UpdateAsync(MapiInformation service);
        Task DeleteAsync(int id);
    }
}
