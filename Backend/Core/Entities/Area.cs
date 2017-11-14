
using Backend.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class Area : EntityObject
    {
        [Required]
        public string Designation { get; set; }
        [Required]
        public string GraphicURL { get; set; }
        
        [ForeignKey("FK_Event")]
        public Event Event { get; set; }

        public int FK_Event { get; set; }
    }
}