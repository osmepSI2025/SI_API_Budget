using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;

namespace SME_API_Budget.Services
{
    public class ReturnPOutcomeService : IReturnPOutcomeService
    {
        private readonly IReturnPOutcomeRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
        public ReturnPOutcomeService(IReturnPOutcomeRepository repository, IApiInformationRepository repositoryApi, ICallAPIService serviceApi)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
        }

        //public async Task<Dictionary<int, ReturnPOutcomeModels>> GetAllAsync(string? year, string? projectcode)
        //    => await _repository.GetAllAsync(year, projectcode);

        public async Task<Dictionary<int, APIResponseDataReturnPOutComeModels>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, APIResponseDataReturnPOutComeModels>();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects.Any())
                {
                    return projects;
                  
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Outcome" });
                if (!LApi.Any())
                {
                    return projects;
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
                    return projects;
                }

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnPOutCome(apiParam, year, projectcode);

                if (resultApi.Data == null || resultApi.Data.Count == 0)
                {
                    return projects;
                }

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPOutcome> newProjects = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;



                    newProjects.Add(new ReturnPOutcome
                    {
                        KeyId = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        DataP3 = item.Value.DATA_P3,
                        DataP4 = Convert.ToDecimal(item.Value.DATA_P4),
                        DataP5 = item.Value.DATA_P5,

                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode
                    });
                }

                if (newProjects.Count > 0)
                    await _repository.AddRangeAsync(newProjects);

                // ✅ เรียกข้อมูลจาก DB หลัง Insert
                projects = await _repository.GetAllAsync(year, projectcode);
                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Dictionary<int, APIResponseDataReturnPOutComeModels>();
            }
            // => await _repository.GetAllAsync(year, pjcode);
        }

        public async Task<ReturnPOutcome> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPOutcome entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPOutcome entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
