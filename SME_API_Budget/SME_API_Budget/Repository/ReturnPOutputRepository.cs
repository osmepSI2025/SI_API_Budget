using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget.Repository
{
    public class ReturnPOutputRepository : IReturnPOutputRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPOutputRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, APIResponseDataReturnPOutputModels>> GetAllAsync(string year, string projectcode)
        {
            var query = _context.ReturnPOutputs.AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.YearBdg == year);

            if (!string.IsNullOrEmpty(projectcode))
                query = query.Where(p => p.ProjectCode == projectcode);

            var list = await query
                .Where(u => u.KeyId.HasValue)
                .Select(u => new
                {
                    KeyId = u.KeyId.Value,
                    Model = new APIResponseDataReturnPOutputModels
                    {
                        DATA_P1 = u.DataP1,
                        DATA_P2 = u.DataP2,
                        DATA_P3 = u.DataP3,
                        DATA_P4 = u.DataP4,
                        DATA_P5 = u.DataP5
                    }
                })
                .ToListAsync();

            return list.ToDictionary(x => x.KeyId, x => x.Model);
        }
        // ✅ ดึง KeyId ทั้งหมดจากฐานข้อมูล
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnPOutputs
                .Where(r => r.KeyId.HasValue) // ✅ เอาเฉพาะที่ไม่เป็น null
                .Select(r => r.KeyId.Value)   // ✅ แปลงจาก int? เป็น int
                .ToListAsync();
        }
        // ✅ Insert หลายรายการพร้อมกัน
        public async Task AddRangeAsync(List<ReturnPOutput> projects)
        {
            await _context.ReturnPOutputs.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
      
        
        
        public async Task<ReturnPOutput> GetByIdAsync(int id)
            => await _context.ReturnPOutputs.FindAsync(id);

        public async Task AddAsync(ReturnPOutput entity)
        {
            await _context.ReturnPOutputs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPOutput entity)
        {
            _context.ReturnPOutputs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPOutputs.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPOutputs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
