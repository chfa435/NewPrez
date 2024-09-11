using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTLookupService _lookupService;
        private readonly IBTProjectService _projectService;
        private readonly IBTRolesService _rolesService;

        public BTTicketService(ApplicationDbContext context,
                               IBTLookupService lookupService,
                               IBTProjectService projectService,
                               IBTRolesService roleService)
        {
            _context = context;
            _lookupService = lookupService;
            _projectService = projectService;
            _rolesService = roleService;
        }

        #region Add New Ticket
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Ticket Attachment 
        public async Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            try
            {
                await _context.AddAsync(ticketAttachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Add Ticket Comment
        public async Task AddTicketCommentAsync(TicketComment ticketComment)
        {
            try
            {
                await _context.AddAsync(ticketComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Archive Ticket
        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = true;
                await UpdateTicketAsync(ticket);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Assign Ticket

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            try
            {
                if (ticket != null)
                {
                    try
                    {
                        ticket.DeveloperUserId = userId;
                        // Revisit this code when assigning Tickets
                        ticket.TicketStatusId = (await _lookupService.LookupTicketStatusIdAsync("Development")).Value;
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


        #region Get Ticket As No Tracking
        public async Task<Ticket> GetTicketAsNoTrackingAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                                               .Include(t => t.DeveloperUser)
                                               .Include(t => t.SubmitterUser)
                                               .Include(t => t.Project)
                                               .Include(t => t.TicketPriority)
                                               .Include(t => t.TicketStatus)
                                               .Include(t => t.TicketType)
                                               .Include(t => t.Comments)
                                               .Include(t => t.Attachments)
                                               .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticketId);

                return ticket!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Tickets By Company Id
        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int accountId)
        {
            List<Ticket> tickets = new List<Ticket>();

            tickets = await _context.Projects
                                    .Where(p => p.Account!.ParentOrganizationId == accountId)
                                    .SelectMany(p => p.Tickets)
                                        .Include(t => t.Attachments)
                                        .Include(t => t.Comments)
                                        .Include(t => t.DeveloperUser)
                                        .Include(t => t.History)
                                        .Include(t => t.SubmitterUser)
                                        .Include(t => t.TicketPriority)
                                        .Include(t => t.TicketStatus)
                                        .Include(t => t.TicketType)
                                        .Include(t => t.Project)
                                    .ToListAsync();



            //List<Ticket> bTickets = await _context.Tickets
            //                                      .Include(t => t.Project)
            //                                      .Include(t => t.Attachments)
            //                                      .Include(t => t.Comments)
            //                                      .Include(t => t.DeveloperUser)
            //                                      .Include(t => t.History)
            //                                      .Include(t => t.SubmitterUser)
            //                                      .Include(t => t.TicketPriority)
            //                                      .Include(t => t.TicketStatus)
            //                                      .Include(t => t.TicketType)
            //                                      .Include(t => t.Project)
            //                                      .Where(t => t.Project!.CompanyId == companyId)
            //                                      .ToListAsync();

            return tickets;
        }
        #endregion

        #region Get Ticket By Id
        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = new();

                ticket = await _context.Tickets
                                     .Include(t => t.DeveloperUser)
                                     .Include(t => t.SubmitterUser)
                                     .Include(t => t.Project)
                                     .Include(t => t.TicketPriority)
                                     .Include(t => t.TicketStatus)
                                     .Include(t => t.TicketType)
                                     .Include(t => t.Comments)
                                        .ThenInclude(c=>c.User)
                                     .Include(t => t.Attachments)
                                        .ThenInclude(c => c.User)
                                     .Include(t => t.History)
                                     .FirstOrDefaultAsync(t => t.Id == ticketId);

                return ticket!;
            }
            catch (Exception)
            {

                throw;
            }        
        }
        #endregion

        #region Get Tickets By User Id
        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Ticket>? tickets = new();

            try
            {
                if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Admin)))
                {
                    tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Developer)))
                {
                    tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!)
                                                    .Where(t => t.DeveloperUserId == userId || t.SubmitterUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Submitter)))
                {
                    tickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
                                                    .SelectMany(t => t.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.ProjectManager)))
                {
                    List<Ticket>? projectTickets = (await _projectService.GetUserProjectsAsync(userId)).SelectMany(t => t.Tickets!).ToList();
                    List<Ticket>? submittedTickets = (await _projectService.GetAllProjectsByOrgIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
                    tickets = projectTickets.Concat(submittedTickets).ToList();
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Archived Tickets
        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = (await GetAllTicketsByCompanyIdAsync(companyId)).Where(t => t.Archived == true).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Ticket Attachment By Id
        public async Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId)
        {
            try
            {
                TicketAttachment? ticketAttachment = await _context.TicketAttachments
                                                                  .Include(t => t.User)
                                                                  .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
                return ticketAttachment!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Restore Ticket
        public async Task RestoreTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = false;
                await UpdateTicketAsync(ticket);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Ticket
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
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
