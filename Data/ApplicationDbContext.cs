using NewTiceAI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace NewTiceAI.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}


        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>()
                        .HasOne(c => c.RelationshipHolder)
                        .WithMany()
                        .HasForeignKey(c => c.RelationshipHolderId);
            modelBuilder.Entity<Contact>()
                        .HasOne(c => c.SalesRepresentative)
                        .WithMany()
                        .HasForeignKey(c => c.SalesRepresentativeId);
            modelBuilder.Entity<Contact>()
                        .HasOne(c => c.Mentor)
                        .WithMany()
                        .HasForeignKey(c => c.MentorId);
        }

        #endregion

        public virtual DbSet<FileUpload> FileUploads { get; set; } = default!;


        public virtual DbSet<Account> Accounts { get; set; } = default!;
        public virtual DbSet<ActionProject> ActionProjects { get; set; } = default!;
        public virtual DbSet<ActionItem> ActionItems { get; set; } = default!;
        public virtual DbSet<ActionItemAttachment> ActionItemAttachments { get; set; } = default!;
        public virtual DbSet<ActionItemComment> ActionItemComments { get; set; } = default!;
        public virtual DbSet<ActionItemPriority> ActionItemPriorities { get; set; } = default!;
        public virtual DbSet<ActionItemStatus> ActionItemStatuses { get; set; } = default!;
        public virtual DbSet<ActionItemType> ActionItemTypes { get; set; } = default!;
        public virtual DbSet<ActionItemHistory> ActionItemHistories { get; set; } = default!;
        public virtual DbSet<ContactAttachment> ContactAttachments { get; set; } = default!;


        public virtual DbSet<Company> Companies { get; set; } = default!;
        public virtual DbSet<Invite> Invites { get; set; } = default!;
        public virtual DbSet<Project> Projects { get; set; } = default!;
        public virtual DbSet<Notification> Notifications { get; set; } = default!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = default!;
        public virtual DbSet<ProjectPriority> ProjectPriorities { get; set; } = default!;
        public virtual DbSet<Ticket> Tickets { get; set; } = default!;
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; } = default!;
        public virtual DbSet<TicketTag> TicketTags { get; set; } = default!;
        public virtual DbSet<TicketComment> TicketComments { get; set; } = default!;
        public virtual DbSet<TicketPriority> TicketPriorities { get; set; } = default!;
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; } = default!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = default!;
        public virtual DbSet<TicketHistory> TicketHistories { get; set; } = default!;
        public virtual DbSet<Organization> Organizations { get; set; } = default!;
        public virtual DbSet<Address> Addresses { get; set; } = default!;
        public virtual DbSet<Contact> Contacts { get; set; } = default!;
        public virtual DbSet<Institution> Institutions { get; set; } = default!;
    }
}