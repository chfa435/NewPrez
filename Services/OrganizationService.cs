using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAccountsAsync(int organizationId)
        {
            try
            {
                List<Account>? accounts = new();

                accounts = await _context.Accounts.Where(a=>a.ParentOrganizationId == organizationId).ToListAsync();

                return accounts;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TAUser>> GetMembersAsync(int companyId)
        {
            try
            {
                List<TAUser>? members = new();

                members = (await _context.Organizations.Include(c=>c.Members).FirstOrDefaultAsync(c=>c.Id == companyId))!.Members.ToList();
                
                return members; 
            }
            catch (Exception)
            {

                throw;
            }        
        }

        public async Task<Organization> GetOrgInfoById(int? organizationId)
        {
            try
            {
                Organization? organization = new();

                if(organizationId != null)
                {
                    organization = await _context.Organizations
                                            .Include(c=>c.Members)
                                            //.Include(c=>c.Invites)
                                            .FirstOrDefaultAsync(c=>c.Id == organizationId); 
                }
                return organization!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
