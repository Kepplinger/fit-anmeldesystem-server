﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Graduate : EntityObject
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [ForeignKey("FK_Address"), Required]
        public Address Address { get; set; }

        public int FK_Address { get; set; }

        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required, JsonIgnore]
        public string RegistrationToken { get; set; }
    }
}