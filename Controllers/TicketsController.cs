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
using NewTiceAI.Extensions;
using NewTiceAI.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using NewTiceAI.Models.ViewModels;

namespace NewTiceAI.Controllers
{
    [Authorize]
    public class TicketsController : TABaseController
    {
        private readonly IBTTicketService _ticketService;
        private readonly UserManager<TAUser> _userManager;
        private readonly IBTProjectService _projectService;
        private readonly IBTTicketHistoryService _ticketHistoryService;
        private readonly IBTNotificationService _notificationService;
        private readonly IBTLookupService _lookupService;
        private readonly IBTFileService _fileService;

        public TicketsController(IBTTicketService ticketService,
                                 UserManager<TAUser> userManager,
                                 IBTProjectService projectService,
                                 IBTTicketHistoryService ticketHistoryService,
                                 IBTNotificationService notificationService,
                                 IBTLookupService lookupService,
                                 IBTFileService fileService)
        {
            _ticketService = ticketService;
            _userManager = userManager;
            _projectService = projectService;
            _ticketHistoryService = ticketHistoryService;
            _notificationService = notificationService;
            _lookupService = lookupService;
            _fileService = fileService;
        }



        // GET: All Tickets
        public async Task<IActionResult> AllTickets()
        {
            List<Ticket> model = (await _ticketService.GetAllTicketsByCompanyIdAsync(_organizationId))
                                                      .OrderByDescending(p => p.Updated).ThenByDescending(p => p.Created).ToList();
            return View(model);
        }

        // GET: My Tickets
        public async Task<IActionResult> MyTickets()
        {
            // ????
            string? userId = _userManager.GetUserId(User);
            //int companyId = User.Identity!.GetOrganizationId();

            List<Ticket> tickets = await _ticketService.GetTicketsByUserIdAsync(userId!, _organizationId);

            //   - OR -

            // ????
            //BTUser btUser = await _userManager.GetUserAsync(User);

            //List<Ticket> tickets = await _ticketService.GetTicketsByUserIdAsync(btUser.Id, btUser.CompanyId);


            return View(tickets);
        }

        // GET: Archived Tickets
        public async Task<IActionResult> ArchivedTickets()
        {
            int companyId = User.Identity!.GetOrganizationId();

            List<Ticket> model = await _ticketService.GetArchivedTicketsAsync(companyId);
            return View(model);
        }


        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpGet]
        public async Task<IActionResult> AssignDeveloper(int? ticketId)
        {
            if (ticketId == null)
            {
                return NotFound();
            }

            AssignDeveloperViewModel model = new();

            model.Ticket = await _ticketService.GetTicketByIdAsync(ticketId.Value);
            model.Developers = new SelectList(await _projectService.GetProjectMembersByRoleAsync(model.Ticket.ProjectId!.Value, nameof(BTRoles.Developer)), "Id", "FullName");


            return View(model);

        }

        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDeveloper(AssignDeveloperViewModel model)
        {
            TAUser? btUser = await _userManager.GetUserAsync(User);
            if (model.DeveloperId != null)
            {

                string? userId = _userManager.GetUserId(User);
                Ticket oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket!.Id);

                try
                {
                    await _ticketService.AssignTicketAsync(model.Ticket.Id, model.DeveloperId);
                }
                catch (Exception)
                {

                    throw;
                }

                // Add Ticket History
                Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket.Id);
                await _ticketHistoryService.AddHistoryAsync(oldTicket, newTicket, userId!);

                // Add Ticket Notification
                Notification notification = new()
                {
                    TicketId = model.Ticket.Id,
                    NotificationTypeId = (await _lookupService.LookupNotificationTypeIdAsync(nameof(BTNotificationType.Ticket))).Value,
                    Title = "Ticketr Assigned",
                    Message = $"Ticket : {model.Ticket.Title}, was assigned by {btUser?.FullName}",
                    SenderId = userId,
                    RecipientId = model.Ticket.DeveloperUserId
                };


                await _notificationService.AddNotificationAsync(notification);
                await _notificationService.SendEmailNotificationAsync(notification, "Ticket Assigned");


                return RedirectToAction(nameof(Details), new { id = model.Ticket.Id });

            }

            return RedirectToAction(nameof(AssignDeveloper), new { ticketId = model.Ticket!.Id });
        }



        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);


            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketComment([Bind("Id,TicketId,Comment")] TicketComment ticketComment)
        {
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                try
                {
                    ticketComment.UserId = _userManager.GetUserId(User);
                    ticketComment.Created = DateTime.UtcNow;

                    await _ticketService.AddTicketCommentAsync(ticketComment);

                    //Add history
                    await _ticketHistoryService.AddHistoryAsync(ticketComment.TicketId, nameof(TicketComment), ticketComment?.UserId!);
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return RedirectToAction("Details", new { id = ticketComment?.TicketId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketAttachment([Bind("Id,FormFile,Description,TicketId")] TicketAttachment ticketAttachment)
        {
            string statusMessage;

            ModelState.Remove("UserId");
            if (ModelState.IsValid && ticketAttachment.FormFile != null)
            {
                try
                {
                    ticketAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(ticketAttachment.FormFile);
                    ticketAttachment.FileName = ticketAttachment.FormFile.FileName;
                    ticketAttachment.FileContentType = ticketAttachment.FormFile.ContentType;

                    ticketAttachment.Created = DateTime.UtcNow;
                    ticketAttachment.UserId = _userManager.GetUserId(User);

                    await _ticketService.AddTicketAttachmentAsync(ticketAttachment);

                    //Add history
                    await _ticketHistoryService.AddHistoryAsync(ticketAttachment.TicketId, nameof(TicketAttachment), ticketAttachment.UserId!);

                }
                catch (Exception)
                {

                    throw;
                }

                statusMessage = "Success: New attachment added to Ticket.";
            }
            else
            {
                statusMessage = "Error: Invalid data.";
            }

            return RedirectToAction("Details", new { id = ticketAttachment.TicketId, message = statusMessage });
        }


        public async Task<IActionResult> ShowFile(int id)
        {
            TicketAttachment ticketAttachment = await _ticketService.GetTicketAttachmentByIdAsync(id);
            string? fileName = ticketAttachment.FileName;
            byte[]? fileData = ticketAttachment.FileData;
            string? ext = Path.GetExtension(fileName)!.Replace(".", "");

            Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
            return File(fileData!, $"application/{ext}");
        }


        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            //int companyId = User.Identity!.GetOrganizationId();
            string? userId = _userManager.GetUserId(User);

            if (User.IsInRole(nameof(BTRoles.Admin)))
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByOrgIdAsync(_organizationId), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetUserProjectsAsync(userId!), "Id", "Name");
            }


            ViewData["TicketPriorityId"] = new SelectList(await _lookupService.GetTicketPrioritiesAsync(), "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(await _lookupService.GetTicketTypesAsync(), "Id", "Name");


            Ticket model = new() { SubmitterUserId = userId};
            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Note,ProjectId,TicketTypeId,TicketPriorityId,SubmitterUserId")] Ticket ticket)
        {
            string? userId = _userManager.GetUserId(User);
            //int companyId = User.Identity!.GetOrganizationId();
            TAUser? btUser = await _userManager.GetUserAsync(User);


            if (ModelState.IsValid)
            {

                try
                {
                    ticket.Created = DateTime.UtcNow;
                    ticket.SubmitterUserId = userId;

                    ticket.TicketStatusId = (await _lookupService.LookupTicketStatusIdAsync(nameof(BTTicketStatuses.New))).Value;

                    await _ticketService.AddNewTicketAsync(ticket);


                    //TODO:  Ticket History
                    Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);
                    await _ticketHistoryService.AddHistoryAsync(null!, newTicket, userId!);

                    //TODO: Ticket Notification
                    TAUser projectManager = await _projectService.GetProjectManagerAsync(ticket.ProjectId!.Value);

                    Notification notification = new()
                    {
                        NotificationTypeId = (await _lookupService.LookupNotificationTypeIdAsync(nameof(BTNotificationType.Ticket))).Value,
                        TicketId = ticket.Id,
                        Title = "New Ticket Added",
                        Message = $"New Ticket: {ticket.Title}, was created by {btUser?.FullName}",
                        Created = DateTime.UtcNow,
                        SenderId = userId,
                        RecipientId = projectManager?.Id
                    };


                    await _notificationService.AddNotificationAsync(notification);
                    if (projectManager != null)
                    {
                        await _notificationService.SendEmailNotificationAsync(notification, $"New Ticket Added For Project: {ticket.Project!.Name}");
                    }
                    else
                    {
                        await _notificationService.SendEmailNotificationsByRoleAsync(notification, _organizationId, nameof(BTRoles.Admin));
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }


            if (User.IsInRole(nameof(BTRoles.Admin)))
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByOrgIdAsync(_organizationId), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetUserProjectsAsync(userId!), "Id", "Name");
            }

            ViewData["TicketPriorityId"] = new SelectList(await _lookupService.GetTicketPrioritiesAsync(), "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(await _lookupService.GetTicketTypesAsync(), "Id", "Name");


            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }


            ViewData["TicketPriorities"] = new SelectList(await _lookupService.GetTicketPrioritiesAsync(), "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatuses"] = new SelectList(await _lookupService.GetTicketStatusesAsync(), "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypes"] = new SelectList(await _lookupService.GetTicketTypesAsync(), "Id", "Name", ticket.TicketTypeId);



            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Note,Created,Archived,ArchivedByProject,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,SubmitterUserId,DeveloperUserId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                string? userId = _userManager.GetUserId(User);
                Ticket oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);

                try
                {
                    ticket.Created= DateTime.SpecifyKind(ticket.Created, DateTimeKind.Utc); 
                    ticket.Updated = DateTime.UtcNow;
                    await _ticketService.UpdateTicketAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                //TODO: Ticket History
                Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);
                await _ticketHistoryService.AddHistoryAsync(oldTicket, newTicket, userId!);



                return RedirectToAction(nameof(AllTickets));
            }

            ViewData["TicketPriorities"] = new SelectList(await _lookupService.GetTicketPrioritiesAsync(), "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatuses"] = new SelectList(await _lookupService.GetTicketStatusesAsync(), "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypes"] = new SelectList(await _lookupService.GetTicketTypesAsync(), "Id", "Name", ticket.TicketTypeId);

            return View(ticket);
        }

        // GET: Tickets/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);
            ticket.Archived = true;
            await _ticketService.UpdateTicketAsync(ticket);


            return RedirectToAction(nameof(AllTickets));
        }

        // GET: Tickets/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);
            await _ticketService.RestoreTicketAsync(ticket);


            return RedirectToAction(nameof(AllTickets));
        }

        private async Task<bool> TicketExists(int id)
        {
            int companyId = User.Identity!.GetOrganizationId();

            return (await _ticketService.GetAllTicketsByCompanyIdAsync(companyId)).Any(t => t.Id == id);
        }
    }
}
