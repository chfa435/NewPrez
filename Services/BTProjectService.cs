using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;

        public BTProjectService(ApplicationDbContext context, 
                                IBTRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        #region Add New Project 
        public async Task AddNewProjectAsync(Project project)
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

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            TAUser currentPM = await GetProjectManagerAsync(projectId);

            if (currentPM != null)
            {
                try
                {
                    // Remove Project Managers
                    await RemoveProjectManagerAsync(projectId);
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
                return await AddUserToProjectAsync(userId, projectId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (btUser != null)
            {
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if(!await IsUserOnProjectAsync(userId, projectId))
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
        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                project.Archived = true;
                await UpdateProjectAsync(project);

                //Archive project tickets
                foreach(Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = true;
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
        public async Task RestoreProjectAsync(Project project)
        {
            try
            {
                project.Archived = false;
                await UpdateProjectAsync(project);

                //Archive project tickets
                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = false;
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

        public async Task<List<TAUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            try
            {
                List<TAUser> developers = await GetProjectMembersByRoleAsync(projectId, nameof(BTRoles.Developer));
                List<TAUser> submitters = await GetProjectMembersByRoleAsync(projectId, nameof(BTRoles.Submitter));
                List<TAUser> admins = await GetProjectMembersByRoleAsync(projectId, nameof(BTRoles.Admin));

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


        public async Task<List<ActionProject>> GetAllProjectsByOrganizationIdAsync(int organizationId)
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
        public async Task<List<Project>> GetAllProjectsByOrgIdAsync(int organizationId)
        {

            try
            {
                List<Project>? projects = new();

                projects = await _context.Projects.Where(p=> p.Account!.ParentOrganizationId == organizationId && p.Archived == false)
                                            .Include(p => p.Members)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Comments)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Attachments)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.History)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.DeveloperUser)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.SubmitterUser)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketStatus)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketPriority)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketType)
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
        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int organizationId, string priorityName)
        {
            try
            {
                List<Project> projects = await GetAllProjectsByOrgIdAsync(organizationId);
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
        public async Task<List<Project>> GetArchivedProjectsAsync(int organizationId)
        {

            try
            {
                List<Project>? projects = new();

                projects = await _context.Projects.Where(p => p.Account!.ParentOrganizationId == organizationId && p.Archived == true)
                                            .Include(p => p.Members)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Comments)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Attachments)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.History)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.DeveloperUser)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.SubmitterUser)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketStatus)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketPriority)
                                            .Include(p => p.Tickets)!
                                                .ThenInclude(t => t.TicketType)
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
        public async Task<Project> GetProjectByIdAsync(int projectId, int organizationId)
        {
            try
            {
                Project? project = new();

                project = await _context.Projects
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketPriority)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketStatus)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketType)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.DeveloperUser)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.SubmitterUser)
                                        .Include(p => p.Members)
                                        .Include(p => p.ProjectPriority)
                                        .Include(p=>p.Account)
                                        .FirstOrDefaultAsync(p => p.Id == projectId && p.Account!.ParentOrganizationId == organizationId);

                return project!;


            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public async Task<TAUser> GetProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                 .Include(p => p.Members)
                                                 .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach(TAUser member in project?.Members!)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        return member;
                    }
                }

                return null!;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<TAUser>> GetProjectMembersByRoleAsync(int projectId, string roleName)
        {
            try
            {
                Project? project = await _context.Projects
                                           .Include(p => p.Members)
                                           .FirstOrDefaultAsync(p => p.Id == projectId);

                List<TAUser> members = new();

                foreach(TAUser user in project!.Members)
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
        public async Task<List<Project>> GetUnassignedProjectsAsync(int companyId)
        {

            try
            {
                List<Project>? result = new();
                List<Project>? projects = new();

                projects = await _context.Projects
                                         .Include(p => p.ProjectPriority)
                                         .Include(p => p.Members)
                                         .Where(p => p.AccountId == companyId).ToListAsync();

                foreach(Project project in projects)
                {
                    if((await GetProjectMembersByRoleAsync(project.Id, nameof(BTRoles.ProjectManager))).Count == 0)
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

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project>? projects = (await _context.Users
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(p=>p.Account)
                                                         .Include(u=>u.Projects)!
                                                            .ThenInclude(p=>p.Members)
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(p => p.Tickets)
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(t => t.Tickets)
                                                                .ThenInclude(t => t.DeveloperUser)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.SubmitterUser)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketPriority)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketStatus)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketType)
                                                         .FirstOrDefaultAsync(u=>u.Id == userId))?.Projects!.ToList();

                return projects!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TAUser>> GetUsersNotOnProjectAsync(int projectId, int organizationId)
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

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                 .Include(p => p.Members)
                                                 .FirstOrDefaultAsync(p => p.Id == projectId);
                bool result = false;

                if(project != null)
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


        public async Task RemoveProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                 .Include(p => p.Members)
                                                 .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (TAUser member in project?.Members!)
                {
                    if(await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        //Remove user from project
                        await RemoveUserFromProjectAsync(member.Id, projectId); 
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                TAUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId,projectId))
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
        public async Task UpdateProjectAsync(Project project)
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
