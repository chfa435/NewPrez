using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using NewTiceAI.Models.Enums;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities;
using System.Runtime.Intrinsics.Arm;
using NewTiceAI.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NewTiceAI.Models
{
    public class Contact
    {
        private DateTime _dateAdded;
        private DateTime? _dateOfBirth;
        //private DateTime? _resGradYear;
        //private DateTime? _fellGradYear;
        //private DateTime? _fellGradYear2;
        private DateTime? _lastMeeting;
        private DateTime? _nextActivity;
        private DateTime? _followUp;
        private DateTime? _packetSent;


        // Primary Key
        public int Id { get; set; }


        public int OrganizationId { get; set; }

        //// Foreign Key
        [DisplayName("Select an account")]
        public int? AccountId { get; set; }

        // Foreign Key
        public int? AddressId { get; set; }

        //// Foreign Key
        //[Display(Name ="Reports To")]
        //public string? DirectReportId { get; set; }

        // Foreign Key
        public string? ContactOwnerId { get; set; }  //???

        //public string ContactOriginator { get; set; }  //???

        //public string ContactEditedBy { get; set; }  //???

        // Foreign Key
        public int? ImportId { get; set; }


        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth {
            get => _dateOfBirth;
            set
            {
                if (value.HasValue)
                {
                    _dateOfBirth = value.Value.ToUniversalTime();
                }
                else
                {
                    _dateOfBirth = null;
                }
            }
        }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Phone]
        [Display(Name = "Mobile Phone Number")]
        public string? Mobile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAdded
        {
            get
            {
                return _dateAdded;
            }

            set
            {
                _dateAdded = value.ToUniversalTime();
            }
        }

        // Image properties
        //public byte[]? ImageData { get; set; }
        //public string? ImageType { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        public Guid? ImageId { get; set; }   
        public FileUpload? Image { get; set; }



        public string? Title { get; set; }

        public bool DoNotCall { get; set; }

        public bool EmailOptOut { get; set; }

        public bool IsActive { get; set; }



        // ************ TEST PROPERTIES ************ //
        // ***************************************** //
        #region TEST PROPERTIES

        //MD/DO/DPM  ???

        //Year/Rank  ???

        //NPI
        //[Display(Name = "NPI Number")]
        //public string? NPI { get; set; } ??? string or number? 

        //Gender
        public Genders? Gender { get; set; }

        //Specialty
        public string? Specialty { get; set; }   // This can easily become a foreign key to a Specialty table

        //Residency
        [Display(Name = "Residency")]
        public int? ResidencyId { get; set; }

        //Residency Graduation Year
        public int? Residency_GradYear { get; set; }
        //{
        //    get => _resGradYear;
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            _resGradYear = value.Value.ToUniversalTime();
        //        }
        //        else
        //        {
        //            _resGradYear = null;
        //        }
        //    }
        //}

        //Fellowship
        [Display(Name = "Fellowship")]
        public int? FellowshipId { get; set; }

        //Fellowship Graduation Year
        public int? Fellowship_GradYear { get; set; }  
        //{
        //    get => _fellGradYear;
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            _fellGradYear = value.Value.ToUniversalTime();
        //        }
        //        else
        //        {
        //            _fellGradYear = null;
        //        }
        //    }
        //}


        //Fellowship 2
        [Display(Name="Fellowship Two")]
        public int? Fellowship2Id { get; set; }

        //Fellowship 2 Graduation Year
        public int? Fellowship2_GradYear { get; set; } 
        //{
        //    get => _fellGradYear2;
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            _fellGradYear2 = value.Value.ToUniversalTime();
        //        }
        //        else
        //        {
        //            _fellGradYear2 = null;
        //        }
        //    }
        //}


        //Key Relationship Holder
        //<link another contact>
        [Display(Name = "Relationship Holder")]
        public int? RelationshipHolderId { get; set; }


        //Current Distributor
        [Display(Name = "Current Distributor")]
        public int? CurrentDistributorId { get; set; }  // ??? Institution


        //Current Sales Representative
        //<link another contact>
        [Display(Name = "Sales Representative")]
        public int? SalesRepresentativeId { get; set; }


        //Practice
        [Display(Name = "Practice")]
        public int? PracticeId { get; set; }    // ??? Institution

        //Practice City//
        //Practice State//
        //public int? PracticeAddressId { get; set; }


        //Hospital(s) 
        //Hospital System
        //
        //public Institution? HospitalSystem { get; set; }
        //public Institution? Hospital { get; set; }
        //Hospital 2
        //Hospital 3


        //Mentor
        //<link another contact>
        [Display(Name = "Mentor")]
        public int? MentorId { get; set; }


        //Product Interest  ???

        //Last Meeting Date
        //<need to set date or be null>
        public DateTime? LastMeetingDate
        {
            get => _lastMeeting;
            set
            {
                if (value.HasValue)
                {
                    _lastMeeting = value.Value.ToUniversalTime();
                }
                else
                {
                    _lastMeeting = null;
                }
            }
        }


        //Next Activity
        //<need to set date or be null>
        public DateTime? NextActivityDate
        {
            get => _nextActivity;
            set
            {
                if (value.HasValue)
                {
                    _nextActivity = value.Value.ToUniversalTime();
                }
                else
                {
                    _nextActivity = null;
                }
            }
        }


        //Product Focus  ???


        //Follow up Date
        //<need to set date or be null>
        public DateTime? FollowupDate
        {
            get => _followUp;
            set
            {
                if (value.HasValue)
                {
                    _followUp = value.Value.ToUniversalTime();
                }
                else
                {
                    _followUp = null;
                }
            }
        }


        //Confirmed Handoff  ???
        public bool HandoffConfirmed { get; set; }

        //Notes
        [Display(Name = "Contact Notes")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string? ContactNotes { get; set; }

        //Profile URL   ???
        public string? ProfileUrl { get; set; }

        //Profile URL 2   ???
        public string? ProfileUrl2 { get; set; }

        //Welcome Packet Sent
        //<need to set date or be null>
        public DateTime? PacketSentDate
        {
            get => _packetSent;
            set
            {
                if (value.HasValue)
                {
                    _packetSent = value.Value.ToUniversalTime();
                }
                else
                {
                    _packetSent = null;
                }
            }
        }

        // Navigation Properties

        public virtual Contact? RelationshipHolder { get; set; }
        public virtual Contact? SalesRepresentative { get; set; }
        public virtual Contact? Mentor { get; set; }
        public virtual Institution? CurrentDistributor { get; set; }
        public virtual Institution? Practice { get; set; }
        public virtual Institution? Residency { get; set; }
        public virtual Institution? Fellowship { get; set; }
        public virtual Institution? Fellowship2 { get; set; }

        #endregion
        // ************ END TEST PROPERTIES ************ //
        // ********************************************* //



        public virtual Organization? Organization { get; set; }
        public virtual Account? Account { get; set; }
        public virtual TAUser? ContactOwner { get; set; }
        public virtual Address? Address { get; set; }
        public virtual Import? Import {  get; set; } 


        public ICollection<Institution> Hospitals { get; set; } = new HashSet<Institution>();
        public ICollection<ActionItem> ActionItems { get; set; } = new HashSet<ActionItem>();
        public virtual ICollection<ContactAttachment> Attachments { get; set; } = new HashSet<ContactAttachment>();


    }
}
