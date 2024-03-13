using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewTiceAI.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public TAUser? BTUser { get; set; }

        public MultiSelectList? Roles { get; set; }

        public List<string>? SelectedRoles { get; set; }    
    }
}
