using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Globalization;

namespace SME_API_Budget.Services
{
    public class ReturnPOutputService : IReturnPOutputService
    {
        private readonly IReturnPOutputRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
        private readonly IReturnProjectService _returnProjectService;

        public ReturnPOutputService(IReturnPOutputRepository repository, IApiInformationRepository repositoryApi,
            ICallAPIService serviceApi, IReturnProjectService returnProjectService)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
            _returnProjectService = returnProjectService;
        }
        public async Task<Dictionary<int, APIResponseDataReturnPOutputModels>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, APIResponseDataReturnPOutputModels>();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects.Any())
                {
                    // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_output" });
                if (!LApi.Any())
                {
                   // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
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
                    // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnPOutput(apiParam, year, projectcode);

                if (resultApi.Data == null || resultApi.Data.Count == 0)
                {
                    // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPOutput> newProjects = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;

          

                    newProjects.Add(new ReturnPOutput
                    {
                        KeyId = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        DataP3 = item.Value.DATA_P3,
                       DataP4 = Convert.ToDecimal( item.Value.DATA_P4),
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
                // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Dictionary<int, APIResponseDataReturnPOutputModels>();
            }
             // => await _repository.GetAllAsync(year, pjcode);
        }
      

        public async Task<ReturnPOutput> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPOutput entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPOutput entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<int> Batch_Return_Output()
        {
            var thaiCulture = new CultureInfo("th-TH");
            var buddhistCalendar = new ThaiBuddhistCalendar();

            var currentYear = buddhistCalendar.GetYear(DateTime.Now);

            var years = Enumerable.Range(currentYear - 4, 5).Reverse();

            foreach (var year in years)
            {
                var Lprojects = await _returnProjectService.GetAllAsync(year.ToString(), "");

                foreach (var item in Lprojects)
                {
                    var result = await GetAllAsync(year.ToString(), item.Value.DATA_P11);
                }


            }
            return 1; // Placeholder for delete operation, implement as needed
        }
    }
}
