using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using NewTiceAI.Models.Enums;

namespace NewTiceAI.Models
{
    public class Address
    {
        public int Id { get; set; }

        //[Required]
        public string? Address1 { get; set; }

        [Display(Name = "Apt, Suiite, Sector #")]
        public string? Address2 { get; set; }

        //[Required]
        public string? City { get; set; }

        //[Required]
        public States? State { get; set; }

        //[Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public int? ZipCode { get; set; }

    }
}
