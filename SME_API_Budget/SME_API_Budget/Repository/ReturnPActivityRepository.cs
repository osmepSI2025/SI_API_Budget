using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_SMEBudget.Entities;
using System;

namespace SME_API_Budget.Repository
{
    public class ReturnPActivityRepository : IReturnPActivityRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPActivityRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReturnPActivity>> GetAllAsync(string? year, string? projectcode)

        {
            try
            {
                var query = _context.ReturnPActivities.AsQueryable();

                if (!string.IsNullOrEmpty(year))
                    query = query.Where(p => p.YearBdg == year);

                if (!string.IsNullOrEmpty(projectcode))
                    query = query.Where(p => p.ProjectCode == projectcode);

                return await query
                    .Include(m => m.ReturnPActivitySubs) // ✅ รวมข้อมูลจาก SubData
                    .ToListAsync();
            } catch(Exception ex) 
            {
                return null;
            }
          
        }
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnPActivities
                .Where(r => r.RefCode != 0)  // Check if RefCode is not the default value
                .Select(r => r.RefCode)      // Just select RefCode as int
                .ToListAsync();
        }
        public async Task AddRangeAsync(List<ReturnPActivity> projects)
        {
            await _context.ReturnPActivities.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsyncSub(List<ReturnPActivitySub> projects)
        {
            await _context.ReturnPActivitySubs.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task<ReturnPActivity> GetByIdAsync(int id)
            => await _context.ReturnPActivities.FindAsync(id);

        public async Task AddAsync(ReturnPActivity entity)
        {
            await _context.ReturnPActivities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPActivity entity)
        {
            _context.ReturnPActivities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPActivities.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPActivities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
