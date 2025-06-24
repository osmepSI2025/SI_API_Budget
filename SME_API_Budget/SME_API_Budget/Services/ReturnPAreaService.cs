using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SME_API_Budget.Services
{
    public class ReturnPAreaService : IReturnPAreaService
    {
        private readonly IReturnPAreaRepository _repository;
        private readonly ICallAPIService _serviceApi;
        private readonly IApiInformationRepository _repositoryApi;
        public ReturnPAreaService(IReturnPAreaRepository repository, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
        {
            _repository = repository;
            _serviceApi = serviceApi;
            _repositoryApi = repositoryApi;
        }

        //public async Task<ApiResponseReturnArea> GetAllAsync(string year, string projectcode)
        //{

        //    ApiResponseReturnArea xapiResponseReturnAreaModels = new ApiResponseReturnArea();
        //    try
        //    {
        //        var projects = await _repository.GetAllAsync(year, projectcode);


        //        if (projects!=null)
        //        {
        //            xapiResponseReturnAreaModels.StatusCode = 200;
        //            xapiResponseReturnAreaModels.Message = "OK";
        //            xapiResponseReturnAreaModels.Data = new APIResponseDataReturnPAreaModels
        //            {
        //                DATA_P1 = projects?.DATA_P1
        //            };
        //            return xapiResponseReturnAreaModels;
        //        }

        //        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Area" });
        //        if (!LApi.Any())
        //        {
        //            xapiResponseReturnAreaModels.StatusCode = 200;
        //            xapiResponseReturnAreaModels.Message = "OK";
        //            xapiResponseReturnAreaModels.Data = new APIResponseDataReturnPAreaModels
        //            {
        //                DATA_P1 = projects?.DATA_P1
        //            };
        //            return xapiResponseReturnAreaModels;
        //        }

        //        var apiParam = LApi.Select(x => new MapiInformationModels
        //        {
        //            ServiceNameCode = x.ServiceNameCode,
        //            ApiKey = x.ApiKey,
        //            AuthorizationType = x.AuthorizationType,
        //            ContentType = x.ContentType,
        //            CreateDate = x.CreateDate,
        //            Id = x.Id,
        //            MethodType = x.MethodType,
        //            ServiceNameTh = x.ServiceNameTh,
        //            Urldevelopment = x.Urldevelopment,
        //            Urlproduction = x.Urlproduction,
        //            Username = x.Username,
        //            Password = x.Password,
        //            UpdateDate = x.UpdateDate
        //        }).First(); // ดึงตัวแรกของ List
        //        var resultApi = await _serviceApi.GetDataApiAsync(apiParam, year, projectcode);

        //        if (resultApi.Data == null || resultApi.Data.Count == 0)
        //        {
        //            xapiResponseReturnAreaModels.StatusCode = 200;
        //            xapiResponseReturnAreaModels.Message = "OK";
        //            xapiResponseReturnAreaModels.Data = new APIResponseDataReturnPAreaModels
        //            {
        //                DATA_P1 = projects?.DATA_P1
        //            };
        //            return xapiResponseReturnAreaModels;
        //        }


        //        List<ReturnPArea> newProjects = new();

        //        foreach (var item in resultApi.Data)
        //        {
        //            DateTime.TryParseExact(item.Value.DATA_P4, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP4);
        //            DateTime.TryParseExact(item.Value.DATA_P5, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataP5);
        //            decimal.TryParse(item.Value.DATA_P12, out decimal dataP12);
        //            decimal.TryParse(item.Value.DATA_P13, out decimal dataP13);

        //            newProjects.Add(new ReturnPArea
        //            {

        //                DataP1 = item.Value.DATA_P1,

        //                CreateDate = DateTime.Now,
        //                UpdateDate = DateTime.Now,
        //                YearBdg = year,
        //                ProjectCode = projectcode,
        //            });
        //        }

        //        if (newProjects.Count > 0)
        //            await _repository.AddRangeAsync(newProjects);

        //        // ✅ เรียกข้อมูลจาก DB หลัง Insert
        //        projects = await _repository.GetAllAsync(year, projectcode);

        //        if (projects != null)
        //        {
        //            xapiResponseReturnAreaModels.StatusCode = 200;
        //            xapiResponseReturnAreaModels.Message = "OK";
        //            xapiResponseReturnAreaModels.Data = new APIResponseDataReturnPAreaModels
        //            {
        //                DATA_P1 = projects?.DATA_P1
        //            };
        //            return xapiResponseReturnAreaModels;

        //        }
        //        else 
        //        {
        //            xapiResponseReturnAreaModels.StatusCode = 200;
        //            xapiResponseReturnAreaModels.Message = "OK";

        //            return xapiResponseReturnAreaModels;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //        return null;
        //    }

        //}
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
    }
}
