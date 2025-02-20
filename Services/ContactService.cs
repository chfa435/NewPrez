using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTLookupService _lookupService;
        private readonly IBTProjectService _projectService;
        private readonly IBTRolesService _rolesService;

        public ContactService(ApplicationDbContext context,
                               IBTLookupService lookupService,
                               IBTProjectService projectService,
                               IBTRolesService roleService)
        {
            _context = context;
            _lookupService = lookupService;
            _projectService = projectService;
            _rolesService = roleService;
        }

        #region Add New Contact
        public async Task AddNewContactAsync(Contact contact)
        {
            try
            {
                await _context.AddAsync(contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Contact Attachment 
        public async Task AddContactAttachmentAsync(ContactAttachment attachment)
        {
            try
            {
                await _context.AddAsync(attachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Get Contact Attachment By Id
        public async Task<ContactAttachment> GetContactAttachmentByIdAsync(int attachmentId)
        {
            try
            {
                ContactAttachment? contactAttachment = await _context.ContactAttachments
                                                                  .Include(c => c.Contact)
                                                                  .Include(c => c.User)
                                                                  .FirstOrDefaultAsync(t => t.Id == attachmentId);
                return contactAttachment!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Get Tickets By Company Id
        public async Task<List<Contact>> GetAllContactsByOrgIdAsync(int organizationId)
        {
            List<Contact> tickets = new List<Contact>();

            tickets = await _context.Contacts
                                    .Where(p => p.Account!.ParentOrganizationId == organizationId)
                                    .Include(c => c.ActionItems)
                                    .Include(c => c.Address)
                                    .Include(c => c.Account)
                                    .Include(c => c.Organization)
                                    .Include(c => c.ContactOwner)
                                    .Include(c => c.RelationshipHolder)
                                    .Include(c => c.Mentor)
                                    .Include(c => c.Residency)
                                    .Include(c => c.Fellowship)
                                    .Include(c => c.Fellowship2)
                                    .Include(c => c.CurrentDistributor)
                                    .Include(c => c.SalesRepresentative)
                                    .Include(c => c.Practice)
                                    .ToListAsync();

            return tickets;
        }
        #endregion

        #region Get Contact By Id
        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            try
            {
                Contact? contact = new();

                contact = await _context.Contacts
                                        .Include(c => c.ActionItems)
                                        .Include(c => c.Address)
                                        .Include(c => c.Account)
                                        .Include(c => c.Organization)
                                        .Include(c => c.ContactOwner)
                                        .Include(c => c.RelationshipHolder)
                                        .Include(c => c.Mentor)
                                        .Include(c => c.Residency)
                                        .Include(c => c.Fellowship)
                                        .Include(c => c.Fellowship2)
                                        .Include(c => c.CurrentDistributor)
                                        .Include(c => c.SalesRepresentative)
                                        .Include(c => c.Practice)
                                        .FirstOrDefaultAsync(t => t.Id == contactId);

                return contact!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Contacts By (Holder, Mentor or SalesRep) Id
        public async Task<List<Contact>> GetContactsByHMSIdAsync(int hmsId, int organizationId)
        {
            List<Contact>? contacts = new();

            try
            {
                contacts = await _context.Contacts.Where(c => c.RelationshipHolderId == hmsId ||
                                                              c.MentorId == hmsId ||
                                                              c.SalesRepresentativeId == hmsId)
                                                  .Include(c => c.ActionItems)
                                                  .Include(c => c.Address)
                                                  .Include(c => c.Account)
                                                  .Include(c => c.Organization)
                                                  .Include(c => c.ContactOwner)
                                                  .Include(c => c.RelationshipHolder)
                                                  .Include(c => c.Mentor)
                                                  .Include(c => c.Residency)
                                                  .Include(c => c.Fellowship)
                                                  .Include(c => c.Fellowship2)
                                                  .Include(c => c.CurrentDistributor)
                                                  .Include(c => c.SalesRepresentative)
                                                  .Include(c => c.Practice).ToListAsync();

                return contacts;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public async Task<bool> ImportContactsAsync(IList<Contact> contacts)
        {
            try
            {
                await _context.Contacts.AddRangeAsync(contacts);
                int result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetContactImportIdAsync(Import import)
        {

            try
            {
                await _context.Imports.AddAsync(import);
                await _context.SaveChangesAsync();
                return import.Id;
            }
            catch (Exception)
            {

                throw;
            }


        }

        #region Update Contact
        public async Task UpdateContactAsync(Contact contact)
        {
            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
