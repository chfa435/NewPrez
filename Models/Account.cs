using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewTiceAI.Models
{
    public class Account
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key
        // Some Accounts may be their own Org //
        public int? ParentOrganizationId { get; set; }

        // Foreign Key
        // This will be the person that created the account
        //public string? AccountOwnerId { get; set; }

        // Foreign Key
        public int? BillingAddressId { get; set; }

        // Foreign Key
        public int? ShippingAddressId { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string? Name { get; set; }


        public decimal? AnnualRevenue { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }

        //public string? Industry { get; set; }
        //public string? Ticker { get; set; }
        //public string? SICCode { get; set; }
        //public string? SICDescription { get; set; }


        public virtual ICollection<TAUser> AccountMembers { get; set; } = new HashSet<TAUser>(); 
        public virtual Organization? ParentOrganization { get; set; }
        //public virtual TAUser? AccountOwner { get; set; }
        public virtual ICollection<ActionProject> Projects { get; set; } = new HashSet<ActionProject>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
        public virtual Address? BillingAddress { get; set; }
        public virtual Address? ShippingAddress { get; set; }

        public virtual ICollection<ActionItem> Opportunities { get; set; } = new HashSet<ActionItem>();

        //TODO: Knowledge Articles//
        //public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
    }
}