using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_SMEBudget.Entities;
using System;

namespace SME_API_Budget.Repository
{
    public class ApiInformationRepository : IApiInformationRepository
    {
        private readonly SMEBudgetDBContext _context;

        public ApiInformationRepository(SMEBudgetDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MapiInformation>> GetAllAsync(MapiInformationModels param)           
        {
            try
            {
                var query = _context.MapiInformations.AsQueryable();

                if (!string.IsNullOrEmpty(param.ServiceNameCode) && param.ServiceNameCode != "")
                    query = query.Where(p => p.ServiceNameCode == param.ServiceNameCode);
                return await query.ToListAsync();
            }
            catch (Exception ex) 
            {
                return  null;
            }
           
        }

        public async Task<MapiInformation> GetByIdAsync(int id)
            => await _context.MapiInformations.FindAsync(id);

        public async Task AddAsync(MapiInformation service)
        {
            await _context.MapiInformations.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MapiInformation service)
        {
            _context.MapiInformations.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _context.MapiInformations.FindAsync(id);
            if (service != null)
            {
                _context.MapiInformations.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
