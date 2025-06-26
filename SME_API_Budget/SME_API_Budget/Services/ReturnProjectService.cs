using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SME_API_Budget.Services
{
    public class ReturnProjectService : IReturnProjectService
    {
        private readonly IReturnProjectRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;

        public ReturnProjectService(IReturnProjectRepository repository, IApiInformationRepository repositoryApi, ICallAPIService serviceApi)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
        }
        public async Task<Dictionary<int, APIResponseReturnProjectModels>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, APIResponseReturnProjectModels>();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects.Any())
                {
                    // return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_Project" });
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
                  //  return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnProject(apiParam, year, projectcode);

                if (resultApi.Data == null || resultApi.Data.Count == 0)
                {
                    //  return projects.ToDictionary(p => p.Value.KeyId ?? p.Key, p => p.Value);
                    return projects;
                }

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnProject> newProjects = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;

                    //DateTime.TryParseExact(item.Value.DATA_P4, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP4);
                    //DateTime.TryParseExact(item.Value.DATA_P5, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP5);
                    //decimal.TryParse(item.Value.DATA_P12, out decimal dataP12);
                    //decimal.TryParse(item.Value.DATA_P13, out decimal dataP13);

                    newProjects.Add(new ReturnProject
                    {
                        KeyId = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        DataP3 = item.Value.DATA_P3,
                        DataP4 = item.Value.DATA_P4,
                        DataP5 = item.Value.DATA_P5,
                        DataP6 = item.Value.DATA_P6,
                        DataP7 = item.Value.DATA_P7,
                        DataP8 = item.Value.DATA_P8,
                        DataP9 = item.Value.DATA_P9,
                        DataP10 = item.Value.DATA_P10,
                        DataP11 = item.Value.DATA_P11,
                        DataP12 = item.Value.DATA_P12,
                        DataP13 = item.Value.DATA_P13,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = item.Value.DATA_P11
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
                return new Dictionary<int, APIResponseReturnProjectModels>();
            }
        }


        public async Task<int> BatchAllAsync(string year)
        {
         

            try
            {
                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_Project" });
               

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
                var resultApi = await _serviceApi.GetDataApiAsync_ReturnProject(apiParam, year,"");

           
                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnProject> newProjects = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;

                    //DateTime.TryParseExact(item.Value.DATA_P4, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP4);
                    //DateTime.TryParseExact(item.Value.DATA_P5, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP5);
                    //decimal.TryParse(item.Value.DATA_P12, out decimal dataP12);
                    //decimal.TryParse(item.Value.DATA_P13, out decimal dataP13);

                    newProjects.Add(new ReturnProject
                    {
                        KeyId = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        DataP3 = item.Value.DATA_P3,
                        DataP4 = item.Value.DATA_P4,
                        DataP5 = item.Value.DATA_P5,
                        DataP6 = item.Value.DATA_P6,
                        DataP7 = item.Value.DATA_P7,
                        DataP8 = item.Value.DATA_P8,
                        DataP9 = item.Value.DATA_P9,
                        DataP10 = item.Value.DATA_P10,
                        DataP11 = item.Value.DATA_P11,
                        DataP12 = item.Value.DATA_P12,
                        DataP13 = item.Value.DATA_P13,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = item.Value.DATA_P11
                    });
                }

                if (newProjects.Count > 0)
                    await _repository.AddRangeAsync(newProjects);

           
               
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
        }



        public async Task<ReturnProject> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnProject entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnProject entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);


        public async Task<int> BatchEndOfDayAllAsync()
        {

            var thaiCulture = new CultureInfo("th-TH");
            var buddhistCalendar = new ThaiBuddhistCalendar();

            var currentYear = buddhistCalendar.GetYear(DateTime.Now);

            var years = Enumerable.Range(currentYear - 4, 5).Reverse();

            var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_Project" });


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
            foreach (var year in years)
            {
                try
                {
              
                    var resultApi = await _serviceApi.GetDataApiAsync_ReturnProject(apiParam, year.ToString(), "");


                    var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                    List<ReturnProject> newProjects = new();

                    foreach (var item in resultApi.Data)
                    {
                        if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                            continue;



                        newProjects.Add(new ReturnProject
                        {
                            KeyId = keyId,
                            DataP1 = item.Value.DATA_P1,
                            DataP2 = item.Value.DATA_P2,
                            DataP3 = item.Value.DATA_P3,
                            DataP4 = item.Value.DATA_P4,
                            DataP5 = item.Value.DATA_P5,
                            DataP6 = item.Value.DATA_P6,
                            DataP7 = item.Value.DATA_P7,
                            DataP8 = item.Value.DATA_P8,
                            DataP9 = item.Value.DATA_P9,
                            DataP10 = item.Value.DATA_P10,
                            DataP11 = item.Value.DATA_P11,
                            DataP12 = item.Value.DATA_P12,
                            DataP13 = item.Value.DATA_P13,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            YearBdg = year.ToString(),
                            ProjectCode = item.Value.DATA_P11
                        });
                    }

                    if (newProjects.Count > 0)
                        await _repository.AddRangeAsync(newProjects);



                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return 0;
                }
            }

            return 1;
        }
    }

}
