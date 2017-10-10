using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models.Validation;

namespace Backend.Entities
{
    public class Company : IEntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(350)]
        public string ShortDescription { get; set; }
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
        [CustomValidation(typeof(EmailValidation), "EmailRules")]
        public string Email { get; set; }
        [Required]
        [CustomValidation(typeof(HomepageValidation),"LinkRules")]
        public string Homepage { get; set; }
        [Required]
        public byte[] CompanySign { get; set; }

        [Required]
        public string SubjectAreas { get; set; }
    }
}