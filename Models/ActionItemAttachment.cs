using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NewTiceAI.Extensions;

namespace NewTiceAI.Models
{
    public class ActionItemAttachment
    {
        private DateTime _dateCreated;

        // Primary Key
        public int Id { get; set; }

        [DisplayName("File Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Added")]
        public DateTime Created
        {
            get
            {
                return _dateCreated;
            }

            set
            {
                _dateCreated = value.ToUniversalTime();
            }
        }

        // Foreign Key
        public int ActionItemId { get; set; }

        // Foreign Key
        [Required]
        public string? SubmitterId { get; set; }

        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".ppt", ".pptx", ".html", ".svg" })]
        public IFormFile? FormFile { get; set; }

        [DisplayName("File Name")]
        public string? FileName { get; set; }

        [DisplayName("File Attachment")]
        public byte[]? FileData { get; set; }

        [DisplayName("File Extension")]
        public string? FileContentType { get; set; }


        // Navigation Properties
        [DisplayName("Ticket")]
        public virtual ActionItem? ActionItem { get; set; }

        [DisplayName("Team Member")]
        public virtual TAUser? Submitter { get; set; }


    }
}
