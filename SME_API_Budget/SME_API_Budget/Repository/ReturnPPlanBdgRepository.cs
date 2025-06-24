using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget.Repository
{
    public class ReturnPPlanBdgRepository : IReturnPPlanBdgRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPPlanBdgRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReturnPPlanBdg>> GetAllAsync(string? year, string? projectcode)

        {
            //var query = _context.ReturnPPlanBdgs.AsQueryable();

            //if (!string.IsNullOrEmpty(year) && year != "")
            //    query = query.Where(p => p.YearBdg == year);

            //if (!string.IsNullOrEmpty(projectcode) && projectcode != "")
            //    query = query.Where(p => p.ProjectCode == projectcode);

            //return await query.ToListAsync();
            try
            {
                var query = _context.ReturnPPlanBdgs.AsQueryable();

                if (!string.IsNullOrEmpty(year))
                    query = query.Where(p => p.YearBdg == year);

                if (!string.IsNullOrEmpty(projectcode))
                    query = query.Where(p => p.ProjectCode == projectcode);

                return await query
                    .Include(m => m.ReturnPPlanBdgSubs) // ✅ รวมข้อมูลจาก SubData
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<int>> GetAllKeyIdsAsync()
        {
            return await _context.ReturnPPlanBdgs
                .Where(r => r.RefCode != 0)
                .Select(r => r.RefCode)
                .ToListAsync();
        }
        public async Task AddRangeAsync(List<ReturnPPlanBdg> projects)
        {
            await _context.ReturnPPlanBdgs.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsyncSub(List<ReturnPPlanBdgSub> projects)
        {
            await _context.ReturnPPlanBdgSubs.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task<ReturnPPlanBdg> GetByIdAsync(int id)
            => await _context.ReturnPPlanBdgs.FindAsync(id);

        public async Task AddAsync(ReturnPPlanBdg entity)
        {
            await _context.ReturnPPlanBdgs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPPlanBdg entity)
        {
            _context.ReturnPPlanBdgs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPPlanBdgs.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPPlanBdgs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
