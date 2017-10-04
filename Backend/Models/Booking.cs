using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Booking : IEntityObject
    {
        [ForeignKey("FK_Event")]
        public Event Event { get; set; }

        public int FK_Event { get; set; }

        [ForeignKey("FK_Company")]
        public Company Company { get; set; }

        public int FK_Company { get; set; }
        [ForeignKey("FK_Location")]
        public Location Location { get; set; }
        public int FK_Location { get; set; }

        [ForeignKey("FK_Presentation")]
        public Presentation Presentation { get; set; }

        public int FK_Presentation { get; set; }

        [ForeignKey("FK_Category")]
        public Category Category { get; set; }
        public int FK_Category { get; set; }
        [Required]
        public bool isAccepted { get; set; }
    }
}