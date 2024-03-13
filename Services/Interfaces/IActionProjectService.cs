using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IActionProjectService
    {
        public Task AddNewActionProjectAsync(ActionProject project);

        public Task<bool> AddActionProjectManagerAsync(string userId, int projectId);

        public Task<bool> AddUserToActionProjectAsync(string userId, int projectId);

        public Task ArchiveActionProjectAsync(ActionProject project);

        public Task<List<ActionProject>> GetAllActionProjectsByPriorityAsync(int organizationId, string priorityName);

        public Task<List<ActionProject>> GetArchivedActionProjectsAsync(int organizationId);


        public Task<List<TAUser>> GetActionProjectMembersByRoleAsync(int projectId, string roleName);

        public Task<List<TAUser>> GetAllActionProjectMembersExceptPMAsync(int projectId);

        public Task<List<TAUser>> GetUsersNotOnActionProjectAsync(int projectId, int organizationId);

        public Task<List<Account>> GetAllAccountByOrganizationIdAsync(int organizationId);

        public Task<List<ActionProject>> GetAllActionProjectsByOrgIdAsync(int organizationId);

        public Task<ActionProject> GetActionProjectByIdAsync(int projectId, int organizationId);

        public Task<TAUser?> GetActionProjectManagerAsync(int? projectId);

        public Task<List<ActionProject>> GetUnassignedActionProjectsAsync(int organizationId);

        //public Task<List<Project>> GetUserActionProjectsAsync(string userId);

        public Task<bool> IsUserOnActionProjectAsync(string userId, int projectId);

        public Task RemoveActionProjectManagerAsync(int projectId);

        public Task<bool> RemoveUserFromActionProjectAsync(string userId, int projectId);

        public Task RestoreActionProjectAsync(ActionProject project);

        public Task UpdateActionProjectAsync(ActionProject project);


    }
}
