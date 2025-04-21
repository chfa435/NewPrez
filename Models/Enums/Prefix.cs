using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models.Enums
{
    public enum Prefix
    {
        [Display(Name = "Mr.")]
        Mr,
        [Display(Name = "Mrs.")]
        Mrs,
        [Display(Name = "Ms.")]
        Ms,
        [Display(Name = "Dr.")]
        Dr,
        [Display(Name = "Prof.")]
        Prof,
        [Display(Name = "Rev.")]
        Rev
    }
} 