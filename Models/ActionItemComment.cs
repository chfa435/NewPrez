using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class ActionItemComment
    {
        private DateTime _dateCreated;

        // Primary Key
        public int Id { get; set; }

        [Required]
        [DisplayName("Member Comment")]
        [StringLength(2000)]
        public string? Comment { get; set; }

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

        // Foreign Key
        public int ActionItemId { get; set; }

        // Foreign Key
        [Required]
        public string? SubmitterId { get; set; }

        // Navigation Properties
        [DisplayName("Action Item")]
        public virtual ActionItem? ActionItem { get; set; }

        [DisplayName("Author")]
        public virtual TAUser? Submitter { get; set; }

    }
}
