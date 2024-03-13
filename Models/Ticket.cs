using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class Ticket
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key - Allow create on submit
        //public int? ContactId { get; set; }

        // Foreign Key
        [Required]
        public string? SubmitterUserId { get; set; }

        // Foreign Key
        public string? DeveloperUserId { get; set; }

        // Foreign Key
        public int? ProjectId { get; set; }

        // Foreign Key
        public int? AccountId { get; set; }

        // Foreign Key
        public int TicketTypeId { get; set; }

        // Foreign Key
        public int TicketStatusId { get; set; }

        // Foreign Key
        public int TicketPriorityId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Ticket Title")]
        public string? Title { get; set; }

        [Required]
        [StringLength(2000)]
        [DisplayName("Ticket Description")]
        public string? Description { get; set; }

        [DisplayName("Ticket Note")]
        public string? Note { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Updated")]
        public DateTime? Updated { get; set; }

        public bool Archived { get; set; }

        [DisplayName("Archived By Project")]
        public bool ArchivedByProject { get; set; }


        // Navigation Properties
        //public virtual Contact? Contact { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Account? Account { get; set; }

        [DisplayName("Priority")]
        public virtual TicketPriority? TicketPriority { get; set; }
        [DisplayName("Status")]
        public virtual TicketStatus? TicketStatus { get; set; }
        [DisplayName("Type")]
        public virtual TicketType? TicketType { get; set; }

        [DisplayName("Submitted By")]
        public virtual TAUser? SubmitterUser { get; set; }

        [DisplayName("Ticket Developer")]
        public virtual TAUser? DeveloperUser { get; set; }

        public virtual ICollection<TicketTag> Tags { get; set; } = new HashSet<TicketTag>();

        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();
    }
}
