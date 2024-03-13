using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class ActionItemService : IActionItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTLookupService _lookupService;
        private readonly IBTProjectService _projectService;
        private readonly IAccountService _accountService;
        private readonly IBTRolesService _rolesService;

        public ActionItemService(ApplicationDbContext context,
                               IBTLookupService lookupService,
                               IBTProjectService projectService,
                               IBTRolesService roleService,
                               IAccountService accountService)
        {
            _context = context;
            _lookupService = lookupService;
            _projectService = projectService;
            _rolesService = roleService;
            _accountService = accountService;
        }

        #region Add ActionItem
        public async Task AddAsync(ActionItem actionItem)
        {
            try
            {
                await _context.AddAsync(actionItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add ActionItem Attachment 
        public async Task AddAttachmentAsync(ActionItemAttachment attachment)
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

        #region Add ActionItem Comment
        public async Task AddCommentAsync(ActionItemComment comment)
        {
            try
            {
                await _context.AddAsync(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Archive ActionItem
        public async Task ArchiveAsync(ActionItem actionItem)
        {
            try
            {
                actionItem.Archived = true;
                await UpdateAsync(actionItem);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Assign ActionItem

        public async Task AssignAsync(int itemId, string userId)
        {
            ActionItem? actionItem = await _context.ActionItems.FirstOrDefaultAsync(t => t.Id == itemId);
            try
            {
                if (actionItem != null)
                {
                    try
                    {
                        actionItem.ActorId = userId;
                        // Revisit this code when assigning Tickets
                        actionItem.ItemStatus = EnumActionItemStatuses.Development;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Get ActionItem As No Tracking
        public async Task<ActionItem> GetAsNoTrackingAsync(int itemId)
        {
            try
            {
                ActionItem? actionItem = await _context.ActionItems
                                               .Include(t => t.Actor)
                                               .Include(t => t.Submitter)
                                               .Include(t => t.Account)
                                               .Include(t => t.Comments)
                                               .Include(t => t.Attachments)
                                               .AsNoTracking().FirstOrDefaultAsync(t => t.Id == itemId);

                return actionItem!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get ActionItems By Company Id
        public async Task<List<ActionItem>> GetItemsByOrgIdAsync(int organizationId)
        {
            List<ActionItem> actionItems = new List<ActionItem>();

            actionItems = await _context.ActionItems
                                    .Where(a => a.Account!.ParentOrganizationId == organizationId)
                                        .Include(a => a.Attachments)
                                        .Include(a => a.Comments)
                                        .Include(a => a.Actor)
                                        .Include(a => a.History)
                                        .Include(a => a.Submitter)
                                        .Include(a => a.Account)
                                        .Include(a => a.Contact)
                                            .ThenInclude(c => c!.RelationshipHolder)
                                        .Include(a => a.Contact)
                                            .ThenInclude(c => c!.Mentor)
                                        .Include(a => a.Contact)
                                            .ThenInclude(c => c!.SalesRepresentative)
                                    .ToListAsync();

            return actionItems;
        }
        #endregion

        #region Get ActionItem By Id
        public async Task<ActionItem> GetItemByIdAsync(int itemId)
        {
            try
            {
                ActionItem? actionItem = new();

                actionItem = await _context.ActionItems
                                     .Include(t => t.Actor)
                                     .Include(t => t.Submitter)
                                     .Include(t => t.Account)
                                     .Include(t => t.Comments)
                                     .Include(t => t.Attachments)
                                     .Include(t => t.History)
                                     .FirstOrDefaultAsync(t => t.Id == itemId);

                return actionItem!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get ActionItems By User Id
        //public async Task<List<ActionItem>> GetUserItemsAsync(string userId, int organizationId)
        //{
        //    TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        //    List<ActionItem>? actionItems = new();

        //    try
        //    {
        //        if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Admin)))
        //        {
        //            tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
        //                                            .SelectMany(p => p.Tickets!).ToList();
        //        }
        //        else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Developer)))
        //        {
        //            tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
        //                                            .SelectMany(p => p.Tickets!)
        //                                            .Where(t => t.DeveloperUserId == userId || t.SubmitterUserId == userId).ToList();
        //        }
        //        else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Submitter)))
        //        {
        //            tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
        //                                            .SelectMany(t => t.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
        //        }
        //        else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.ProjectManager)))
        //        {
        //            List<Ticket>? projectTickets = (await _projectService.GetUserProjectsAsync(userId)).SelectMany(t => t.Tickets!).ToList();
        //            List<Ticket>? submittedTickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
        //                                            .SelectMany(p => p.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
        //            tickets = projectTickets.Concat(submittedTickets).ToList();
        //        }

        //        return tickets;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion

        #region Get Archived Tickets
        public async Task<List<ActionItem>> GetArchivedAsync(int organizationId)
        {
            try
            {
                List<ActionItem> actionItems = (await GetItemsByOrgIdAsync(organizationId)).Where(t => t.Archived == true).ToList();
                return actionItems;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get ActionItem Attachment By Id
        public async Task<ActionItemAttachment> GetAttachmentByIdAsync(int attachmentId)
        {
            try
            {
                ActionItemAttachment? attachment = await _context.ActionItemAttachments
                                                                  .Include(t => t.Submitter)
                                                                  .FirstOrDefaultAsync(t => t.Id == attachmentId);
                return attachment!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Restore ActionItem
        public async Task RestoreAsync(ActionItem actionItem)
        {
            try
            {
                actionItem.Archived = false;
                await UpdateAsync(actionItem);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update ActionItem
        public async Task UpdateAsync(ActionItem actionItem)
        {
            try
            {
                _context.Update(actionItem);
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
