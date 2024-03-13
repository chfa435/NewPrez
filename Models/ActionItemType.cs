using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class ActionItemType
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Type")]
        public string? Name { get; set; }
    }
}
