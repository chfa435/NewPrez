using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class ActionItemStatus
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Status")]
        public string? Name { get; set; }
    }
}
