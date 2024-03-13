using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class ActionProjectService : IActionProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;

        public ActionProjectService(ApplicationDbContext context,
                                IBTRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        #region Add New Project 
        public async Task AddNewActionProjectAsync(ActionProject project)
        {
            try
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public async Task<bool> AddActionProjectManagerAsync(string userId, int projectId)
        {
            TAUser currentPM = await GetActionProjectManagerAsync(projectId);

            if (currentPM != null)
            {
                try
                {
                    // Remove Project Managers
                    await RemoveActionProjectManagerAsync(projectId);
                }
                catch (Exception)
                {

                    throw;
                }
            }


            // Add the new PM
            try
            {
                // Add Project Manager
                return await AddUserToActionProjectAsync(userId, projectId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> AddUserToActionProjectAsync(string userId, int projectId)
        {
            TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (btUser != null)
            {
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (!await IsUserOnActionProjectAsync(userId, projectId))
                {
                    try
                    {
                        project!.Members!.Add(btUser);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        #region Archive Project
        public async Task ArchiveActionProjectAsync(ActionProject project)
        {
            try
            {
                project.Archived = true;
                await UpdateActionProjectAsync(project);

                //Archive project tickets
                foreach (ActionItem ticket in project.ActionItems)
                {
                    ticket.Archived = true;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region Restore Project
        public async Task RestoreActionProjectAsync(ActionProject project)
        {
            try
            {
                project.Archived = false;
                await UpdateActionProjectAsync(project);

                //Archive project tickets
                foreach (ActionItem ticket in project.ActionItems)
                {
                    ticket.Archived = false;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public async Task<List<TAUser>> GetAllActionProjectMembersExceptPMAsync(int projectId)
        {
            try
            {
                List<TAUser> developers = await GetActionProjectMembersByRoleAsync(projectId, nameof(BTRoles.Developer));
                List<TAUser> submitters = await GetActionProjectMembersByRoleAsync(projectId, nameof(BTRoles.Submitter));
                List<TAUser> admins = await GetActionProjectMembersByRoleAsync(projectId, nameof(BTRoles.Admin));

                List<TAUser> teamMembers = developers.Concat(submitters).Concat(admins).ToList();

                return teamMembers;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Account>> GetAllAccountByOrganizationIdAsync(int organizationId)
        {

            try
            {
                List<Account>? accounts = new();

                accounts = await _context.Accounts.Where(p => p.ParentOrganizationId == organizationId)
                                                  .Include(p => p.AccountMembers)
                                                  .Include(p => p.ParentOrganization)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ProjectPriority)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.Members)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.Comments)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.Attachments)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.History)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.Notifications)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.Actor)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.Submitter)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.ItemStatus)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.ItemPriority)
                                                  .Include(p => p.Projects)
                                                      .ThenInclude(p => p.ActionItems)!
                                                         .ThenInclude(t => t.ItemType)
                                                  .ToListAsync();
                return accounts;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<List<ActionProject>> GetAllActionProjectsByOrganizationIdAsync(int organizationId)
        {

            try
            {
                List<ActionProject>? projects = new();

                projects = await _context.Accounts.Where(p => p.ParentOrganizationId == organizationId)
                                                  .SelectMany(a => a.Projects)
                                                  .Where(p => p.Archived == false)
                                                  .Include(p => p.Members)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.Comments)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.Attachments)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.History)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.Notifications)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.Actor)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.Submitter)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.ItemStatus)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.ItemPriority)
                                                  .Include(p => p.ActionItems)!
                                                      .ThenInclude(t => t.ItemType)
                                                  .Include(p => p.ProjectPriority)
                                                  .ToListAsync();
                return projects;
            }
            catch (Exception)
            {

                throw;
            }

        }


        #region Get All Projects By Organization Id
        public async Task<List<ActionProject>> GetAllActionProjectsByOrgIdAsync(int organizationId)
        {

            try
            {
                List<ActionProject>? projects = new();

                projects = await _context.ActionProjects.Where(p => p.Account!.ParentOrganizationId == organizationId && p.Archived == false)
                                            .Include(p => p.Members)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Comments)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Attachments)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.History)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Actor)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Submitter)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemStatus)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemPriority)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemType)
                                            .Include(p => p.ProjectPriority)
                                            .ToListAsync();
                return projects;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get All Projects By Priority
        public async Task<List<ActionProject>> GetAllActionProjectsByPriorityAsync(int organizationId, string priorityName)
        {
            try
            {
                List<ActionProject> projects = await GetAllActionProjectsByOrgIdAsync(organizationId);
                int priorityId = await LookupProjectPriorityId(priorityName);

                return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Look up Project Priority Id
        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            try
            {
                int priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName))!.Id;
                return priorityId;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Archived Projects By Organization Id
        public async Task<List<ActionProject>> GetArchivedActionProjectsAsync(int organizationId)
        {

            try
            {
                List<ActionProject>? projects = new();

                projects = await _context.ActionProjects.Where(p => p.Account!.ParentOrganizationId == organizationId && p.Archived == true)
                                            .Include(p => p.Members)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Comments)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Attachments)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.History)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Actor)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.Submitter)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemStatus)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemPriority)
                                            .Include(p => p.ActionItems)!
                                                .ThenInclude(t => t.ItemType)
                                            .Include(p => p.ProjectPriority)
                                            .ToListAsync();
                return projects;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get Project By Id
        public async Task<ActionProject> GetActionProjectByIdAsync(int projectId, int organizationId)
        {
            try
            {
                ActionProject? project = new();

                project = await _context.ActionProjects
                                        .Include(p => p.ActionItems)
                                            .ThenInclude(t => t.ItemPriority)
                                        .Include(p => p.ActionItems)
                                            .ThenInclude(t => t.ItemStatus)
                                        .Include(p => p.ActionItems)
                                            .ThenInclude(t => t.ItemType)
                                        .Include(p => p.ActionItems)
                                            .ThenInclude(t => t.Actor)
                                        .Include(p => p.ActionItems)
                                            .ThenInclude(t => t.Submitter)
                                        .Include(p => p.Members)
                                        .Include(p => p.ProjectPriority)
                                        .Include(p => p.Account)
                                        .FirstOrDefaultAsync(p => p.Id == projectId && p.Account!.ParentOrganizationId == organizationId);

                return project!;


            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public async Task<TAUser?> GetActionProjectManagerAsync(int? projectId)
        {
            try
            {
                if (projectId != null)
                {

                    Project? project = await _context.Projects
                                                     .Include(p => p.Members)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId);

                    foreach (TAUser member in project?.Members!)
                    {
                        if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                        {
                            return member;
                        }
                    }
                }

                return null!;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<TAUser>> GetActionProjectMembersByRoleAsync(int projectId, string roleName)
        {
            try
            {
                Project? project = await _context.Projects
                                           .Include(p => p.Members)
                                           .FirstOrDefaultAsync(p => p.Id == projectId);

                List<TAUser> members = new();

                foreach (TAUser user in project!.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(user, roleName))
                    {
                        members.Add(user);
                    }
                }

                return members;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Get Unassigned Projects By Organization Id
        public async Task<List<ActionProject>> GetUnassignedActionProjectsAsync(int companyId)
        {

            try
            {
                List<ActionProject>? result = new();
                List<ActionProject>? projects = new();

                projects = await _context.ActionProjects
                                         .Include(p => p.ProjectPriority)
                                         .Include(p => p.Members)
                                         .Where(p => p.AccountId == companyId).ToListAsync();

                foreach (ActionProject project in projects)
                {
                    if ((await GetActionProjectMembersByRoleAsync(project.Id, nameof(BTRoles.ProjectManager))).Count == 0)
                    {
                        result.Add(project);
                    }
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        //public async Task<List<ActionProject>> GetUserActionProjectsAsync(string userId)
        //{
        //    try
        //    {
        //        List<ActionProject>? projects = (await _context.Users
        //                                                 .Include(u => u.Projects)!
        //                                                    .ThenInclude(p=>p.Account)
        //                                                 .Include(u=>u.Projects)!
        //                                                    .ThenInclude(p=>p.Members)
        //                                                 .Include(u => u.Projects)!
        //                                                    .ThenInclude(p => p.Tickets)
        //                                                 .Include(u => u.Projects)!
        //                                                    .ThenInclude(t => t.Tickets)
        //                                                        .ThenInclude(t => t.DeveloperUser)
        //                                                 .Include(u => u.Projects)!
        //                                                     .ThenInclude(t => t.Tickets)
        //                                                         .ThenInclude(t => t.SubmitterUser)
        //                                                 .Include(u => u.Projects)!
        //                                                     .ThenInclude(t => t.Tickets)
        //                                                         .ThenInclude(t => t.TicketPriority)
        //                                                 .Include(u => u.Projects)!
        //                                                     .ThenInclude(t => t.Tickets)
        //                                                         .ThenInclude(t => t.TicketStatus)
        //                                                 .Include(u => u.Projects)!
        //                                                     .ThenInclude(t => t.Tickets)
        //                                                         .ThenInclude(t => t.TicketType)
        //                                                 .FirstOrDefaultAsync(u=>u.Id == userId))?.ActionProjects!.ToList();

        //        return projects!;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<List<TAUser>> GetUsersNotOnActionProjectAsync(int projectId, int organizationId)
        {
            try
            {
                List<TAUser> users = await _context.Users.Where(u => u.Projects!.All(p => p.Id != projectId) && u.OrganizationId == organizationId).ToListAsync();
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsUserOnActionProjectAsync(string userId, int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                 .Include(p => p.Members)
                                                 .FirstOrDefaultAsync(p => p.Id == projectId);
                bool result = false;

                if (project != null)
                {
                    result = project.Members.Any(m => m.Id == userId);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task RemoveActionProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                 .Include(p => p.Members)
                                                 .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (TAUser member in project?.Members!)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        //Remove user from project
                        await RemoveUserFromActionProjectAsync(member.Id, projectId);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromActionProjectAsync(string userId, int projectId)
        {
            try
            {
                TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if (await IsUserOnActionProjectAsync(userId, projectId))
                    {
                        project?.Members?.Remove(btUser!);
                        await _context.SaveChangesAsync();
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Update Project
        public async Task UpdateActionProjectAsync(ActionProject project)
        {
            try
            {
                _context.Update(project);
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
