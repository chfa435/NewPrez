using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;

//using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

//using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Enums;
using NewTiceAI.Extensions;
using NewTiceAI.Helpers;
using NewTiceAI.Models;
using NewTiceAI.Models.Enums;
using NewTiceAI.Models.ViewModels;
using NewTiceAI.Services;
using NewTiceAI.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewTiceAI.Controllers
{
    public class ContactsController : TABaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;
        //private readonly UserManager<TAUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IEmailSender _emailService;
        private readonly IInstitutionService _institutionService;
        private readonly IBTProjectService _projectService;
        private readonly IBTLookupService _lookupService;
        private readonly IContactService _contactService;
        private readonly IBTFileService _fileService;

        public ContactsController(ApplicationDbContext context,
                  UserManager<TAUser> userManager,
                  IAccountService accountService,
                  IImageService imageService,
                  IEmailSender emailService,
                  IInstitutionService institutionService,
                  IBTProjectService projectService,
                  IBTLookupService lookupService,
                  IContactService contactService,
                  IBTFileService fileService)
        {
            _context = context;
            //_userManager = userManager;
            _accountService = accountService;
            _imageService = imageService;
            _emailService = emailService;
            _institutionService = institutionService;
            _projectService = projectService;
            _lookupService = lookupService;
            _contactService = contactService;
            _fileService = fileService;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(int? accountId, string? swalMessage = null)
        {
            ViewData["SwalMessage"] = swalMessage;

            //

            // Get the Contacts from the AppUser
            List<Contact> contacts = new List<Contact>();



            if (accountId == null)
            {
                contacts = await _context.Contacts
                                         .Where(c => c.OrganizationId == _organizationId)
                                         .Include(c => c.Address)
                                         .Include(c => c.Account)
                                         .ToListAsync();
            }
            else
            {
                contacts = (await _context.Accounts
                                          .Include(c => c.Contacts)
                                            .ThenInclude(c => c.Address)
                                          .FirstOrDefaultAsync(c => c.ParentOrganizationId == _organizationId && c.Id == accountId))!
                                          .Contacts.ToList();
            }


            ViewData["AccountsList"] = new SelectList(await _accountService.GetAccountsByOrganizationIdAsync(_organizationId), "Id", "Name", accountId);

            return View(contacts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchContacts(string? searchString)
        {
            //string? userId = _userManager.GetUserId(User);

            // Get the Contacts from the AppUser
            List<Contact> contacts = new List<Contact>();






            if (string.IsNullOrEmpty(searchString))
            {
                contacts = _context.Contacts
                                   .Where(c => c.OrganizationId == _organizationId)
                                   .OrderBy(c => c.LastName)
                                   .ThenBy(c => c.FirstName)
                                   .ToList();
            }
            else
            {
                contacts = _context!.Contacts
                                    .Where(c => c.OrganizationId == _organizationId &&
                                                c.FirstName!.ToLower().Contains(searchString.ToLower()) ||
                                                c.LastName!.ToLower().Contains(searchString.ToLower()))
                                    .Include(c => c.Address)
                                    .Include(c => c.Account)
                                    .OrderBy(c => c.LastName)
                                    .ThenBy(c => c.FirstName)
                                    .ToList();
            }

            ViewData["AccountsList"] = new SelectList(await _accountService.GetAccountsByOrganizationIdAsync(_organizationId), "Id", "Name");

            return View(nameof(Index), contacts);

        }


        // EmailContact
        public async Task<IActionResult> EmailContact(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Contact? contact = await _context.Contacts
                                             .Where(c => c.OrganizationId == _organizationId)
                                             .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            // Instantiate EmailData
            EmailData emailData = new EmailData()
            {
                EmailAddress = contact!.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName
            };

            // Instantiate the ViewModel
            EmailContactViewModel viewModel = new EmailContactViewModel()
            {
                Contact = contact,
                EmailData = emailData
            };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailContact(EmailContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? swalMessage = string.Empty;

                try
                {
                    await _emailService.SendEmailAsync(viewModel.EmailData!.EmailAddress!,
                                                       viewModel.EmailData.EmailSubject!,
                                                       viewModel.EmailData.EmailBody!);

                    swalMessage = "Success: Email Sent!";
                    return RedirectToAction(nameof(Index), new { swalMessage });
                }
                catch (Exception)
                {
                    swalMessage = "Error: Email Send Failed!";
                    return RedirectToAction(nameof(Index), new { swalMessage });
                    throw;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContactAttachment([Bind("Id,FormFile,Description,ContactId")] ContactAttachment attachment)
        {
            string statusMessage;

            ModelState.Remove("UserId");

            if (ModelState.IsValid && attachment.FormFile != null)
            {
                try
                {
                    attachment.FileData = await _fileService.ConvertFileToByteArrayAsync(attachment.FormFile);
                    attachment.FileName = attachment.FormFile.FileName;
                    attachment.FileContentType = attachment.FormFile.ContentType;

                    attachment.Created = DateTime.Now;
                    attachment.UserId = _userId;

                    await _contactService.AddContactAttachmentAsync(attachment);

                }
                catch (Exception)
                {

                    throw;
                }

                Contact contact = await _contactService.GetContactByIdAsync(attachment.ContactId);
                statusMessage = $"Success: New attachment added for {contact.FullName}.";
            }
            else
            {
                statusMessage = "Error: Invalid data.";
            }

            return RedirectToAction("Details", new { id = attachment.ContactId, message = statusMessage });
        }


        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id, string message = "")
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.ActionItems)
                .Include(c => c.Address)
                .Include(c => c.Account)
                .Include(c => c.Attachments)
                .Include(c => c.Organization)
                .Include(c => c.ContactOwner)
                .Include(c => c.RelationshipHolder)
                .Include(c => c.Mentor)
                .Include(c => c.Residency)
                .Include(c => c.Fellowship)
                .Include(c => c.Fellowship2)
                .Include(c => c.CurrentDistributor)
                .Include(c => c.SalesRepresentative)
                .Include(c => c.Practice)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            string? userId = _userId;

            ViewData["Institutions"] = new SelectList(_context.Organizations, "Id", "Name");

            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.ParentOrganizationId == _organizationId).OrderBy(a => a.Name), "Id", "Name");
            ViewData["ActorId"] = new SelectList(_context.Users.Where(c => c.OrganizationId == _organizationId).OrderBy(c => c.LastName).ThenBy(c => c.FirstName), "Id", "FullName");
            ViewData["ItemPriority"] = new SelectList(Enum.GetValues(typeof(EnumActionItemPriorities)).Cast<EnumActionItemPriorities>().ToList());
            ViewData["ItemType"] = new SelectList(Enum.GetValues(typeof(EnumActionItemTypes)).Cast<EnumActionItemTypes>().ToList());

            return View(contact);
        }


        [HttpGet]
        public IActionResult ReadExcelData()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadExcelData(IFormFile? excelFile)
        {
            IList<Contact> contacts = new List<Contact>();

            if (excelFile != null)
            {

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using var stream = new MemoryStream();
                excelFile.CopyTo(stream);
                stream.Position = 0;

                using var reader = ExcelReaderFactory.CreateReader(stream);
                //DataSet? dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                //{
                //    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                //    {
                //        UseHeaderRow = true,
                //    }
                //});


                //// Read each row of the file
                //foreach(var record in dataSet.Tables)
                //{

                //}

                //IEnumerable<Contact> myData = dataSet.Tables[0].AsEnumerable().Select(r => new Contact{
                //    FirstName = r.Field<string>("First Name"),
                //    LastName = r.Field<string>("Last Name"),
                //    Title = r.Field<string>("Title"),
                //    Gender = r.Field<Genders>("Gender"),
                //    Email = r.Field<string>("Email"),
                //    PhoneNumber = r.Field<string>("Phone"),
                //    Residency_GradYear = r.Field<int>("Residency Grad Year"),
                //    Fellowship_GradYear = r.Field<int>("Fellowship Grad Year"),
                //    OrganizationId = _organizationId,
                //    IsActive = true
                //});

                // Skip header line
                reader.Read();     
                //reader.Read();

                while (reader.Read())
                {

                    // 0 - first name
                    // 1 - last name
                    // 2 - title
                    // 3 - gender
                    // 4 - email
                    // 5 - phone
                    // 6 - residency grad year
                    // 7 - fellowship grad year

                    string? firstName = reader.GetValue(0) == null ? null : reader.GetValue(0).ToString();
                    string? lastName = reader.GetValue(1) == null ? null : reader.GetValue(1).ToString();
                    string? title = reader.GetValue(2) == null ? null : reader.GetValue(2).ToString();
                    Genders? gender = reader.GetValue(3) == null ? null : (Genders)Enum.Parse(typeof(Genders), reader.GetValue(3).ToString()!);
                    string? email = reader.GetValue(4) == null ? null : reader.GetValue(4).ToString();
                    string? phoneNumber = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString();
                    int? residencyGradYear = reader.GetValue(6) == null ? null : int.Parse(reader.GetValue(6).ToString()!);
                    int? fellowshipGradYear = reader.GetValue(7) == null ? null : int.Parse(reader.GetValue(7).ToString()!);

                    contacts.Add(new Contact
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Title = title,
                        Gender = gender,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Residency_GradYear = residencyGradYear,
                        Fellowship_GradYear = fellowshipGradYear,
                        OrganizationId = _organizationId,
                        IsActive = true
                    });
                }

                await _contactService.LoadContactsAsync(contacts);
            }
            ViewData["PageTitle"] = "New Contacts Import";
            return View(nameof(Index), contacts);

        }

        // GET: Contacts/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList());
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            return View(new Contact());
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrganizationId,AccountId,AddressId,ContactOwnerId,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Mobile,DateAdded,ImageFile,Title,DoNotCall,EmailOptOut,IsActive,Gender,Specialty,ResidencyId,Residency_GradYear,FellowshipId,Fellowship_GradYear,Fellowship2Id,Fellowship2_GradYear,RelationshipHolderId,CurrentDistributorId,SalesRepresentativeId,PracticeId,MentorId,LastMeetingDate,NextActivityDate,FollowupDate,HandoffConfirmed,ContactNotes,ProfileUrl,ProfileUrl2,PacketSentDate")] Contact contact, Address address)
        {

            ModelState.Remove("OrganizationId");
            if (ModelState.IsValid)
            {
                contact.OrganizationId = User.Identity!.GetOrganizationId();
                //contact.DateAdded = DateTime.UtcNow;
                if (address != null)
                {
                    await _context.AddAsync(address);
                    await _context.SaveChangesAsync();
                    contact.AddressId = address.Id;
                }

                if (contact.ImageFile != null)
                {
                    //contact.ImageData = await _imageService.ConvertFileToByteArrayAsync(contact.ImageFile);
                    //contact.ImageType = contact.ImageFile.ContentType;
                    FileUpload? image = await FileUploader.GetFileUploadAsync(contact.ImageFile);
                    // Add teh new image to the database
                    await _context.FileUploads.AddAsync(image);
                    await _context.SaveChangesAsync();

                    contact.ImageId = image.Id;
                }


                contact.IsActive = true;

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList());
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            return View(contact);
        }


        // GET: Contacts/Create
        public async Task<IActionResult> CreateNew()
        {
            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList());
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            return View(new Contact());
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew([Bind("Id,OrganizationId,AccountId,AddressId,ContactOwnerId,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Mobile,DateAdded,ImageFile,Title,DoNotCall,EmailOptOut,IsActive,Gender,Specialty,ResidencyId,Residency_GradYear,FellowshipId,Fellowship_GradYear,Fellowship2Id,Fellowship2_GradYear,RelationshipHolderId,CurrentDistributorId,SalesRepresentativeId,PracticeId,MentorId,LastMeetingDate,NextActivityDate,FollowupDate,HandoffConfirmed,ContactNotes,ProfileUrl,ProfileUrl2,PacketSentDate")] Contact contact, Address address)
        {

            ModelState.Remove("OrganizationId");
            if (ModelState.IsValid)
            {
                contact.OrganizationId = User.Identity!.GetOrganizationId();
                //contact.DateAdded = DateTime.UtcNow;
                if (address != null)
                {
                    await _context.AddAsync(address);
                    await _context.SaveChangesAsync();
                    contact.AddressId = address.Id;
                }

                if (contact.ImageFile != null)
                {
                    //contact.ImageData = await _imageService.ConvertFileToByteArrayAsync(contact.ImageFile);
                    //contact.ImageType = contact.ImageFile.ContentType;
                    FileUpload? image = await FileUploader.GetFileUploadAsync(contact.ImageFile);
                    // Add teh new image to the database
                    await _context.FileUploads.AddAsync(image);
                    await _context.SaveChangesAsync();

                    contact.ImageId = image.Id;
                }


                contact.IsActive = true;

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList());
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            return View(contact);
        }



        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Address)
                .Include(c => c.Account)
                .Include(c => c.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contact == null)
            {
                return NotFound();
            }



            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList(), "Key", "Value");
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            ViewData["Years"] = new SelectList(Enumerable.Range(1920, DateTime.Now.Year - 1919).OrderDescending());
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationId,AccountId,AddressId,ContactOwnerId,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Mobile,DateAdded,ImageFile,ImageData,ImageType,Title,DoNotCall,EmailOptOut,IsActive,Gender,Specialty,ResidencyId,Residency_GradYear,FellowshipId,Fellowship_GradYear,Fellowship2Id,Fellowship2_GradYear,RelationshipHolderId,CurrentDistributorId,SalesRepresentativeId,PracticeId,MentorId,LastMeetingDate,NextActivityDate,FollowupDate,HandoffConfirmed,ContactNotes,ProfileUrl,ProfileUrl2,PacketSentDate")] Contact contact, UpdateAddressViewModel? address)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //contact.DateAdded = DateTime.SpecifyKind(contact.DateAdded, DateTimeKind.Utc);
                    Address updatedAddress = new();
                    if (address != null && contact.AddressId == null)
                    {
                        updatedAddress = new()
                        {
                            Id = 0,
                            Address1 = address.Address1,
                            Address2 = address.Address2,
                            City = address.City,
                            State = address.State,
                            ZipCode = address.ZipCode
                        };
                        await _context.AddAsync(updatedAddress);
                        await _context.SaveChangesAsync();

                    }
                    else if (contact.AddressId != null && address != null)
                    {
                        updatedAddress = new()
                        {
                            Id = contact.AddressId.Value,
                            Address1 = address.Address1,
                            Address2 = address.Address2,
                            City = address.City,
                            State = address.State,
                            ZipCode = address.ZipCode
                        };
                        _context.Update(updatedAddress);
                        await _context.SaveChangesAsync();
                    }

                    contact.AddressId = updatedAddress.Id;

                    if (contact.ImageFile != null)
                    {
                        //contact.ImageData = await _imageService.ConvertFileToByteArrayAsync(contact.ImageFile);
                        //contact.ImageType = contact.ImageFile.ContentType;
                        FileUpload? image = await FileUploader.GetFileUploadAsync(contact.ImageFile);
                        // Add teh new image to the database
                        await _context.FileUploads.AddAsync(image);
                        await _context.SaveChangesAsync();

                        contact.ImageId = image.Id;
                    }

                    if (contact.DateOfBirth != null)
                    {
                        contact.DateOfBirth = DateTime.SpecifyKind(contact.DateOfBirth.Value, DateTimeKind.Utc);
                    }


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
                return RedirectToAction(nameof(Details), new { id = contact.Id });
            }

            ViewData["GendersList"] = new SelectList(Enum.GetValues(typeof(Genders)).Cast<Genders>().ToList());
            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());
            ViewData["AccountsList"] = new SelectList((await _accountService.GetAccountsByOrganizationIdAsync(_organizationId)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["InstitutionsList"] = new SelectList((await _institutionService.GetInstitutionsAsync()).OrderBy(i => i.Name), "Id", "Name");
            ViewData["ContactsList"] = new SelectList((await _context.Contacts.Where(c => c.OrganizationId == _organizationId).ToListAsync()).OrderBy(c => c.FullName), "Id", "FullName");

            return View(contact);
        }

        [HttpPost]
        public async Task<JsonResult> EmailOption(int? contactId, string? option)
        {
            try
            {
                Contact? contact = _context.Contacts.Find(contactId);

                if (contact != null)
                {
                    contact.EmailOptOut = !contact.EmailOptOut;
                    await _context.SaveChangesAsync();

                    return Json(contact.EmailOptOut);
                }
                return Json(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
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
            return _context.Contacts.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ShowFile(int id)
        {
            ContactAttachment contactAttachment = await _contactService.GetContactAttachmentByIdAsync(id);
            string? fileName = contactAttachment.FileName;
            byte[]? fileData = contactAttachment.FileData;
            string? ext = Path.GetExtension(fileName)!.Replace(".", "");

            Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
            return File(fileData!, $"application/{ext}");
        }
    }
}
