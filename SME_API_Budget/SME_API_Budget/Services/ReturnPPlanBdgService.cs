using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SME_API_Budget.Services
{
    public class ReturnPPlanBdgService : IReturnPPlanBdgService
    {
        private readonly IReturnPPlanBdgRepository _repository;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
       
        public ReturnPPlanBdgService(IReturnPPlanBdgRepository repository, IApiInformationRepository repositoryApi, ICallAPIService serviceApi)
        {
            _repository = repository;
            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
        }
        public async Task<IEnumerable<ReturnPPlanBdg>> GetAllAsync(string year, string projectcode)
        {
            var result = new Dictionary<int, ReturnPActivityModels>();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // ทำให้ JSON อ่านง่ายขึ้น
            };

            try
            {
                var projects = await _repository.GetAllAsync(year, projectcode);
                //    var strJson = JsonSerializer.Serialize(projects);
                if (projects.Any())
                {
                    return projects.ToList();
                }

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Plan_Bdg" });
                if (!LApi.Any())
                {
                    return projects.ToList();
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
                    return projects.ToList();
                }

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnPPlanBdg(apiParam, year, projectcode);

                if (resultApi.Data == null)
                {
                    return projects.ToList();
                }

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPPlanBdg> newProjects = new();
                List<ReturnPPlanBdgSub> newProjectsSub = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;
                    newProjects.Add(new ReturnPPlanBdg
                    {
                        RefCode = keyId,
                        DataP1 = item.Value.DATA_P1,
                       
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode
                    });

                    // 🔹 อ่านค่า SubData ที่เป็น Dictionary
                    foreach (var subItem in item.Value.SubData)
                    {
                        newProjectsSub.Add(new ReturnPPlanBdgSub
                        {
                            RefCode = keyId,
                            SubCode = subItem.Key, // ใช้ Key เป็น SubCode
                            DataPS1 = subItem.Value.DATA_P_S1,
                            DataPS2 = Convert.ToDecimal(subItem.Value.DATA_P_S2),
                            BdgType = subItem.Value.BDG_TYPE,
                            RefCode2 = subItem.Value.REF_CODE_2,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        });
                    }
                }

                if (newProjects.Count > 0)
                    await _repository.AddRangeAsync(newProjects);
                // ✅ บันทึกข้อมูล SubData
                if (newProjectsSub.Count > 0)
                    await _repository.AddRangeAsyncSub(newProjectsSub);

                // ✅ เรียกข้อมูลจาก DB หลัง Insert
                projects = await _repository.GetAllAsync(year, projectcode);
                return projects.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new List<ReturnPPlanBdg>();
            }
            // => await _repository.GetAllAsync(year, pjcode);
        }
        //public async Task<IEnumerable<ReturnPPlanBdg>> GetAllAsync(string? year, string? projectcode)
        //    => await _repository.GetAllAsync(year, projectcode);

        public async Task<ReturnPPlanBdg> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(ReturnPPlanBdg entity)
            => await _repository.AddAsync(entity);

        public async Task UpdateAsync(ReturnPPlanBdg entity)
            => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
