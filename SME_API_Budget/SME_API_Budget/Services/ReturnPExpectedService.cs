using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Globalization;
using System.Text.Json;

namespace SME_API_Budget.Services
{
    public class ReturnPExpectedService : IReturnPExpectedService
    {
        private readonly IReturnPExpectedRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
        private readonly IReturnProjectService _returnProjectService;
        public ReturnPExpectedService(IReturnPExpectedRepository repository, IApiInformationRepository repositoryApi,
            ICallAPIService serviceApi, IReturnProjectService returnProjectService)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
            _returnProjectService = returnProjectService;
        }

        public async Task<Dictionary<int, ReturnPExpectedApiResponse>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, ReturnPExpectedApiResponse>();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects.Any())
                {
                    return projects;
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Expected" });
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
                }).First();

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnExpected(apiParam, year, projectcode);

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPExpected> newProjects = new();

                foreach (var item in resultApi.Data)
                {
                    //if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                    //    continue;

                    var mainEntity = new ReturnPExpected
                    {
                        KeyId = int.Parse(item.Key),
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode
                    };

                    if (item.Value.SubData != null)
                    {
                        foreach (var itemsub in item.Value.SubData)
                        {
                            decimal dataPS1 = 0;
                            if (itemsub.Value.ValueKind == JsonValueKind.Number)
                            {
                                dataPS1 = itemsub.Value.GetDecimal();
                            }
                            else if (itemsub.Value.ValueKind == JsonValueKind.String && decimal.TryParse(itemsub.Value.GetString(), out decimal parsedValue))
                            {
                                dataPS1 = parsedValue;
                            }

                            mainEntity.ReturnPExpectedSubs.Add(new ReturnPExpectedSub
                            {
                                SubCode = itemsub.Key,
                                KeyId = int.Parse(item.Key),
                                DataPS1 = dataPS1
                               
                            });
                        }
                    }

                    newProjects.Add(mainEntity);
                }

                if (newProjects.Count > 0)
                {
                    try
                    {
                        await _repository.AddRangeAsync(newProjects);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in AddRangeAsync: " + ex.ToString());
                        throw;
                    }
                }

                projects = await _repository.GetAllAsync(year, projectcode);
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


        public async Task<int> BatchReturn_Expected()
        {
            var thaiCulture = new CultureInfo("th-TH");
            var buddhistCalendar = new ThaiBuddhistCalendar();

            var currentYear = buddhistCalendar.GetYear(DateTime.Now);

            var years = new[] { currentYear - 1, currentYear + 1 };


            foreach (var year in years)
            {
                var Lprojects = await _returnProjectService.GetAllAsync(year.ToString(), "");

                foreach (var item in Lprojects)
                {
                    if (item.Value.DATA_P11.Length > 5)
                    {
                        var result = await GetAllAsync(year.ToString(), item.Value.DATA_P11);
                    }
                }


            }
            return 1; // Placeholder for delete operation, implement as needed
        }
    }
}
