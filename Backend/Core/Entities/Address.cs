using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities

{
    public class Address : EntityObject {
        
        [Required]
        [MaxLength(25)]
        public string City { get; set; }

        [Required]
        [MaxLength(7)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string  Number { get; set; }

        public string AddressAdditional {get; set;}
        
    }
}