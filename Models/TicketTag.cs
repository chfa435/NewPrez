using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class TicketTag
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
