using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.Entities
{
    public class LockPage : TimestampEntityObject
    {
        [Required]
        public string Expired { get; set; }
        [Required]
        public string Incoming { get; set; }
    }
}
