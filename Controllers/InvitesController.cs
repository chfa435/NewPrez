using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using NewTiceAI.Extensions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace NewTiceAI.Controllers
{
    [Authorize]
    public class InvitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        private readonly IBTInviteService _inviteService;
        private readonly UserManager<TAUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly IEmailSender _emailService;

    
        public InvitesController(ApplicationDbContext context,
                                 IBTProjectService projectService,
                                 IOrganizationService organizationService,
                                 IBTInviteService inviteService,
                                 UserManager<TAUser> userManager,
                                 IDataProtectionProvider dataProtectionProvider,
                                 IEmailSender emailService)
        {
            _context = context;
            _projectService = projectService;
            _organizationService = organizationService;
            _inviteService = inviteService;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector("CF.GENESIS.BugTr@cker.2022");
            _emailService = emailService;
        }

        // GET: Invites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invites.Include(i => i.Organization).Include(i => i.Invitee).Include(i => i.Invitor).Include(i => i.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invites == null)
            {
                return NotFound();
            }

            var invite = await _context.Invites
                .Include(i => i.Organization)
                .Include(i => i.Invitee)
                .Include(i => i.Invitor)
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invite == null)
            {
                return NotFound();
            }

            return View(invite);
        }

        [Authorize(Roles="Admin")]
        // GET: Invites/Create
        public async Task<IActionResult> Create()
        {
            int organizationId = User.Identity!.GetOrganizationId();


            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByOrgIdAsync(organizationId), "Id", "Name");

            return View();
        }

        // POST: Invites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,InviteeEmail,InviteeFirstName,InviteeLastName,Message")] Invite invite)
        {
            ModelState.Remove("InvitorId");

                int orgId = User.Identity!.GetOrganizationId();

            if (ModelState.IsValid)
            {
                try
                {
                    Organization organization = await _organizationService.GetOrgInfoById(orgId);
                    
                    Guid guid = Guid.NewGuid();

                    string p_token = _protector.Protect(guid.ToString());
                    string p_email = _protector.Protect(invite.InviteeEmail!);
                    string p_organization = _protector.Protect(organization.Id.ToString());

                    string? callbackUrl = Url.Action("ProcessInvite", "Invites", new { p_token, p_email, p_organization }, protocol: Request.Scheme);

                    string body = $@"{invite.Message} <br />
                              Please join my Company. <br />
                              Click the following link to join our team. <br />
                              <a href=""{callbackUrl}"">COLLABORATE</a>";

                    string? destination = invite.InviteeEmail;

                    string subject = $" Genesis Tracker: {organization.Name} Invite";

                    await _emailService.SendEmailAsync(destination!, subject, body);


                    // Save invite in the DB
                    invite.OrganizationToken = guid;
                    invite.OrganizationId = orgId;
                    invite.InviteDate = DateTime.UtcNow;
                    invite.InvitorId = _userManager.GetUserId(User);
                    invite.IsValid = true;

                    await _inviteService.AddNewInviteAsync(invite);


                    return RedirectToAction("Dashboard_new", "Home", new { swalMessage = "Invitation Sent!"});

                }
                catch (Exception)
                {

                    throw;
                }
               
            }

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByOrgIdAsync(orgId), "Id", "Name");

            return View(invite);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ProcessInvite(string p_token, string p_email, string p_organization)
        {
            if(p_token == null)
            {
                return NotFound();
            }

            Guid organizationToken = Guid.Parse(_protector.Unprotect(p_token));
            string inviteeEmail = _protector.Unprotect(p_email);
            int organizationId = int.Parse(_protector.Unprotect(p_organization));

            try
            {
                Invite invite = await _inviteService.GetInviteAsync(organizationToken, inviteeEmail, organizationId);

                if (invite !=null)
                {
                    return View(invite);
                }
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
