using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<TAUser> _userManager;

        public BTRolesService(ApplicationDbContext context,
                              RoleManager<IdentityRole> roleManager,
                              UserManager<TAUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(TAUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<IdentityRole>> GetBTRolesAsync()
        {
            try
            {
                List<IdentityRole> result = new();
                result = await _context.Roles.ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            try
            {
                IdentityRole? role = _context.Roles.Find(roleId);
                string? result = await _roleManager.GetRoleNameAsync(role!);
                return result!;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetUserRoleAsync(TAUser user)
        {
            try
            {
                IEnumerable<string> result = await _userManager.GetRolesAsync(user);
                return result.FirstOrDefault()!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(TAUser user)
        {
            try
            {
                IEnumerable<string> result = await _userManager.GetRolesAsync(user);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<TAUser>> GetUsersInRoleAsync(string roleName, int orgId)
        {
            try
            {
                List<TAUser> btUsers = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
                List<TAUser> results = btUsers.Where(u => u.OrganizationId == orgId).ToList();

                return results; 
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TAUser>> GetUsersNotInRoleAsync(string roleName, int organizationId)
        {
            try
            {
                List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
                List<TAUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();

                List<TAUser> result = roleUsers.Where(u => u.OrganizationId == organizationId).ToList();

                return result;
            }
            catch (Exception)
            {

                throw;
            }        
        }

        public async Task<bool> IsUserInRoleAsync(TAUser user, string roleName)
        {
            try
            {
                bool result = await _userManager.IsInRoleAsync(user, roleName);
                return result;  
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(TAUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
                return result;  
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromRolesAsync(TAUser user, IEnumerable<string> roles)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
