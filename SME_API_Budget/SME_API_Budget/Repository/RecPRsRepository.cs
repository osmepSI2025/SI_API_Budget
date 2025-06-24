using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;
using SME_API_SMEBudget.Entities;
using System;

namespace SME_API_Budget.Repository
{
    public class RecPRsRepository : IRecPRsRepository
    {
        private readonly SMEBudgetDBContext _context;

        public RecPRsRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecPR>> GetAllAsync()
        => await _context.RecPRs.ToListAsync();

        public async Task<RecPR> GetByIdAsync(int id)
        => await _context.RecPRs.FindAsync(id);

        public async Task AddAsync(RecPR rec)
        {
            await _context.RecPRs.AddAsync(rec);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecPR rec)
        {
            _context.RecPRs.Update(rec);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rec = await _context.RecPRs.FindAsync(id);
            if (rec != null)
            {
                _context.RecPRs.Remove(rec);
                await _context.SaveChangesAsync();
            }
        }
    }
}
