using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;
using System;

namespace SME_API_Budget.Repository
{
    public class ReturnPAreaRepository : IReturnPAreaRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPAreaRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDataReturnPAreaModels?> GetAllAsync(string? year, string? projectcode)
        {
            var query = _context.ReturnPAreas.AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.YearBdg == year);

            if (!string.IsNullOrEmpty(projectcode))
                query = query.Where(p => p.ProjectCode == projectcode);

            var project = await query
                .Select(u => new APIResponseDataReturnPAreaModels
                {
                    DATA_P1 = u.DataP1
                })
                .FirstOrDefaultAsync(); // ✅ ใช้ `await` ที่นี่โดยตรง

            return project; // ❌ ไม่ต้อง `await` เพราะ `project` ไม่ใช่ Task
        }
        // ✅ Insert หลายรายการพร้อมกัน
        public async Task AddRangeAsync(List<ReturnPArea> projects)
        {
            await _context.ReturnPAreas.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task<ReturnPArea> GetByIdAsync(int id)
            => await _context.ReturnPAreas.FindAsync(id);

        public async Task AddAsync(ReturnPArea entity)
        {
            await _context.ReturnPAreas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPArea entity)
        {
            _context.ReturnPAreas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPAreas.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPAreas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
