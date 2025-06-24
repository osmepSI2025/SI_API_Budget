using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Text.Json;

namespace SME_API_Budget.Services
{
    public class ReturnPExpectedService : IReturnPExpectedService
    {
        private readonly IReturnPExpectedRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
        public ReturnPExpectedService(IReturnPExpectedRepository repository, IApiInformationRepository repositoryApi, ICallAPIService serviceApi)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
        }

        //public async Task<IEnumerable<ReturnPExpected>> GetAllAsync(string? year, string? projectcode)

        //    => await _repository.GetAllAsync(year, projectcode);
        public async Task<Dictionary<int, ReturnPExpectedApiResponse>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, ReturnPExpectedApiResponse>();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects.Any())
                {
                    //return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Expected" });
                if (!LApi.Any())
                {
                    //return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
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
                //if (apiParam == null)
                //{
                //    return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                //}

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnExpected(apiParam, year, projectcode);


                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPExpected> newProjects = new();
                List<ReturnPExpectedSub> newProjectsSub = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;

                    // Add main entity
                    newProjects.Add(new ReturnPExpected
                    {
                        KeyId = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode
                    });

                    // Add sub entities
                    foreach (var itemsub in item.Value.SubData)
                    {
                        decimal dataPS1 = 0;
                        if (itemsub.Value.ValueKind == JsonValueKind.Number)
                        {
                            // Directly parse number
                            dataPS1 = itemsub.Value.GetDecimal();
                        }
                        else if (itemsub.Value.ValueKind == JsonValueKind.String && decimal.TryParse(itemsub.Value.GetString(), out decimal parsedValue))
                        {
                            // Handle string numbers (e.g., "5000")
                            dataPS1 = parsedValue;
                        }

                        newProjectsSub.Add(new ReturnPExpectedSub
                        {
                            SubCode = itemsub.Key,
                            KeyId = keyId,
                            DataPS1 = dataPS1
                        });
                    }
                }

                if (newProjects.Count > 0)
                    await _repository.AddRangeAsync(newProjects);

                if (newProjectsSub.Count > 0)
                    await _repository.AddSubRangeAsync(newProjectsSub);
              
                projects = await _repository.GetAllAsync(year, projectcode);
                //  return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Dictionary<int, ReturnPExpectedApiResponse>();
            }
            // => await _repository.GetAllAsync(year, pjcode);
        }

        public async Task<ReturnPExpected> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPExpected entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPExpected entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
