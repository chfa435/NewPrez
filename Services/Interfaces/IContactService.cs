using Microsoft.EntityFrameworkCore;
using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IContactService
    {
        public Task AddNewContactAsync(Contact contact);

        public Task AddContactAttachmentAsync(ContactAttachment attachment);


        public Task<ContactAttachment> GetContactAttachmentByIdAsync(int attachmentId);


        public Task<List<Contact>> GetAllContactsByOrgIdAsync(int organizationId);

        public Task<Contact> GetContactByIdAsync(int contactId);

        public Task<List<Contact>> GetContactsByHMSIdAsync(int hmsId, int organizationId);

        public Task<bool> LoadContactsAsync(IList<Contact> contacts);

        public Task UpdateContactAsync(Contact ticket);

    }
}
