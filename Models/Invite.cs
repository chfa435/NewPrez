using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class Invite
    {
        // Primary Key
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Sent")]
        public DateTime InviteDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Joined")]
        public DateTime? JoinDate { get; set; }

        public Guid OrganizationToken { get; set; }

        public int OrganizationId { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public string? InvitorId { get; set; }

        public string? InviteeId { get; set; }

        [Required]
        [DisplayName("Email")]
        public string? InviteeEmail { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string? InviteeFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string? InviteeLastName { get; set; }

        public string? Message { get; set; }

        public bool IsValid { get; set; }

        // Navigation Properties

        public virtual Organization? Organization { get; set; }
        public virtual Project? Project { get; set; }
        public virtual TAUser? Invitor { get; set; }
        public virtual TAUser? Invitee { get; set; }

    }
}
