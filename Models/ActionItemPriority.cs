using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class ActionItemPriority
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Priority")]
        public string? Name { get; set; }
    }
}
