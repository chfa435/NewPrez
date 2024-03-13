using System.Collections.Generic;

namespace NewTiceAI.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Organization? Organization { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Project> Projects { get; set; } = new List<Project>(); 
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        public List<TAUser> Members { get; set; } = new List<TAUser>();
    }
}
