using NewTiceAI.Models;
using Microsoft.AspNetCore.Identity;

namespace NewTiceAI.Services.Interfaces
{
    public interface IBTRolesService
    {
        //New
        public Task<bool> AddUserToRoleAsync(TAUser user, string roleName);
        
        //New
        public Task<List<IdentityRole>> GetBTRolesAsync();

        public Task<bool> IsUserInRoleAsync(TAUser user, string roleName);

        //New
        public Task<string> GetRoleNameByIdAsync(string roleId);

        public Task<List<TAUser>> GetUsersInRoleAsync(string roleName, int companyId);

        //New
        public Task<List<TAUser>> GetUsersNotInRoleAsync(string roleName, int companyId);

        //New
        public Task<string> GetUserRoleAsync(TAUser user);

        //New
        public Task<IEnumerable<string>> GetUserRolesAsync(TAUser user);
        
        //New
        public Task<bool> RemoveUserFromRoleAsync(TAUser user, string roleName);

        //New
        public Task<bool> RemoveUserFromRolesAsync(TAUser user, IEnumerable<string> roles);

    }
}
