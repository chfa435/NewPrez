using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NewTiceAI.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NewTiceAI.Models
{
    public class ContactAttachment
    {
        private DateTime _created;

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
                return _created;
            }

            set
            {
                _created = value.ToUniversalTime();
            }
        }

        // Foreign Key
        public int ContactId { get; set; }

        // Foreign Key
        [Required]
        public string? UserId { get; set; }

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
        [DisplayName("Contact")]
        public virtual Contact? Contact { get; set; }

        [DisplayName("Team Member")]
        public virtual TAUser? User { get; set; }


    }

}
