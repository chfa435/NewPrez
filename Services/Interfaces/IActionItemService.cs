using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IActionItemService
    {
        public Task AddAsync(ActionItem actionItem);

        public Task AddAttachmentAsync(ActionItemAttachment attachment);

        public Task AddCommentAsync(ActionItemComment comment);

        public Task ArchiveAsync(ActionItem actionItem);

        public Task AssignAsync(int itemId, string userId);


        public Task<List<ActionItem>> GetArchivedAsync(int organizationId);

        public Task<ActionItemAttachment> GetAttachmentByIdAsync(int attachmentId);

        public Task<ActionItem> GetAsNoTrackingAsync(int itemId);

        public Task<List<ActionItem>> GetItemsByOrgIdAsync(int organizationId);

        public Task<ActionItem> GetItemByIdAsync(int itemId);

        //public Task<List<ActionItem>> GetUserItemsAsync(string userId, int organizationId);


        public Task RestoreAsync(ActionItem actionItem);

        public Task UpdateAsync(ActionItem actionItem);
    }
}
