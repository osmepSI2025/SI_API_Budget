using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;
using System;
using System.Text.Json;

namespace SME_API_Budget.Repository
{
    public class ReturnPExpectedRepository : IReturnPExpectedRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPExpectedRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, ReturnPExpectedApiResponse>> GetAllAsync(string year, string projectcode)
        {
            var query = _context.ReturnPExpecteds
                .Include(p => p.ReturnPExpectedSubs)
                .AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.YearBdg == year);

            if (!string.IsNullOrEmpty(projectcode))
                query = query.Where(p => p.ProjectCode == projectcode);

            var data = await query.ToListAsync();

            var projects = data.ToDictionary(
                p => p.KeyId, // Changed the key type to int
                p => new ReturnPExpectedApiResponse
                {
                    DATA_P1 = p.DataP1,
                    DATA_P2 = p.DataP2,
                    SubData = p.ReturnPExpectedSubs.ToDictionary(
                        s => s.SubCode,
                        s => JsonSerializer.SerializeToElement(new ReturnPExpectedSubModels
                        {
                            DATA_P_S1 = s.DataPS1
                        })
                    )
                });

            return projects;
        }



        //// ✅ ดึง KeyId ทั้งหมดจากฐานข้อมูล
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnPExpecteds
                .Where(r => r.KeyId !=0) // ✅ เอาเฉพาะที่ไม่เป็น null
                .Select(r => r.KeyId)   // ✅ แปลงจาก int? เป็น int
                .ToListAsync();
        }
        // ✅ Insert หลายรายการพร้อมกัน
        public async Task AddRangeAsync(List<ReturnPExpected> projects)
        {
            await _context.ReturnPExpecteds.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task AddSubRangeAsync(List<ReturnPExpectedSub> projects)
        {
            await _context.ReturnPExpectedSubs.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        
        public async Task SaveDataAsync(ReturnPExpectedModels responseData)
        {
            // Check if the main entity already exists
            var mainEntity = await _context.ReturnPExpecteds
                .Include(e => e.ReturnPExpectedSubs)
                .FirstOrDefaultAsync(e => e.KeyId == responseData.KeyId);

            if (mainEntity == null)
            {
                // Create new main entity
                mainEntity = new ReturnPExpected
                {
                    KeyId = responseData.KeyId,
                    DataP1 = responseData.DATA_P1,
                    DataP2 = responseData.DATA_P2,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                _context.ReturnPExpecteds.Add(mainEntity);
            }
            else
            {
                // Update existing main entity
                mainEntity.DataP1 = responseData.DATA_P1;
                mainEntity.DataP2 = responseData.DATA_P2;
                mainEntity.UpdateDate = DateTime.Now;
            }

            //  // Add sub data
            foreach (var sub in responseData.ToSubEntities())
            {
                // Check if sub already exists
                var existingSub = mainEntity.ReturnPExpectedSubs.FirstOrDefault(s => s.SubCode == sub.SubCode);
                if (existingSub == null)
                {
                    existingSub = new ReturnPExpectedSub
                    {
                        SubCode = sub.SubCode,
                        DataPS1 = sub.DATA_P_S1,
                       
                    };

                    mainEntity.ReturnPExpectedSubs.Add(existingSub);
                }
                else
                {
                    // Update existing sub
                    existingSub.DataPS1 = sub.DATA_P_S1;
                }
            }

            // Save to DB
            await _context.SaveChangesAsync();
            Console.WriteLine("Data saved successfully.");
        }
        public async Task<ReturnPExpected> GetByIdAsync(int id)
            => await _context.ReturnPExpecteds.FindAsync(id);

        public async Task AddAsync(ReturnPExpected entity)
        {
            await _context.ReturnPExpecteds.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPExpected entity)
        {
            _context.ReturnPExpecteds.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPExpecteds.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPExpecteds.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
