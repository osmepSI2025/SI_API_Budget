using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Globalization;

namespace SME_API_Budget.Services
{
    public class ReturnPAreaService : IReturnPAreaService
    {
        private readonly IReturnPAreaRepository _repository;
        private readonly ICallAPIService _serviceApi;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly IReturnProjectService _returnProjectService;

        public ReturnPAreaService(IReturnPAreaRepository repository, ICallAPIService serviceApi,
            IApiInformationRepository repositoryApi, IReturnProjectService returnProjectService)
        {
            _repository = repository;
            _serviceApi = serviceApi;
            _repositoryApi = repositoryApi;
            _returnProjectService = returnProjectService;
        }

        public async Task<APIResponseDataReturnPAreaModels> GetAllAsync(string year, string projectcode)
        {
            ApiResponseReturnArea xapiResponseReturnAreaModels = new();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects != null)
                {
                    return projects;
                    //xapiResponseReturnAreaModels.StatusCode = 200;
                    //xapiResponseReturnAreaModels.Message = "OK";
                    //xapiResponseReturnAreaModels.Data = new APIResponseDataReturnPAreaModels
                    //{
                    //    DATA_P1 = projects.DATA_P1 ?? ""
                    //};
                    //return xapiResponseReturnAreaModels;
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Area" });
                if (!LApi.Any())
                {
                    return null;                //return new ApiResponseReturnArea  
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

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnArea(apiParam, year, projectcode);


                if (resultApi.Data == null)
                {
                    return null;
                }
                else
                {
                    List<ReturnPArea> newProjects = new();
                    newProjects.Add(new ReturnPArea
                    {
                        DataP1 = resultApi.Data.DATA_P1,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode,
                    });


                    if (newProjects.Count > 0)
                        await _repository.AddRangeAsync(newProjects);

                }



                projects = await _repository.GetAllAsync(year, projectcode);

                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public async Task<ReturnPArea> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPArea entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPArea entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<int> BatchP_Area()
        {
            var thaiCulture = new CultureInfo("th-TH");
            var buddhistCalendar = new ThaiBuddhistCalendar();

            var currentYear = buddhistCalendar.GetYear(DateTime.Now);

            var years = Enumerable.Range(currentYear - 4, 5).Reverse();

            foreach (var year in years)
            {
                var Lprojects = await _returnProjectService.GetAllAsync(year.ToString(),"");

                foreach (var item in Lprojects)
                {               
                    var result = await GetAllAsync(year.ToString(), item.Value.DATA_P11);                 
                }
              

            }
            return 1; // Placeholder for delete operation, implement as needed
        }
    }
}
