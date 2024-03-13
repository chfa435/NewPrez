using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NewTiceAI.Models.Enums;

namespace NewTiceAI.Models
{
    public class ActionItem
    {
        private DateTime _dateCreated;
        private DateTime? _dateUpdated;

        // Primary Key
        public int Id { get; set; }

        // Foreign Key - Allow create on submit
        public int? ContactId { get; set; }

        // Foreign Key
        [Required]
        public string? SubmitterId { get; set; }

        [DisplayName("Item Actor")]
        public string? ActorId { get; set; }

        [DisplayName("Account Name")]
        public int? AccountId { get; set; }

        [DisplayName("Project Name")]
        public int? ActionProjectId { get; set; }

        [DisplayName("Type")]
        public EnumActionItemTypes ItemType { get; set; }

        [DisplayName("Status")]
        public EnumActionItemStatuses ItemStatus { get; set; }

        [DisplayName("Priority")]
        public EnumActionItemPriorities ItemPriority { get; set; }

        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Description { get; set; }

        [DisplayName("Notes")]
        public string? Note { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Added")]
        public DateTime Created
        {
            get
            {
                return _dateCreated;
            }

            set
            {
                _dateCreated = value.ToUniversalTime();
            }
        }

        [DataType(DataType.Date)]
        [DisplayName("Date Updated")]
        public DateTime? Updated
        {
            get => _dateUpdated;
            set
            {
                if (value.HasValue)
                {
                    _dateUpdated = value.Value.ToUniversalTime();
                }
                else
                {
                    _dateUpdated = null;
                }
            }
        }

        public bool Archived { get; set; }


        // Navigation Properties
        public virtual Contact? Contact { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ActionProject? ActionProject { get; set; }
        //[DisplayName("Priority")]
        //public virtual ActionItemPriority? ItemPriority { get; set; }
        //[DisplayName("Status")]
        //public virtual ActionItemStatus? ItemStatus { get; set; }
        //[DisplayName("Type")]
        //public virtual ActionItemType? ItemType { get; set; }

        [DisplayName("Submitted By")]
        public virtual TAUser? Submitter { get; set; }

        [DisplayName("Ticket Developer")]
        public virtual TAUser? Actor { get; set; }

        public virtual ICollection<ActionItemComment> Comments { get; set; } = new HashSet<ActionItemComment>();
        public virtual ICollection<ActionItemAttachment> Attachments { get; set; } = new HashSet<ActionItemAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<ActionItemHistory> History { get; set; } = new HashSet<ActionItemHistory>();
    }

}
