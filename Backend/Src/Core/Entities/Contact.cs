using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Contact : TimestampEntityObject
    {
        [Required, MaxLength(15)]
        public string FirstName { get; set; }

        [Required, MaxLength(15)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}