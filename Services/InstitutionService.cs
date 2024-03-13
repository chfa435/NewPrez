using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;

namespace NewTiceAI.Services
{
    public class InstitutionService : IInstitutionService
    {
        private readonly ApplicationDbContext _context;

        public InstitutionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddInstitutionAsync(Institution? institution)
        {
            try
            {
                if (institution != null)
                {
                    await _context.AddAsync(institution);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteInstitutionAsync(Institution? institution)
        {
            try
            {
                if (institution != null)
                {
                    _context.Institutions.Remove(institution);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Institution> GetInstitutionAsync(int? institutionId)
        {
            try
            {
                Institution? institution = new();
                if (institutionId != null)
                {
                    institution = await _context.Institutions
                                                .FirstOrDefaultAsync(m => m.Id == institutionId);
                }
                return institution!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Institution>> GetInstitutionsAsync()
        {
            try
            {
                return await _context.Institutions.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateInstitutionAsync(Institution? institution)
        {
            try
            {
                if (institution != null)
                {
                    _context.Update(institution);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
