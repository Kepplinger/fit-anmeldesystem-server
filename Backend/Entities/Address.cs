using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Entities

{
    public class Address : IEntityObject {
        
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
        [MaxLength(5)]
        public string  Number { get; set; }
        
    }
}