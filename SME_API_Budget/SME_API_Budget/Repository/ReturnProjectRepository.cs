using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget.Repository
{
    public class ReturnProjectRepository : IReturnProjectRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnProjectRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<int, APIResponseReturnProjectModels>> GetAllAsync(string? year, string? projectCode)
        {
            var query = _context.ReturnProjects.AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.YearBdg == year);

            if (!string.IsNullOrEmpty(projectCode))
                query = query.Where(p => p.ProjectCode == projectCode);

            var list = await query
                .Where(p => p.KeyId.HasValue)
                .ToListAsync();

            var result = list.ToDictionary(
                p => p.KeyId!.Value,
                p => new APIResponseReturnProjectModels
                {
                    DATA_P1 = p.DataP1,
                    DATA_P2 = p.DataP2,
                    DATA_P3 = p.DataP3,
                    DATA_P4 = p.DataP4,
                    DATA_P5 = p.DataP5,
                    DATA_P6 = p.DataP6,
                    DATA_P7 = p.DataP7,
                    DATA_P8 = p.DataP8,
                    DATA_P9 = p.DataP9,
                    DATA_P10 = p.DataP10,
                    DATA_P11 = p.DataP11,
                    DATA_P12 = p.DataP12,
                    DATA_P13 = p.DataP13,
                }
            );

            return result;
        }



        // ✅ ดึง KeyId ทั้งหมดจากฐานข้อมูล
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnProjects
                .Where(r => r.KeyId.HasValue) // ✅ เอาเฉพาะที่ไม่เป็น null
                .Select(r => r.KeyId.Value)   // ✅ แปลงจาก int? เป็น int
                .ToListAsync();
        }

        // ✅ Insert หลายรายการพร้อมกัน
        public async Task AddRangeAsync(List<ReturnProject> projects)
        {
            await _context.ReturnProjects.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }

        public async Task<ReturnProject> GetByIdAsync(int id)
            => await _context.ReturnProjects.FindAsync(id);



        public async Task AddAsync(ReturnProject entity)
        {
            await _context.ReturnProjects.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnProject entity)
        {
            _context.ReturnProjects.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnProjects.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnProjects.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
