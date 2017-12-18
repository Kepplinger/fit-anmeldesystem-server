using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities

{
    public class Address : EntityObject {
        
        [Required]
        [MaxLength(25)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string StreetNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        public string Addition {get; set;}
        
    }
}