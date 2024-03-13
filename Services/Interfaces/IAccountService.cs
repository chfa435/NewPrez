using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IAccountService
    {
        Task AddContactAsync(int accountId, int contactId);
        Task<bool> IsAccountMember(int accountyId, int employeeId);
        Task<bool> IsAccountContact(int accountId, int contactId);
        //Task<IEnumerable<Account>> GetAccountsByOwnerIdAsync(string ownerId);
        Task<IEnumerable<Account>> GetAccountsByOrganizationIdAsync(int organizationId);
        Task<ICollection<int>> GetContactIdsAsync(int accountId);
        Task<ICollection<string>> GetMemberIdsAsync(int accountId);
        //Task<ICollection<Account>> GetContactAccountsAsync(int contactId);
        //Task<ICollection<int>> GetContactAccountIdsAsync(int contactId);
        Task<ICollection<Account>> GetMemberAccountsAsync(string memberId);
        Task RemoveContactAsync(int accountId, int employeeId);
    }
}