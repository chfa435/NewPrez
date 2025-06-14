﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models
{
    public class Company
    {
        // Primary Key
        public int Id { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string? Name { get; set; }

        [DisplayName("Company Description")]
        public string? Description { get; set; }


        // Navigation Properties

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public virtual ICollection<TAUser> Members { get; set; } = new HashSet<TAUser>();
        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();

    }
}
