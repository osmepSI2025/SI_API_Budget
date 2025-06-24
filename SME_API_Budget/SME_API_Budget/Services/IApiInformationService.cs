using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface IApiInformationService
    {
        Task<IEnumerable<MapiInformation>> GetAllServicesAsync(MapiInformationModels apiModels);
        Task<MapiInformation> GetServiceByIdAsync(int id);
        Task AddServiceAsync(MapiInformation service);
        Task UpdateServiceAsync(MapiInformation service);
        Task DeleteServiceAsync(int id);
    }
}
