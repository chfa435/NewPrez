namespace NewTiceAI.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int? AddressId { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();

        public virtual ICollection<TAUser> Members { get; set; } = new HashSet<TAUser>();    
        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
        public virtual Address? Address { get; set; }

    }
}
