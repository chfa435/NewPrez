﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewTiceAI.Models.ViewModels
{
    public class ProjectMembersViewModel
    {
        public Project? Project { get; set; }  

        public MultiSelectList? UsersList { get; set; }

        public List<string>? SelectedUsers { get; set; }  

    }
}
