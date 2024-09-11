using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NewTiceAI.Models.Enums;

namespace NewTiceAI.Models
{
    public class Opportunity
    {
        private DateTime _dateCreated;
        private DateTime? _dateClosed;

        // Primary Key
        public int Id { get; set; }

        // Foreign Key - Allow create on submit
        public int? ContactId { get; set; }

        // Foreign Key
        [Required]
        public string? SubmitterId { get; set; }


        [DisplayName("Account Name")]
        public int? AccountId { get; set; }

        [DisplayName("Project Name")]
        public int? ActionProjectId { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Type")]
        public EnumActionItemTypes ItemType { get; set; }

        [DisplayName("Status")]
        public EnumActionItemStatuses ItemStatus { get; set; }

        [DisplayName("Priority")]
        public EnumActionItemPriorities ItemPriority { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Description { get; set; }

        [DisplayName("Notes")]
        public string? Notes { get; set; }

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
        [DisplayName("Date Closed")]
        public DateTime? ClosedDate
        {
            get => _dateClosed;
            set
            {
                if (value.HasValue)
                {
                    _dateClosed = value.Value.ToUniversalTime();
                }
                else
                {
                    _dateClosed = null;
                }
            }
        }


        // Navigation Properties
        public virtual Contact? Contact { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ActionProject? ActionProject { get; set; }

        [DisplayName("Submitted By")]
        public virtual TAUser? Submitter { get; set; }
    }

}
