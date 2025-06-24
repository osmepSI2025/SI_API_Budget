using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;

namespace SME_API_Budget.Services
{
    public class ApiInformationService :IApiInformationService
    {
        private readonly IApiInformationRepository _repository;

        public ApiInformationService(IApiInformationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MapiInformation>> GetAllServicesAsync(MapiInformationModels apiModels)
            => await _repository.GetAllAsync(apiModels);

        public async Task<MapiInformation> GetServiceByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddServiceAsync(MapiInformation service)
            => await _repository.AddAsync(service);

        public async Task UpdateServiceAsync(MapiInformation service)
            => await _repository.UpdateAsync(service);

        public async Task DeleteServiceAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
