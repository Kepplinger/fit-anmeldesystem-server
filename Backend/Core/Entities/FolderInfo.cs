using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class FolderInfo : EntityObject
    {

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress, Required]
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

        
        public int EstablishmentsCountAut { get; set; }

        [MaxLength(30)]
        public string EstablishmentsAut { get; set; }
    }
}
