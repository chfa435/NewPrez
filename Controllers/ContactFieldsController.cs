using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;

namespace Tice_AI.Controllers
{
    public class ContactFieldsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactFieldsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactFields
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contacts.Include(c => c.Account).Include(c => c.Address).Include(c => c.ContactOwner).Include(c => c.CurrentDistributor).Include(c => c.Fellowship).Include(c => c.Fellowship2).Include(c => c.Mentor).Include(c => c.Organization).Include(c => c.Practice).Include(c => c.RelationshipHolder).Include(c => c.Residency).Include(c => c.SalesRepresentative);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ContactFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Account)
                .Include(c => c.Address)
                .Include(c => c.ContactOwner)
                .Include(c => c.CurrentDistributor)
                .Include(c => c.Fellowship)
                .Include(c => c.Fellowship2)
                .Include(c => c.Mentor)
                .Include(c => c.Organization)
                .Include(c => c.Practice)
                .Include(c => c.RelationshipHolder)
                .Include(c => c.Residency)
                .Include(c => c.SalesRepresentative)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: ContactFields/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name");
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["ContactOwnerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CurrentDistributorId"] = new SelectList(_context.Institutions, "Id", "Id");
            ViewData["FellowshipId"] = new SelectList(_context.Institutions, "Id", "Id");
            ViewData["Fellowship2Id"] = new SelectList(_context.Institutions, "Id", "Id");
            ViewData["MentorId"] = new SelectList(_context.Contacts, "Id", "FirstName");
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Id");
            ViewData["PracticeId"] = new SelectList(_context.Institutions, "Id", "Id");
            ViewData["RelationshipHolderId"] = new SelectList(_context.Contacts, "Id", "FirstName");
            ViewData["ResidencyId"] = new SelectList(_context.Institutions, "Id", "Id");
            ViewData["SalesRepresentativeId"] = new SelectList(_context.Contacts, "Id", "FirstName");
            return View();
        }

        // POST: ContactFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrganizationId,AccountId,AddressId,ContactOwnerId,Prefix,MiddleName,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Mobile,DateAdded,ImageData,ImageType,Title,DoNotCall,EmailOptOut,IsActive,Gender,Specialty,ResidencyId,Residency_GradYear,FellowshipId,Fellowship_GradYear,Fellowship2Id,Fellowship2_GradYear,RelationshipHolderId,CurrentDistributorId,SalesRepresentativeId,PracticeId,MentorId,LastMeetingDate,NextActivityDate,FollowupDate,HandoffConfirmed,ContactNotes,ProfileUrl,ProfileUrl2,PacketSentDate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", contact.AccountId);
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", contact.AddressId);
            ViewData["ContactOwnerId"] = new SelectList(_context.Users, "Id", "Id", contact.ContactOwnerId);
            ViewData["CurrentDistributorId"] = new SelectList(_context.Institutions, "Id", "Id", contact.CurrentDistributorId);
            ViewData["FellowshipId"] = new SelectList(_context.Institutions, "Id", "Id", contact.FellowshipId);
            ViewData["Fellowship2Id"] = new SelectList(_context.Institutions, "Id", "Id", contact.Fellowship2Id);
            ViewData["MentorId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.MentorId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Id", contact.OrganizationId);
            ViewData["PracticeId"] = new SelectList(_context.Institutions, "Id", "Id", contact.PracticeId);
            ViewData["RelationshipHolderId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.RelationshipHolderId);
            ViewData["ResidencyId"] = new SelectList(_context.Institutions, "Id", "Id", contact.ResidencyId);
            ViewData["SalesRepresentativeId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.SalesRepresentativeId);
            return View(contact);
        }

        // GET: ContactFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", contact.AccountId);
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", contact.AddressId);
            ViewData["ContactOwnerId"] = new SelectList(_context.Users, "Id", "Id", contact.ContactOwnerId);
            ViewData["CurrentDistributorId"] = new SelectList(_context.Institutions, "Id", "Id", contact.CurrentDistributorId);
            ViewData["FellowshipId"] = new SelectList(_context.Institutions, "Id", "Id", contact.FellowshipId);
            ViewData["Fellowship2Id"] = new SelectList(_context.Institutions, "Id", "Id", contact.Fellowship2Id);
            ViewData["MentorId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.MentorId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Id", contact.OrganizationId);
            ViewData["PracticeId"] = new SelectList(_context.Institutions, "Id", "Id", contact.PracticeId);
            ViewData["RelationshipHolderId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.RelationshipHolderId);
            ViewData["ResidencyId"] = new SelectList(_context.Institutions, "Id", "Id", contact.ResidencyId);
            ViewData["SalesRepresentativeId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.SalesRepresentativeId);
            return View(contact);
        }

        // POST: ContactFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationId,AccountId,AddressId,ContactOwnerId,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Mobile,DateAdded,ImageData,ImageType,Title,DoNotCall,EmailOptOut,IsActive,Gender,Specialty,ResidencyId,Residency_GradYear,FellowshipId,Fellowship_GradYear,Fellowship2Id,Fellowship2_GradYear,RelationshipHolderId,CurrentDistributorId,SalesRepresentativeId,PracticeId,MentorId,LastMeetingDate,NextActivityDate,FollowupDate,HandoffConfirmed,ContactNotes,ProfileUrl,ProfileUrl2,PacketSentDate")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", contact.AccountId);
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", contact.AddressId);
            ViewData["ContactOwnerId"] = new SelectList(_context.Users, "Id", "Id", contact.ContactOwnerId);
            ViewData["CurrentDistributorId"] = new SelectList(_context.Institutions, "Id", "Id", contact.CurrentDistributorId);
            ViewData["FellowshipId"] = new SelectList(_context.Institutions, "Id", "Id", contact.FellowshipId);
            ViewData["Fellowship2Id"] = new SelectList(_context.Institutions, "Id", "Id", contact.Fellowship2Id);
            ViewData["MentorId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.MentorId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "Id", contact.OrganizationId);
            ViewData["PracticeId"] = new SelectList(_context.Institutions, "Id", "Id", contact.PracticeId);
            ViewData["RelationshipHolderId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.RelationshipHolderId);
            ViewData["ResidencyId"] = new SelectList(_context.Institutions, "Id", "Id", contact.ResidencyId);
            ViewData["SalesRepresentativeId"] = new SelectList(_context.Contacts, "Id", "FirstName", contact.SalesRepresentativeId);
            return View(contact);
        }

        // GET: ContactFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Account)
                .Include(c => c.Address)
                .Include(c => c.ContactOwner)
                .Include(c => c.CurrentDistributor)
                .Include(c => c.Fellowship)
                .Include(c => c.Fellowship2)
                .Include(c => c.Mentor)
                .Include(c => c.Organization)
                .Include(c => c.Practice)
                .Include(c => c.RelationshipHolder)
                .Include(c => c.Residency)
                .Include(c => c.SalesRepresentative)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: ContactFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
          return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
