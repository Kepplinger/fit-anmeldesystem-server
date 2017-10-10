using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models.Validation;

namespace Backend.Models
{
    public class Person : IEntityObject
    {
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }
        [Required]
        [CustomValidation(typeof(EmailValidation),"EmailRules")]
        public string Email { get; set; }
        [Required]
        public byte[] Picture { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set;}
    }
}