using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Extensions;
using NewTiceAI.Models;

namespace NewTiceAI.Controllers
{
    public class AccountsController : TABaseController
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context, UserManager<TAUser> userManager)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> AllAccounts()
        {
            //Org Id
            IEnumerable<Account> accounts = await _context.Accounts
                                                          .Where(a => a.ParentOrganizationId == _organizationId)
                                                          .Include(a=>a.ShippingAddress)
                                                          .Include(a=>a.Contacts).ToListAsync();

            return View(accounts);
        }

        // GET: Accounts
        public async Task<IActionResult> MyAccounts()
        {
            //string? userId = _userManager.GetUserId(User);
            TAUser? taUser = await _context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.Id == _userId);

            IEnumerable<Account> accounts = taUser!.Accounts;
            return View(accounts);
        }


        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.Include(a=>a.ParentOrganization).Include(a=>a.ShippingAddress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BTUser,Name")] Account account, bool isOrganization, Address? acctAddress)
        {

            if (ModelState.IsValid)
            {
                //account.AccountOwnerId = _userManager.GetUserId(User);
                if (isOrganization == false)
                {
                    account.ParentOrganizationId = _organizationId;
                }
                else
                {
                    Organization org = new()
                    {
                        Name = account.Name,
                        Description = account.Description
                    };
                    _context.Add(org);
                    await _context.SaveChangesAsync();

                    account.ParentOrganizationId = org.Id;
                }

                if (acctAddress != null)
                {
                    await _context.AddAsync(acctAddress);
                    await _context.SaveChangesAsync();
                    account.ShippingAddressId = acctAddress.Id;
                }

                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllAccounts));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.Include(a=>a.ShippingAddress).FirstOrDefaultAsync(a=>a.Id==id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BTUser,Name,ParentOrganizationId")] Account account, Address acctAddress)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Address updatedAddress = new()
                    {
                        Address1 = acctAddress.Address1,
                        Address2 = acctAddress.Address2,
                        City = acctAddress.City,
                        State = acctAddress.State,
                        ZipCode = acctAddress.ZipCode
                    };

                    if (acctAddress != null && account.ShippingAddressId == null)
                    {
                        updatedAddress.Id = 0;
                        await _context.AddAsync(updatedAddress);
                        await _context.SaveChangesAsync();
                        account.ShippingAddressId = updatedAddress.Id;
                    }
                    else if (account.ShippingAddressId != null && acctAddress != null)
                    {
                        updatedAddress.Id = account.ShippingAddressId.Value;
                        _context.Update(updatedAddress);
                    }

                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllAccounts));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllAccounts));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
