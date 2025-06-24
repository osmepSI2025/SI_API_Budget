using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;

namespace SME_API_Budget.Services
{
    public class ReturnPPayService : IReturnPPayService
    {
        private readonly IReturnPPayRepository _repository;
        private readonly ICallAPIService _serviceApi;
        private readonly IApiInformationRepository _repositoryApi;

        public ReturnPPayService(IReturnPPayRepository repository, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
        {
            _repository = repository; 
            _serviceApi = serviceApi;
            _repositoryApi = repositoryApi;
        }

        //public async Task<IEnumerable<ReturnPPay>> GetAllAsync(string? year, string? projectcode)
        //    => await _repository.GetAllAsync(year, projectcode);
        public async Task<APIResponseDataReturnPPayModels> GetAllAsync(string year, string projectcode)
        {
            APIResponseDataReturnPPayModels xapiResponseReturnAreaModels = new();

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);

                if (projects != null)
                {
                    return projects;
                  
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Pay" });
                if (!LApi.Any())
                {

                    return xapiResponseReturnAreaModels;


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
                    return xapiResponseReturnAreaModels;
                }

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnPPay(apiParam, year, projectcode);


                if (resultApi.Data == null)
                {
                    return xapiResponseReturnAreaModels;
                }
                else
                {
                    if (!string.IsNullOrEmpty(resultApi.Data.DATA_P1) && !string.IsNullOrEmpty(resultApi.Data.DATA_P2.ToString()))
                    {
                        List<ReturnPPay> newProjects = new();
                        newProjects.Add(new ReturnPPay
                        {
                            DataP1 = resultApi.Data.DATA_P1.ToString(),
                            DataP2 = resultApi.Data.DATA_P2.ToString(),
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            YearBdg = year,
                            ProjectCode = projectcode,
                        });


                        if (newProjects.Count > 0)
                            await _repository.AddRangeAsync(newProjects);


                    }

                }



                projects = await _repository.GetAllAsync(year, projectcode);

                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return xapiResponseReturnAreaModels;
                
            }
        }
        public async Task<ReturnPPay> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPPay entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPPay entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
