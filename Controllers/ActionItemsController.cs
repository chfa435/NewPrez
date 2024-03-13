using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Services;
using NewTiceAI.Services.Interfaces;

namespace NewTiceAI.Controllers
{
    public class ActionItemsController : TABaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _userManager;
        private readonly IBTLookupService _lookupService;
        private readonly IBTNotificationService _notificationService;
        private readonly IBTProjectService _projectService;
        private readonly IBTTicketService _ticketService;
        private readonly IBTTicketHistoryService _ticketHistoryService;
        private readonly IActionItemService _actionItemService;

        public ActionItemsController(ApplicationDbContext context,
                                     UserManager<TAUser> userManager,
                                     IBTProjectService projectService,
                                     IBTTicketService ticketService,
                                     IBTLookupService lookupService,
                                     IBTTicketHistoryService ticketHistoryService,
                                     IBTNotificationService notificationService,
                                     IActionItemService actionItemService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _ticketService = ticketService;
            _lookupService = lookupService;
            _ticketHistoryService = ticketHistoryService;
            _notificationService = notificationService;
            _actionItemService = actionItemService;
        }

        // GET: ActionItems
        public async Task<IActionResult> Index()
        {
            List<ActionItem> model = (await _actionItemService.GetItemsByOrgIdAsync(_organizationId))
                                          .OrderByDescending(p => p.Updated).ThenByDescending(p => p.Created).ToList();
            return View(model);
        }

        // GET: ActionItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActionItems == null)
            {
                return NotFound();
            }

            var actionItem = await _context.ActionItems
                .Include(a => a.Account)
                .Include(a => a.Actor)
                .Include(a => a.Contact)
                .Include(a => a.Submitter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionItem == null)
            {
                return NotFound();
            }

            return View(actionItem);
        }

        // GET: ActionItems/Create
        //public IActionResult Create()
        //{
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name");
        //    ViewData["ActorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "FirstName");
        //    ViewData["ItemPriorityId"] = new SelectList(_context.ActionItemsPriorities, "Id", "Name");
        //    ViewData["ItemStatusId"] = new SelectList(_context.ActionItemsStatuses, "Id", "Name");
        //    ViewData["ItemTypeId"] = new SelectList(_context.ActionItemsTypes, "Id", "Name");
        //    ViewData["SubmitterId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        // POST: ActionItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ContactId,SubmitterId,ActorId,AccountId,ItemTypeId,ItemStatusId,ItemPriorityId,Title,Description,Note,Created,Updated,Archived")] ActionItem actionItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(actionItem);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", actionItem.AccountId);
        //    ViewData["ActorId"] = new SelectList(_context.Users, "Id", "Id", actionItem.ActorId);
        //    ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "FirstName", actionItem.ContactId);
        //    ViewData["ItemPriorityId"] = new SelectList(_context.ActionItemsPriorities, "Id", "Name", actionItem.ItemPriorityId);
        //    ViewData["ItemStatusId"] = new SelectList(_context.ActionItemsStatuses, "Id", "Name", actionItem.ItemStatusId);
        //    ViewData["ItemTypeId"] = new SelectList(_context.ActionItemsTypes, "Id", "Name", actionItem.ItemTypeId);
        //    ViewData["SubmitterId"] = new SelectList(_context.Users, "Id", "Id", actionItem.SubmitterId);
        //    return View(actionItem);
        //}
        // GET: Tickets/Create
        public IActionResult Create()
        {
            //int companyId = User.Identity!.GetOrganizationId();
            //string? userId = _userManager.GetUserId(User);

            //if (User.IsInRole(nameof(BTRoles.Admin)))
            //{
            //    ViewData["AccountId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(_organizationId), "Id", "Name");
            //}
            //else
            //{
            //    ViewData["AccountId"] = new SelectList(await _projectService.GetUserProjectsAsync(userId), "Id", "Name");
            //}

            ViewData["ActorId"] = new SelectList(_context.Users.Where(u=>u.OrganizationId==_organizationId).OrderBy(u=>u.LastName).ThenBy(u=>u.FirstName), "Id", "FullName");
            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.ParentOrganizationId == _organizationId).OrderBy(a=>a.Name), "Id", "Name");
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(c => c.OrganizationId == _organizationId).OrderBy(c=>c.LastName).ThenBy(c=>c.FirstName), "Id", "FullName");
            ViewData["ItemPriority"] = new SelectList(Enum.GetValues(typeof(EnumActionItemPriorities)).Cast<EnumActionItemPriorities>().ToList());
            ViewData["ItemType"] = new SelectList(Enum.GetValues(typeof(EnumActionItemTypes)).Cast<EnumActionItemTypes>().ToList());

            string? userId = _userManager.GetUserId(User);
            return View(new ActionItem() { SubmitterId = userId});
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContactId,SubmitterId,ActorId,AccountId,ItemType,ItemPriority,Title,Description,Note")] ActionItem actionItem)
        {
            string? userId = _userManager.GetUserId(User);
            TAUser? btUser = await _userManager.GetUserAsync(User);

            //ModelState.ClearValidationState("SubmitterId");
            ModelState.Remove("SubmitterId");
            if (ModelState.IsValid)
            {
                try
                {
                    actionItem.Created = DateTime.Now;
                    actionItem.SubmitterId = userId;

                    if(actionItem.ActorId == null) actionItem.ActorId = userId; 

                    actionItem.ItemStatus = EnumActionItemStatuses.New;

                    //await _ticketService.AddNewTicketAsync(actionItem);

                    //_context.Add(actionItem);
                    //await _context.SaveChangesAsync();

                    await _actionItemService.AddAsync(actionItem);  

                    //TODO:  Ticket History
                    //Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);
                    //await _ticketHistoryService.AddHistoryAsync(null!, newTicket, userId);

                    //TODO: Ticket Notification
                    //TAUser projectManager = await _projectService.GetProjectManagerAsync(ticket.ProjectId!.Value);

                    //Notification notification = new()
                    //{
                    //    NotificationTypeId = (await _lookupService.LookupNotificationTypeIdAsync(nameof(BTNotificationType.Ticket))).Value,
                    //    TicketId = ticket.Id,
                    //    Title = "New Ticket Added",
                    //    Message = $"New Ticket: {ticket.Title}, was created by {btUser.FullName}",
                    //    Created = DateTime.UtcNow,
                    //    SenderId = userId,
                    //    RecipientId = projectManager?.Id
                    //};


                    //await _notificationService.AddNotificationAsync(notification);
                    //if (projectManager != null)
                    //{
                    //    await _notificationService.SendEmailNotificationAsync(notification, $"New Ticket Added For Project: {ticket.Project!.Name}");
                    //}
                    //else
                    //{
                    //    await _notificationService.SendEmailNotificationsByRoleAsync(notification, _organizationId, nameof(BTRoles.Admin));
                    //}
                }
                catch (Exception)
                {

                    throw;
                }

                return RedirectToAction("Details", "Contacts", new { id = actionItem.ContactId });
            }


            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.ParentOrganizationId == _organizationId), "Id", "Name");
            ViewData["ActorId"] = new SelectList(_context.Contacts.Where(c => c.OrganizationId == _organizationId), "Id", "FullName");
            ViewData["ItemPriority"] = new SelectList(Enum.GetValues(typeof(EnumActionItemPriorities)).Cast<EnumActionItemPriorities>().ToList());
            ViewData["ItemType"] = new SelectList(Enum.GetValues(typeof(EnumActionItemTypes)).Cast<EnumActionItemTypes>().ToList());



            return View(actionItem);
        }


        // GET: ActionItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActionItems == null)
            {
                return NotFound();
            }

            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", actionItem.AccountId);
            ViewData["ActorId"] = new SelectList(_context.Users.Where(u=>u.OrganizationId==_organizationId), "Id", "FullName", actionItem.ActorId);
            //ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "FirstName", actionItem.ContactId);
            //ViewData["ItemStatus"] = new SelectList(Enum.GetValues(typeof(EnumActionItemStatuses)).Cast<EnumActionItemStatuses>().ToList());
            //ViewData["ItemPriority"] = new SelectList(Enum.GetValues(typeof(EnumActionItemPriorities)).Cast<EnumActionItemPriorities>().ToList());
            //ViewData["ItemType"] = new SelectList(Enum.GetValues(typeof(EnumActionItemTypes)).Cast<EnumActionItemTypes>().ToList());
            //ViewData["SubmitterId"] = new SelectList(_context.Users, "Id", "Id", actionItem.SubmitterId);
            return View(actionItem);
        }

        // POST: ActionItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactId,SubmitterId,ActorId,AccountId,ItemTypeId,ItemStatusId,ItemPriorityId,Title,Description,Note,Created,Updated,Archived")] ActionItem actionItem)
        {
            if (id != actionItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    actionItem.Updated = DateTime.Now;

                    //_context.Update(actionItem);
                    //await _context.SaveChangesAsync();

                    await _actionItemService.UpdateAsync(actionItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionItemExists(actionItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", actionItem.AccountId);
            ViewData["ActorId"] = new SelectList(_context.Users.Where(u => u.OrganizationId == _organizationId), "Id", "FullName", actionItem.ActorId);
            //ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "FirstName", actionItem.ContactId);
            //ViewData["ItemStatus"] = new SelectList(Enum.GetValues(typeof(EnumActionItemStatuses)).Cast<EnumActionItemStatuses>().ToList());
            //ViewData["ItemPriority"] = new SelectList(Enum.GetValues(typeof(EnumActionItemPriorities)).Cast<EnumActionItemPriorities>().ToList());
            //ViewData["ItemType"] = new SelectList(Enum.GetValues(typeof(EnumActionItemTypes)).Cast<EnumActionItemTypes>().ToList());
            //ViewData["SubmitterId"] = new SelectList(_context.Users, "Id", "Id", actionItem.SubmitterId);
            return View(actionItem);
        }

        // GET: ActionItems/Delete/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.ActionItems == null)
            {
                return NotFound();
            }

            var actionItem = await _context.ActionItems
                .Include(a => a.Account)
                .Include(a => a.Actor)
                .Include(a => a.Contact)
                .Include(a => a.Submitter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionItem == null)
            {
                return NotFound();
            }

            return View(actionItem);
        }

        // POST: ActionItems/Delete/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            if (_context.ActionItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ActionItem'  is null.");
            }
            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem != null)
            {
                _context.ActionItems.Remove(actionItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionItemExists(int id)
        {
            return (_context.ActionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
