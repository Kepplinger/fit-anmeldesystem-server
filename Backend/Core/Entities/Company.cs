using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Company : EntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("FK_Address")]
        public Address Address { get; set; }
        public int FK_Address { get; set; }

        [ForeignKey("FK_Contact")]
        public Contact Contact { get; set; }
        public int FK_Contact { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Homepage { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        public string Branch { get; set; }

        public int EstablishmentsCountInt { get; set; }

        public string EstablishmentsInt { get; set; }

        [Required]
        public int EstablishmentsCountAut { get; set; }

        [Required]
        public string EstablishmentsAut { get; set; }
    }
}