using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.Entities
{
    public class SmtpConfig : TimestampEntityObject {

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required, EmailAddress]
        public string MailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
