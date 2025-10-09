using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget.Repository
{
    public class ReturnPPayRepository : IReturnPPayRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ReturnPPayRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDataReturnPPayModels> GetAllAsync(string? year, string? projectcode)
        {
            try {
                var query = _context.ReturnPPays.AsQueryable();

                if (!string.IsNullOrEmpty(year))
                    query = query.Where(p => p.YearBdg == year);

                if (!string.IsNullOrEmpty(projectcode))
                    query = query.Where(p => p.ProjectCode == projectcode);

                var project = await query
                    .Select(u => new APIResponseDataReturnPPayModels
                    {
                        DATA_P1 = u.DataP1,
                        DATA_P2 = u.DataP2
                    })
                    .FirstOrDefaultAsync(); // ✅ ใช้ `await` ที่นี่โดยตรง

                return project; // ❌ ไม่ต้อง `await` เพราะ `project` ไม่ใช่ Task
            }
            catch (Exception ex) 
            {
                return null;
            }
   
        }

        public async Task<ReturnPPayModels> GetPayCheckAllAsync(string? year, string? projectcode)
        {
            try
            {
                var query = _context.ReturnPPays.AsQueryable();

                if (!string.IsNullOrEmpty(year))
                    query = query.Where(p => p.YearBdg == year);

                if (!string.IsNullOrEmpty(projectcode))
                    query = query.Where(p => p.ProjectCode == projectcode);

                var project = await query
                    .Select(u => new ReturnPPayModels
                    {
                        DATA_P1 = u.DataP1,
                        DATA_P2 = u.DataP2 != null ? u.DataP2.ToString() : "",
                        YearBdg = u.YearBdg,
                        ProjectCode = u.ProjectCode
                       // , CreateDate = u.CreateDate,
                        , Id = u.Id

                    })
                    .FirstOrDefaultAsync(); // ✅ ใช้ `await` ที่นี่โดยตรง

                return project; // ❌ ไม่ต้อง `await` เพราะ `project` ไม่ใช่ Task
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task AddRangeAsync(List<ReturnPPay> projects)
        {
            await _context.ReturnPPays.AddRangeAsync(projects);
            await _context.SaveChangesAsync();
        }
        public async Task<ReturnPPay> GetByIdAsync(int id)
            => await _context.ReturnPPays.FindAsync(id);

        public async Task AddAsync(ReturnPPay entity)
        {
            await _context.ReturnPPays.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnPPay entity)
        {
            try
            {
                _context.ReturnPPays.Update(entity);
              var ccc= await _context.SaveChangesAsync();

            }
            catch(Exception ex) {
            
            }  
          
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ReturnPPays.FindAsync(id);
            if (entity != null)
            {
                _context.ReturnPPays.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
