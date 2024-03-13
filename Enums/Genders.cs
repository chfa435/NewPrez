using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Enums
{
    public enum Genders
    {
        [Display(Name="<select gender>")]
        None,
        Male,
        Female,
    }
}
