using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using NewTiceAI.Extensions;

namespace NewTiceAI.Models
{
    public class Project
    {
        private DateTime _created;
        private DateTime _startDate;
        private DateTime _endDate;


        // Primary Key
        public int Id { get; set; }

        //// Foreign Key
        //public int CompanyId { get; set; }
        // Foreign Key
        public int AccountId { get; set; }
        // Foreign Key
        public int ProjectPriorityId { get; set; }


        [Required]
        [StringLength(240, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [DisplayName("Project Name")]
        public string? Name { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [DisplayName("Project Description")]
        public string? Description { get; set; }


        [Required]
        [DisplayName("Date Created")]
        [DataType(DataType.Date)]
        public DateTime Created
        {
            get
            {
                return _created;
            }

            set
            {
                _created = value.ToUniversalTime();
            }
        }

        [DataType(DataType.Date)]
        [DisplayName("Project Start Date")]
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value.ToUniversalTime();
            }
        }

        [DataType(DataType.Date)]
        [DisplayName("Project End Date")]
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                _endDate = value.ToUniversalTime();
            }
        }


        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]   // 1 MB = 1048576 bytes (= 1024^2 B) = 2^20 B
        public IFormFile? ImageFormFile { get; set; }

        [DisplayName("File Name")]
        public string? ImageFileName { get; set; }

        [DisplayName("Project Image")]
        public byte[]? ImageFileData { get; set; }

        [DisplayName("File Extension")]
        public string? ImageContentType { get; set; }


        public bool Archived { get; set; }

        // Navigation Properties
        //public virtual Company? Company { get; set; }
        public virtual Account? Account { get; set; }

        public virtual ProjectPriority? ProjectPriority { get; set; }
        public virtual ICollection<TAUser> Members { get; set; } = new HashSet<TAUser>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
