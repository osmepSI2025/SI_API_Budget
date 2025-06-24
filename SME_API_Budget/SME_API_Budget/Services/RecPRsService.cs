using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;

namespace SME_API_Budget.Services
{
    public class RecPRsService : IRecPRsService
    {
        private readonly IRecPRsRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;

        public RecPRsService(IRecPRsRepository repository, IApiInformationRepository repositoryApi, ICallAPIService serviceApi)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
        }

        public async Task<IEnumerable<RecPR>> GetAllRecPRsAsync()
            => await _repository.GetAllAsync();

        public async Task<RecPR> GetRecPRsByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddRecPRsAsync(RecPR rec)
            => await _repository.AddAsync(rec);

        public async Task UpdateRecPRsAsync(RecPR rec)
            => await _repository.UpdateAsync(rec);

        public async Task DeleteRecPRsAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<ApiRecPRResponseModel> SendDataAsync(RecPRSubDataModel Senddata, string year, string projectcode, string? ref_code)
        {
            try
            {
                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Rec_P_Rs" });
                if (!LApi.Any())
                {
                    return null;
                }
                var apiParam = LApi.Select(x => new MapiInformationModels
                {
                    ServiceNameCode = x.ServiceNameCode,
                    ApiKey = x.ApiKey,
                    AuthorizationType = x.AuthorizationType,
                    ContentType = x.ContentType,
                    CreateDate = x.CreateDate,
                    Id = x.Id,
                    MethodType = x.MethodType,
                    ServiceNameTh = x.ServiceNameTh,
                    Urldevelopment = x.Urldevelopment,
                    Urlproduction = x.Urlproduction,
                    Username = x.Username,
                    Password = x.Password,
                    UpdateDate = x.UpdateDate
                }).First(); // ดึงตัวแรกของ List
                if (apiParam == null)
                {
                    return null;
                }

                var resultApi = await _serviceApi.RecDataApiAsync_RecPRs(apiParam, year, projectcode,ref_code, Senddata);
           
                return resultApi;
            }
            catch (Exception ex)
            {
                return null;
            }




        }
    }
}
