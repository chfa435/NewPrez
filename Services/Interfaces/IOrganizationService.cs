using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IOrganizationService
    {

        public Task<List<Account>> GetAccountsAsync(int organizationId);

        public Task<List<TAUser>> GetMembersAsync(int organizationId);

        public Task<Organization> GetOrgInfoById(int? organizationId);

    }
}
