namespace NewTiceAI.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? AddressId { get; set; }

        //public ICollection<Contact> Associates { get; set; } = new HashSet<Contact>();
        public virtual Address? Address { get; set; }
    }
}
