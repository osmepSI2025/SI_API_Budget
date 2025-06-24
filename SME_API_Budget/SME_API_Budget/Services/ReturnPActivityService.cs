using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Repository;
using SME_API_SMEBudget.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Services
{
    public class ReturnPActivityService : IReturnPActivityService
    {
        private readonly SMEBudgetDBContext _context;
        private readonly IApiInformationRepository _repositoryApi;
        private readonly ICallAPIService _serviceApi;
        private readonly IReturnPActivityRepository _repository;

        public ReturnPActivityService(SMEBudgetDBContext context, IApiInformationRepository repositoryApi, ICallAPIService serviceApi, IReturnPActivityRepository repository)
        {
            _context = context;

            _repositoryApi = repositoryApi;
            _serviceApi = serviceApi;
            _repository = repository;
        }

        public async Task<IEnumerable<ReturnPActivity>> GetAllAsync(string year, string projectcode)
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

                var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Return_P_Activity" });
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

                var resultApi = await _serviceApi.GetDataApiAsync_ReturnPActivity(apiParam, year, projectcode);

                if (resultApi.Data == null)
                {
                    return projects.ToList();
                }

                var existingKeyIds = (await _repository.GetAllKeyIdsAsync()).ToHashSet();
                List<ReturnPActivity> newProjects = new();
                List<ReturnPActivitySub> newProjectsSub = new();

                foreach (var item in resultApi.Data)
                {
                    if (!int.TryParse(item.Key, out int keyId) || existingKeyIds.Contains(keyId))
                        continue;
                    newProjects.Add(new ReturnPActivity
                    {
                        RefCode = keyId,
                        DataP1 = item.Value.DATA_P1,
                        DataP2 = item.Value.DATA_P2,
                        DataP3 = Convert.ToDecimal(item.Value.DATA_P3),
                        DataP4 = Convert.ToDecimal(item.Value.DATA_P4),
                        DataP5 = item.Value.DATA_P5,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        YearBdg = year,
                        ProjectCode = projectcode
                    });

                    // 🔹 อ่านค่า SubData ที่เป็น Dictionary
                    foreach (var subItem in item.Value.SubData)
                    {
                        newProjectsSub.Add(new ReturnPActivitySub
                        {
                            RefCode = keyId,
                            SubCode = subItem.Key, // ใช้ Key เป็น SubCode
                            DataP6 = subItem.Value.DATA_P6,
                            DataP7 = Convert.ToDecimal(subItem.Value.DATA_P7),
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
                return new List<ReturnPActivity>();
            }
            // => await _repository.GetAllAsync(year, pjcode);
        }

        public async Task<ReturnPActivity?> GetByIdAsync(int? refCode)
        {
            return await _context.ReturnPActivities
                .Include(m => m.ReturnPActivitySubs)
                .FirstOrDefaultAsync(m => m.RefCode == Convert.ToInt16(refCode));
        }

        public async Task AddAsync(ReturnPActivity activity)
        {
            _context.ReturnPActivities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPActivity activity)
        {
            _context.ReturnPActivities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? refCode)
        {
            var activity = await GetByIdAsync(refCode);
            if (activity != null)
            {
                _context.ReturnPActivities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
