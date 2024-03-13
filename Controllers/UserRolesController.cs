using NewTiceAI.Extensions;
using NewTiceAI.Models;
using NewTiceAI.Models.ViewModels;
using NewTiceAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace NewTiceAI.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserRolesController : TABaseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly IBTRolesService _rolesService;

        public UserRolesController(IOrganizationService organizationService, IBTRolesService rolesService)
        {
            _organizationService = organizationService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            // 1 - Add an instance of the ViewModel as a List (model)
            List<ManageUserRolesViewModel> model = new();
            // 2 -Get CompanyId
            int organizationId = User.Identity!.GetOrganizationId();
            // Get all company users
            List<TAUser> btUsers = await _organizationService.GetMembersAsync(organizationId);

            // 3 - Loop over the users to populate the ViewModel
            //      - instantiate single ViewModel
            //      - use _rolesService
            //      - Create multi-selects

            foreach (TAUser btUser in btUsers)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.BTUser = btUser;
                IEnumerable<string> currentRoles = await _rolesService.GetUserRolesAsync(btUser);
                viewModel.Roles = new MultiSelectList(await _rolesService.GetBTRolesAsync(), "Name", "Name", currentRoles);

                model.Add(viewModel);
            }


            // 4 - Return the model to the View
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {

            // Get the company Id
            //int organizationId = User.Identity!.GetOrganizationId();

            // Instantiate the BTUser
            TAUser? btUser = (await _organizationService.GetMembersAsync(_organizationId)).FirstOrDefault(u => u.Id == member.BTUser!.Id);

            // Get Roles for the User
            IEnumerable<string> currentRoles = await _rolesService.GetUserRolesAsync(btUser!);

            // Get Selected Roles for the User
            //string? selectedUserRoles = member.SelectedRoles!.FirstOrDefault();


            //Add User to new role
            if (member.SelectedRoles != null)
            {
                if (await _rolesService.RemoveUserFromRolesAsync(btUser!, currentRoles))
                {
                    //await _rolesService.AddUserToRoleAsync(btUser!, selectedUserRole);

                    foreach (string role in member.SelectedRoles)
                    {
                        await _rolesService.AddUserToRoleAsync(btUser!, role);
                    }
                }
            }
            else
            {
                // Navigate back to the View
                return RedirectToAction(nameof(ManageUserRoles));

            }

            return RedirectToAction("CompanyMembers", "Companies");   
        }

    }

}
