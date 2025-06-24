using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget.Repository
{
    public class ReturnPRiskRepository : IReturnPRiskRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPRiskRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, APIResponseDataReturnPRiskModels>> GetAllAsync(string year, string projectcode)
        {
    
            var query = _context.ReturnPRisks.AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.YearBdg == year);

            if (!string.IsNullOrEmpty(projectcode))
                query = query.Where(p => p.ProjectCode == projectcode);

            var projects = await query
                .Select(u => new APIResponseDataReturnPRiskModels
                {
                  //  Id = u.Id,
                    DATA_P1 = u.DataP1,
                    DATA_P2 = u.DataP2,
                    DATA_P3 = u.DataP3,
                    DATA_P4 = u.DataP4,
                    DATA_P5 = u.DataP5,
                    RefCode = u.RefCode,


                })
                .ToDictionaryAsync(p => p.RefCode ?? 0); // ✅ เปลี่ยนเป็น Dictionary

            return projects;

        }
        // ✅ ดึง KeyId ทั้งหมดจากฐานข้อมูล
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnPRisks
                .Where(r => r.KeyId.HasValue) // ✅ เอาเฉพาะที่ไม่เป็น null
                .Select(r => r.KeyId.Value)   // ✅ แปลงจาก int? เป็น int
                .ToListAsync();
        }
        // ✅ Insert หลายรายการพร้อมกัน
        public async Task AddRangeAsync(List<ReturnPRisk> projects)
        {
            await _context.ReturnPRisks.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }


        public async Task<ReturnPRisk> GetByIdAsync(int id)
            => await _context.ReturnPRisks.FindAsync(id);

        public async Task AddAsync(ReturnPRisk entity)
        {
            await _context.ReturnPRisks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPRisk entity)
        {
            _context.ReturnPRisks.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPRisks.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPRisks.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
