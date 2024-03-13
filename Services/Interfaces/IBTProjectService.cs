using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IBTProjectService
    {
        public Task AddNewProjectAsync(Project project);

        public Task<bool> AddProjectManagerAsync(string userId, int projectId);

        public Task<bool> AddUserToProjectAsync(string userId, int projectId);

        public Task ArchiveProjectAsync(Project project);

        public Task<List<Project>> GetAllProjectsByPriorityAsync(int organizationId, string priorityName);

        public Task<List<Project>> GetArchivedProjectsAsync(int organizationId);


        public Task<List<TAUser>> GetProjectMembersByRoleAsync(int projectId, string roleName);

        public Task<List<TAUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        public Task<List<TAUser>> GetUsersNotOnProjectAsync(int projectId, int organizationId);

        public Task<List<Account>> GetAllAccountByOrganizationIdAsync(int organizationId);

        public Task<List<Project>> GetAllProjectsByOrgIdAsync(int organizationId);

        public Task<Project> GetProjectByIdAsync(int projectId, int organizationId);

        public Task<TAUser> GetProjectManagerAsync(int projectId);

        public Task<List<Project>> GetUnassignedProjectsAsync(int organizationId);

        public Task<List<Project>> GetUserProjectsAsync(string userId);

        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        public Task RemoveProjectManagerAsync(int projectId);

        public Task<bool> RemoveUserFromProjectAsync(string userId, int projectId);

        public Task RestoreProjectAsync(Project project);

        public Task UpdateProjectAsync(Project project);


    }
}
