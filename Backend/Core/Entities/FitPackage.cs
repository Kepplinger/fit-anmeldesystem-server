using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class FitPackage : EntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Discriminator { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
