using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NewTiceAI.Services.Interfaces;
using NewTiceAI.Extensions;
using NewTiceAI.Models.ViewModels;
using NewTiceAI.Models.Enums;
using NewTiceAI.Helpers;

namespace NewTiceAI.Controllers
{
    [Authorize]
    public class ProjectsController : TABaseController
    {
        private readonly UserManager<TAUser> _userManager;
        private readonly IBTProjectService _projectService;
        private readonly IBTRolesService _rolesService;
        private readonly IBTFileService _fileService;
        private readonly IBTLookupService _lookupService;
        private readonly IOrganizationService _organizationService;

        public ProjectsController(UserManager<TAUser> userManager,
                                  IBTProjectService projectService,
                                  IBTRolesService rolesService,
                                  IBTFileService fileService,
                                  IBTLookupService lookupService,
                                  IOrganizationService organizationService)
        {
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;
            _fileService = fileService;
            _lookupService = lookupService;
            _organizationService = organizationService;
        }

        // GET: Projects
        public async Task<IActionResult> AllProjects()
        {
            int companyId = User.Identity!.GetOrganizationId();

            List<Project> projects = await _projectService.GetAllProjectsByOrgIdAsync(companyId);

            return View(projects);
        }

        public async Task<IActionResult> MyProjects()
        {
            string? userId = _userManager.GetUserId(User);

            List<Project> projects = await _projectService.GetUserProjectsAsync(userId!);

            return View(projects);
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> UnassignedProjects()
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            //int companyId1 = btUser.CompanyId;

            int companyId = User.Identity!.GetOrganizationId();

            List<Project> projects = await _projectService.GetUnassignedProjectsAsync(companyId);

            return View(projects);
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> ArchivedProjects()
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            //int companyId1 = btUser.CompanyId;

            int companyId = User.Identity!.GetOrganizationId();

            List<Project> projects = await _projectService.GetArchivedProjectsAsync(companyId);

            return View(projects);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignProjectManager(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetOrganizationId();

            AssignPMViewModel model = new();

            string? currentPMId = (await _projectService.GetProjectManagerAsync(id.Value))?.Id;

            model.Project = await _projectService.GetProjectByIdAsync(id.Value, companyId);
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager),companyId), "Id", "FullName", currentPMId);

            return View(model);  
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectManager(AssignPMViewModel model)
        {
            if (!string.IsNullOrEmpty(model.PMID))
            {
                await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                return RedirectToAction(nameof(Details), new { id = model.Project.Id });
            }

            return View(nameof(AssignProjectManager), new { id= model.Project!.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> AssignProjectMembers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMembersViewModel model = new();

            model.Project = await _projectService.GetProjectByIdAsync(id.Value, _organizationId);

            List<TAUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _organizationId);
            List<TAUser> submitters = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _organizationId);

            // Get available members
            List<TAUser> teamMembers = developers.Concat(submitters).ToList();

            // Get any current members
            List<string> projectMembers = model.Project.Members.Select(m=>m.Id).ToList();

            model.UsersList = new MultiSelectList(teamMembers, "Id", "FullName", projectMembers);

            return View(model);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectMembers(ProjectMembersViewModel model)
        {
            if(model.SelectedUsers != null)
            {
                List<string> memberIds = (await _projectService.GetAllProjectMembersExceptPMAsync(model.Project!.Id)).Select(m => m.Id).ToList();


                // Remove current members
                foreach(string member in memberIds)
                {
                    await _projectService.RemoveUserFromProjectAsync(member, model.Project.Id);
                }

                // Add selected members
                foreach(string member in model.SelectedUsers)
                {
                    await _projectService.AddUserToProjectAsync(member, model.Project.Id);  
                }


                return RedirectToAction(nameof(Details), new { id = model.Project!.Id });

            }

            return RedirectToAction(nameof(AssignProjectMembers), new { id = model.Project!.Id });
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //BTUser btUser = await _userManager.GetUserAsync(User);
            //int companyId = btUser.CompanyId;
            int companyId = User.Identity!.GetOrganizationId();

            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }


        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Create()
        {
            // Instantiate ViewModel
            AddProjectWithPMViewModel model = new();
                        
            // Assign model properties
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName");
            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");

            ViewData["AccountId"] = new SelectList((await _organizationService.GetAccountsAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");


            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProjectWithPMViewModel model)
        {
            if (ModelState.IsValid)
            {
                //model.Project!.AccountId = User.Identity!.GetOrganizationId();
                //model.Project.StartDate = PGDataHelper.FormatDate(model.Project.StartDate);
                //model.Project.EndDate = PGDataHelper.FormatDate(model.Project.EndDate);


                // Check for Image data
                if (model.Project?.ImageFormFile != null)
                {
                    model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                    model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                    model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                }

                model.Project!.Created = DateTime.Now;

                // Call Service method to add new project
                await _projectService.AddNewProjectAsync(model.Project!);

                // After the project has been created,
                // Allow Admin to add ProjectManager on create
                // Check for Project Manager data
                if (!string.IsNullOrEmpty(model.PMID))
                {
                    await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                }

                return RedirectToAction(nameof(AllProjects));
            }


            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName");
            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");
            ViewData["AccountId"] = new SelectList((await _organizationService.GetAccountsAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");

            return View(model.Project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddProjectWithPMViewModel model = new();

            Project project = await _projectService.GetProjectByIdAsync(id.Value, _organizationId);

            model.Project = project;

            // Get PM if one is assigned
            TAUser projectManager = await _projectService.GetProjectManagerAsync(project.Id);

            if (projectManager != null)
            {
                model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName");
            }

            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");



            if (project == null)
            {
                return NotFound();
            }


            ViewData["AccountId"] = new SelectList((await _organizationService.GetAccountsAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");

            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddProjectWithPMViewModel model)
        {
            if (model.Project == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //model.Project!.Created = DateTime.SpecifyKind(model.Project.Created, DateTimeKind.Utc);
                    //model.Project.StartDate = DateTime.SpecifyKind(model.Project.StartDate, DateTimeKind.Utc);
                    //model.Project.EndDate = DateTime.SpecifyKind(model.Project.EndDate, DateTimeKind.Utc);


                    if (model.Project?.ImageFormFile != null)
                    {
                        model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                        model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                        model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                    }


                    // Call Service method to update/edit project
                    await _projectService.UpdateProjectAsync(model.Project!);


                    // TODO: Allow Admin to edit ProjectManager
                    //
                    if (!string.IsNullOrEmpty(model.PMID))
                    {
                        await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExists(model.Project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllProjects));
            }

            // Get PM if one is assigned
            TAUser projectManager = await _projectService.GetProjectManagerAsync(model.Project.Id);

            if (projectManager != null)
            {
                model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _organizationId), "Id", "FullName");
            }

            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");
            ViewData["AccountId"] = new SelectList((await _organizationService.GetAccountsAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");

            return View(model);
        }

        // GET: Projects/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            int companyId = User.Identity!.GetOrganizationId();

            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {

            int companyId = User.Identity!.GetOrganizationId();

            var project = await _projectService.GetProjectByIdAsync(id, companyId);
            if (project != null)
            {
                // Call Service method to archive (Delete) project
                await _projectService.ArchiveProjectAsync(project);
            }

            return RedirectToAction(nameof(AllProjects));
        }


        // GET: Projects/Archive/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetOrganizationId();

            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Archive/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {

            int companyId = User.Identity!.GetOrganizationId();

            var project = await _projectService.GetProjectByIdAsync(id, companyId);
            if (project != null)
            {
                // Call Service method to archive (Delete) project
                await _projectService.RestoreProjectAsync(project);
            }

            return RedirectToAction(nameof(AllProjects));
        }


        private async Task<bool> ProjectExists(int id)
        {
            int companyId = User.Identity!.GetOrganizationId();
            return (await _projectService.GetAllProjectsByOrgIdAsync(companyId)).Any(p => p.Id == id);
        }
    }
}
