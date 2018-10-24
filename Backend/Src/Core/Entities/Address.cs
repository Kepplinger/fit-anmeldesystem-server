using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities

{
    public class Address : TimestampEntityObject {
        
        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string StreetNumber { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public string Addition {get; set;}
    }
}