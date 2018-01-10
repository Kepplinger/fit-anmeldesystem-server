using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Company : EntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("FK_Address"),Required]
        public Address Address { get; set; }
        public int FK_Address { get; set; }

        [ForeignKey("FK_Contact"),Required]
        public Contact Contact { get; set; }
        public int FK_Contact { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress,Required]
        public string Email { get; set; }

        [Required, Url]
        public string Homepage { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required, MaxLength(20)]
        public string Branch { get; set; }

        public int EstablishmentsCountInt { get; set; }

        [MaxLength(30)]
        public string EstablishmentsInt { get; set; }

        [Required]
        public int EstablishmentsCountAut { get; set; }

        [Required, MaxLength(30)]
        public string EstablishmentsAut { get; set; }
    }
}