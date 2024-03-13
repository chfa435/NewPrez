using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class ActionItemHistory
    {
        private DateTime _dateCreated;

        // Primary Key
        public int Id { get; set; }

        [DisplayName("Modified Property")]
        public string? PropertyName { get; set; }

        [DisplayName("Description of Modification")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Modified")]
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

        [DisplayName("Previous Value")]
        public string? OldValue { get; set; }

        [DisplayName("Updated Value")]
        public string? NewValue { get; set; }

        public int ActionItemId { get; set; }

        [Required]
        public string? SubmitterId { get; set; }

        // Navigation Properties

        public virtual ActionItem? ActionItem { get; set; }

        public virtual TAUser? Submitter { get; set; }
    }
}
