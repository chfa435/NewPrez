using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace NewTiceAI.Services
{
    public class AccountService : IAccountService
    {

        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddContactAsync(int accountId, int contactId)
        {
            try
            {
                if (!await IsAccountMember(accountId, contactId))
                {
                    Contact? contact = (await _context.Contacts.FindAsync(contactId));
                    Account? account = (await _context.Accounts.FindAsync(accountId));

                    if (account != null && contact != null)
                    {
                        account.Contacts.Add(contact);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsAccountMember(int accountId, int memberId)
        {
            try
            {
                TAUser? member = await _context.Users.FindAsync(memberId);

                return await _context.Accounts
                                       .Include(c => c.AccountMembers)
                                       .Where(c => c.Id == accountId && c.AccountMembers
                                       .Contains(member!))
                                       .AnyAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsAccountContact(int accountId, int contactId)
        {
            try
            {
                TAUser? contact = await _context.Users.FindAsync(contactId);

                return await _context.Accounts
                                       .Include(c => c.Contacts)
                                       .Where(c => c.Id == accountId)
                                       .AnyAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //public async Task<ICollection<Account>> GetContactAccountsAsync(int contactId)
        //{
        //    try
        //    {
        //        Contact? contact = await _context.Contacts.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == contactId);
        //        return contact!.Accounts;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public async Task<ICollection<int>> GetContactAccountIdsAsync(int contactId)
        //{
        //    try
        //    {
        //        List<int> categoryIds = (await GetContactAccountsAsync(contactId)).Select(c => c.Id).ToList();
        //        return categoryIds;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<IEnumerable<Account>> GetAccountsByOrganizationIdAsync(int organizationId)
        {
            List<Account> accounts = new();

            try
            {
                accounts = await _context.Accounts.Where(c => c.ParentOrganizationId == organizationId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return accounts;
        }

        //public async Task<IEnumerable<Account>> GetAccountsByOwnerIdAsync(string ownerId)
        //{
        //    List<Account> accounts = new List<Account>();

        //    try
        //    {
        //        accounts = await _context.Accounts.Where(c => c.AccountOwnerId == ownerId).ToListAsync();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return accounts;
        //}


        public async Task RemoveContactAsync(int accountId, int contactId)
        {
            try
            {
                if (await IsAccountContact(accountId, contactId))
                {
                    Contact? contact = await _context.Contacts.FindAsync(contactId);
                    Account? account = await _context.Accounts.FindAsync(accountId);

                    if (account != null && contact != null)
                    {
                        account.Contacts.Remove(contact);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ICollection<int>> GetContactIdsAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<string>> GetMemberIdsAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Account>> GetMemberAccountsAsync(string memberId)
        {
            try
            {
                TAUser? member = await _context.Users.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == memberId);
                return member?.Accounts!;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}